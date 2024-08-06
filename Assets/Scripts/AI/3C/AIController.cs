
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using System.Linq;
using AI.TokenPool;
using Mirror;
using IronAngel;
using Random = UnityEngine.Random;
using LogicState;
using System;

using Utils = IronAngel.Utils;

public class AIController : NetworkBehaviour
{
    private AIMovement m_AIMovement;
    private DamageSensor m_DamageSensor;
    private VisionSensor m_VisionSensor;
    private GameObject m_ChaseGO;
    private BaseProperties m_BaseProperties;
    private LogicStateManager m_LogicStateManager;
    
    private GameObject m_LeftHandWeapon;
    private GameObject m_RightHandWeapon;


    /// <summary>
    /// 训练路线
    /// </summary>
    public SplineContainer PatrolRoute;
    // Start is called before the first frame update
    void Start()
    {
        m_BaseProperties = GetComponent<BaseProperties>();
        
        m_LogicStateManager = GetComponent<LogicStateManager>();
        
        m_AIMovement = GetComponent<AIMovement>();
        
        m_VisionSensor = GetComponent<VisionSensor>();
        
        m_DamageSensor = GetComponent<DamageSensor>();
        m_DamageSensor.SetNotifyPerceivedDelegate(BeDamaged);


        EventCenter.AddListener<LogicStateManager,ELogicState>(
            EventType.LogicState_AIAttacking_StateOut,
            OnAIAttackingStateOut
            );
        
        RegisterWeapon();
    }

    [ServerCallback]
    public void BeDamaged()
    {
        if (m_BaseProperties.m_Properties.m_AutoAvoid)
        {
            Avoid();
        }
    }

    /// <summary>
    /// 受到伤害时概率触发躲避
    /// </summary>
    /// <param name="attacker"></param>
    [ServerCallback]
    public void Avoid()
    {
        if(!Utils.RandomBool(m_BaseProperties.m_Properties.m_AutoAvoid_Probability))
        {
            return;
        }

        if (m_LogicStateManager.IncludeState(ELogicState.PlayerDashing))
        {
            return;
        }
        
        // 一半的概率向左冲刺，一半的概率向右冲刺
        if (Utils.RandomBool(0.5f))
        {
            Dash(transform.rotation*Vector3.left);
        }
        else
        {
            Dash(transform.rotation*Vector3.right);
        }
        
        
        m_BaseProperties.m_Properties.m_AutoAvoid_RestTimes -= 1;
    }
    
    

    [ServerCallback]
    private void Dash(Vector2 dashDir)
    {
        m_AIMovement.Dash(dashDir);
    }

    [ServerCallback]
    private void RegisterWeapon()   
    {
        WeaponSystemCenter.Instance.RegisterAIWeapon(this);
    }
    
    
    public void SetLeftHandWeapon(GameObject weapon)
    {
        m_LeftHandWeapon = weapon;
    }

    public void SetRightHandWeapon(GameObject weapon)
    {
        m_RightHandWeapon = weapon;
    }

    public WeaponType GetLeftHandWeaponType()
    {
        return m_BaseProperties.m_Properties.m_LeftHandWeapon;
    }

    public WeaponType GetRightHandWeaponType()
    {
        return m_BaseProperties.m_Properties.m_RightHandWeapon;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    

    [ServerCallback]
    public virtual void Patrol()
    {
        if (PatrolRoute != null)
        {
            m_AIMovement.PatrolWithFixedRoute(PatrolRoute);
        }
        else
        {
            Debug.Log("没有设置巡逻路线");
        }
    }

    [ServerCallback]
    public virtual void Chase()
    {
        var chaseGameObjects = GetPerceiveGameObjects();
        if (chaseGameObjects.Count > 0)
        {
            m_ChaseGO = chaseGameObjects[0];
            //Debug.Log("AI正在追逐"+chaseGameObjects[0]);
            m_AIMovement.Chase(chaseGameObjects[0]);
        }
        else
        {
            if (m_ChaseGO!=null)
            {
                m_AIMovement.Chase(m_ChaseGO);
            }

        }
    }
    [ServerCallback]
    public virtual bool SetDestination(Vector3 target)
    {
        return m_AIMovement.SetDestination(target);
    }
    
    [ServerCallback]
    public List<GameObject> GetPerceiveGameObjects()
    {
        List<GameObject> result =
            (m_DamageSensor.GetPerceiveGameObjects().Concat(m_VisionSensor.GetPerceiveGameObjects())).ToList();
        return result;
    }

    [ServerCallback]
    public List<GameObject> GetGameObjectsInAttackRange()
    {
        var result = GetPerceiveGameObjects();

        if (result.Count <= 0)
        {
            return result;
            //Debug.Log("GetGameObjectsInAttackRange() result empty");
        }
        
        // 移除超出攻击范围的
        result.RemoveAll(o =>
            Vector3.Distance(o.transform.position, transform.position) > 
            m_BaseProperties.m_Properties.m_AttackRange
            );

        return result;
    }

    [ServerCallback]
    public bool Attack()
    {
        if (m_LogicStateManager.IncludeState(ELogicState.AIAttacking))
        {
            return false;
        }
        
        var enemy = GetGameObjectsInAttackRange();
        if (enemy.Count <= 0)
        {
            return false;
        }
        
        
        if (m_LeftHandWeapon == null && m_RightHandWeapon == null)
        {
            return false;
        }
        
        
        if (!TokenPool.ApplyToken(m_BaseProperties.m_Properties.m_TokenWeight) == false)
        {
            return false;
        }

        
        m_LogicStateManager.AddState(ELogicState.AIAttacking);
        
        if (!m_LogicStateManager.IncludeState(ELogicState.AIAttacking))
        {
            return false;
        }
       

        var leftFire = IronAngel.Utils.RandomBool(m_BaseProperties.m_Properties.m_ProbabilityOfLeftWeapon);
        var rightFire = IronAngel.Utils.RandomBool(m_BaseProperties.m_Properties.m_ProbabilityOfRightWeapon);

        if (leftFire == false && rightFire == false)
        {
            if (IronAngel.Utils.RandomBool(0.5f))
            {
                leftFire = true;
            }
            else
            {
                rightFire = true;
            }
        }
       
        
        if (leftFire)
        {
            LeftHandFire();
        }
        
        if(rightFire)
        {
            RightHandFire();
        }
        
        return true;
    }

    [ServerCallback]
    private void LeftHandFire()
    {
        var leftDuration = m_BaseProperties.m_Properties.m_LeftHandWeaponAttackingDuration;
        m_LogicStateManager.SetStateDuration(ELogicState.AIAttacking, leftDuration);
        StartCoroutine(LeftHandFireCoroutine());
    }

    [ServerCallback]
    private void RightHandFire()
    {
        var rightDuration = m_BaseProperties.m_Properties.m_RightHandWeaponAttackingDuration;
        m_LogicStateManager.SetStateDuration(ELogicState.AIAttacking, rightDuration);
        StartCoroutine(RightHandFireCoroutine());
    }
    
    private IEnumerator LeftHandFireCoroutine()
    {
        while (m_LogicStateManager.IncludeState(ELogicState.AIAttacking))
        {
            var dir = transform.rotation*Vector3.up;
            dir = ComputeAngleOfFire(dir);
            WeaponSystemCenter.Instance.CmdFire(gameObject, m_LeftHandWeapon,transform.position,dir);
            yield return null;
        }
    }
    
    private IEnumerator RightHandFireCoroutine()
    {
        while (m_LogicStateManager.IncludeState(ELogicState.AIAttacking))
        {
            var dir = transform.rotation*Vector3.up;
            dir = ComputeAngleOfFire(dir);
            WeaponSystemCenter.Instance.CmdFire(gameObject, m_RightHandWeapon,transform.position,dir);
            yield return null;
        }
    }
    
    
    /// <summary>
    /// 以m_BaseProperties.m_Properties.m_RangeOfAimingError
    /// 计算在此范围内偏转的随机方向
    /// </summary>
    /// <param name="originDir"></param>
    /// <returns></returns>
    private Vector3 ComputeAngleOfFire(Vector3 originDir)
    {
        var result = originDir.normalized;
        Quaternion quaternion = Quaternion.LookRotation(result);
        Vector3 eulerAngles = quaternion.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.y = 0;
        var rangeOfAimingError = m_BaseProperties.m_Properties.m_RangeOfAimingError;
        eulerAngles.z -= rangeOfAimingError/2;
        float randomValue = (float) Random.Range(0, rangeOfAimingError);
        eulerAngles.z += randomValue;

        quaternion = Quaternion.Euler(eulerAngles);
        result = quaternion * result;
        result.z = 0;
        result = result.normalized;
        return result;
    }

    [ServerCallback]
    private void OnAIAttackingStateOut(LogicStateManager logicStateManager,ELogicState eLogicState)
    {
        if (logicStateManager != m_LogicStateManager || eLogicState != ELogicState.AIAttacking)
        {
            return;
        }
        TokenPool.ReturnToken();
        
    }

    
}
