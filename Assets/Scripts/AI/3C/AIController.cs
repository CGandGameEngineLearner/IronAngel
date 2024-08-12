
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
using Unity.VisualScripting;
using Utils = IronAngel.Utils;

public class AIController : NetworkBehaviour
{
    private AIMovement m_AIMovement;
    private DamageSensor m_DamageSensor;
    private VisionSensor m_VisionSensor;
    private GameObject m_ChaseGO;
    private BaseProperties m_BaseProperties;
    private LogicStateManager m_LogicStateManager;
    
    

    [Tooltip("左手位置的标记GameObject")]
    public GameObject LeftHand;
    
    [Tooltip("右手位置的标记GameObject")]
    public GameObject RightHand;
    

    [Tooltip("AI是否可以在受到伤害时自动躲闪以下")]
    public bool m_AutoAvoid;

    [Tooltip("AI躲避的概率"),Range(0,1)]
    public float m_AutoAvoid_Probability;

    [Tooltip("允许AI躲避的剩余次数")]
    public float m_AutoAvoid_RestTimes;

    [Tooltip("AI抢攻击Token的优先权重"),Range(0,1)]
    public float m_TokenWeight;

    [Tooltip("AI的射击角度误差范围"),Range(0,90)]
    public float m_RangeOfAimingError;
    
    
    [Tooltip("左手武器每次攻击的持续时长(一定要大于前摇时长，因为攻击时长包含了前摇时长)")]
    public float m_LeftHandWeaponAttackingDuration;
    
    [Tooltip("右手武器每次攻击的持续时长(一定要大于前摇时长，因为攻击时长包含了前摇时长)")]
    public float  m_RightHandWeaponAttackingDuration;
    
    [Tooltip("左手武器使用概率"),Range(0,1)]
    public float m_ProbabilityOfLeftWeapon;

    [Tooltip("右手武器使用概率"),Range(0,1)]
    public float m_ProbabilityOfRightWeapon;
    

    /// <summary>
    /// 训练路线
    /// </summary>
    public SplineContainer PatrolRoute;
    // Start is called before the first frame update
    void OnEnable()
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
    private void OnDisable()
    {
        // 移除攻击状态以归还Token
        m_LogicStateManager.RemoveState(ELogicState.AIAttacking);
    }

    [ServerCallback]
    public void BeDamaged()
    {
        if (m_AutoAvoid)
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
        if (m_AutoAvoid_RestTimes <= 0)
        {
            return;
        }
        
        if(!Utils.RandomBool(m_AutoAvoid_Probability))
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
        
        
        m_AutoAvoid_RestTimes -= 1;
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
        if (!weapon) return;
        
        m_BaseProperties.m_Properties.m_LeftWeaponGO = weapon;
        weapon.transform.SetParent(LeftHand.transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        
        // 隐藏AI武器
        weapon.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetRightHandWeapon(GameObject weapon)
    {
        if (!weapon) return;
        
        m_BaseProperties.m_Properties.m_RightWeaponGO = weapon;
        weapon.transform.SetParent(RightHand.transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        
        // 隐藏AI武器
        weapon.GetComponent<SpriteRenderer>().enabled = false;
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
            //Debug.Log("没有设置巡逻路线");
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
        //眩晕状态不能攻击
        if (m_LogicStateManager.IncludeState(ELogicState.StunModifier))
        {
            return false;
        }
        
        if (m_LogicStateManager.IncludeState(ELogicState.AIAttacking))
        {
            return false;
        }
        
        
        var enemy = GetGameObjectsInAttackRange();
        if (enemy.Count <= 0)
        {
            return false;
        }
        
        
        if (m_BaseProperties.m_Properties.m_LeftWeaponGO == null &&  m_BaseProperties.m_Properties.m_RightWeaponGO == null)
        {
            return false;
        }
        
        
        if (TokenPool.ApplyToken(m_TokenWeight) == false)
        {
            return false;
        }

        
        m_LogicStateManager.AddState(ELogicState.AIAttacking);
        
        if (!m_LogicStateManager.IncludeState(ELogicState.AIAttacking))
        {
            return false;
        }
       

        var leftFire = IronAngel.Utils.RandomBool(m_ProbabilityOfLeftWeapon);
        var rightFire = IronAngel.Utils.RandomBool(m_ProbabilityOfRightWeapon);

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
        var leftDuration = m_LeftHandWeaponAttackingDuration;
        var LeftHandAttackPreCastDelay = WeaponSystemCenter.GetWeaponConfig(m_BaseProperties.m_Properties.m_LeftWeaponGO).attackPreCastDelay;
        if (leftDuration <= LeftHandAttackPreCastDelay)
        {
            Debug.LogError("攻击时长包含前摇时长，所以前摇时长不能大于攻击时长");
        }
        m_LogicStateManager.SetStateDuration(ELogicState.AIAttacking, leftDuration);
        StartCoroutine(FireCoroutine(m_BaseProperties.m_Properties.m_LeftWeaponGO));
    }

    [ServerCallback]
    private void RightHandFire()
    {
        var rightDuration = m_RightHandWeaponAttackingDuration;
        var RightHandAttackPreCastDelay = WeaponSystemCenter.GetWeaponConfig( m_BaseProperties.m_Properties.m_RightWeaponGO).attackPreCastDelay;
        if (rightDuration <= RightHandAttackPreCastDelay)
        {
            #if UNITY_EDITOR
            Debug.LogError("攻击时长包含前摇时长，所以前摇时长不能大于攻击时长");
            #endif
        }
        m_LogicStateManager.SetStateDuration(ELogicState.AIAttacking, rightDuration);
        StartCoroutine(FireCoroutine( m_BaseProperties.m_Properties.m_RightWeaponGO));
    }
    
    [ServerCallback]
    private IEnumerator FireCoroutine(GameObject weapon)
    {
        
        m_LogicStateManager.AddState(ELogicState.AIAttackPreCastDelay);
        var AttackPreCastDelay = WeaponSystemCenter.GetWeaponConfig(weapon).attackPreCastDelay;
        m_LogicStateManager.SetStateDuration(ELogicState.AIAttackPreCastDelay, AttackPreCastDelay);
        var weaponInstance = weapon.GetComponent<WeaponInstance>();
        
        var dir = transform.rotation*Vector3.up;
        WeaponSystemCenter.Instance.StartLaserPointer(gameObject,weapon,transform.position, dir);
        
        while (m_LogicStateManager.IncludeState(ELogicState.AIAttackPreCastDelay))
        {
            m_AIMovement.SetMoveEnabled(false);
            yield return null;
        }
        
        m_AIMovement.SetMoveEnabled(true);
        
        while (m_LogicStateManager.IncludeState(ELogicState.AIAttacking))
        {
            var firePoint = weaponInstance.firePoint.position;
            dir = transform.rotation*Vector3.up;
            dir = ComputeAngleOfFire(dir);
            WeaponSystemCenter.Instance.CmdFire(gameObject, weapon,firePoint,dir, false);
            yield return null;
        }
        
        WeaponSystemCenter.Instance.RpcUnFire(weapon);
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
        var rangeOfAimingError = m_RangeOfAimingError;
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
