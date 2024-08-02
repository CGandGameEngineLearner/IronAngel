
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using AI.TokenPool;
using Mirror;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using LogicState;

public class AIController : NetworkBehaviour
{
    private AIMovement m_AIMovement;
    private IAISensor m_AISensor;
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
        m_AISensor = GetComponent<IAISensor>();


        EventCenter.AddListener<LogicStateManager,ELogicState>(
            EventType.LogicState_AIAttacking_StateOut,
            OnAIAttackingStateOut
            );
        
        RegisterWeapon();
       
    }

    [ServerCallback]
    private void RegisterWeapon()
    {
        WeaponSystemCenter.RegisterAIWeapon(this);
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
        var chaseGameObjects = m_AISensor.GetPerceiveGameObjects();
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
        return m_AISensor.GetPerceiveGameObjects();
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

    
    public bool Attack()
    {
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
        
        // 随机使用左右手武器
        int rand = Random.Range(0, 1);
        
        if (rand == 0)
        {
            m_LogicStateManager.AddState(ELogicState.AIAttacking);
            m_LogicStateManager.SetStateDuration(ELogicState.AIAttacking, m_BaseProperties.m_Properties.m_LeftHandWeaponAttackingDuration);
            var dir = enemy[0].transform.position - transform.position;
            dir = ComputeAngleOfFire(dir);
        
            Debug.DrawLine(transform.position,transform.position + 10*dir,Color.red,10);
            WeaponSystemCenter.Instance.CmdFire(gameObject, m_LeftHandWeapon,transform.position,dir);
        }
        else
        {
            m_LogicStateManager.SetStateDuration(ELogicState.AIAttacking, m_BaseProperties.m_Properties.m_RightHandWeaponAttackingDuration);
            var dir = enemy[0].transform.position - transform.position;
            dir = ComputeAngleOfFire(dir);
        
            Debug.DrawLine(transform.position,transform.position + 10*dir,Color.red,10);
            WeaponSystemCenter.Instance.CmdFire(gameObject, m_RightHandWeapon,transform.position,dir);
        }
        
        
        
        return true;
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
