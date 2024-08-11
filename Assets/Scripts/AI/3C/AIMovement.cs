
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using System.Collections;
using LogicState;
using UnityEditor;
using Vector2 = UnityEngine.Vector2;

public class AIMovement : MonoBehaviour
{
    
    protected NavMeshAgent agent;
    private LogicStateManager m_LogicStateManager;
    private Vector3 m_LastPos;
    private Vector3 m_MoveDirection = Vector3.zero;
    private BaseProperties m_BaseProperties;
    private Rigidbody2D m_Rigidbody;

    private float m_NormalSpeed;
    private GameObject m_ChaseTarget;
    private Vector2 m_dashDir;

    private bool m_MoveEnabled = true;
    
    [Tooltip("墙体图层")]
    public LayerMask m_WallLayer;
    
    [Tooltip("冲刺速度"),Range(0,float.PositiveInfinity)]
    public float m_DashSpeed;
    
    [Tooltip("冲刺时长"),Range(0,float.PositiveInfinity)]
    public float m_DashDuration;
    
    [Tooltip("AI与玩家交战的距离")]
    public float m_EngagementDistance;

    private void OnEnable()
    {
        m_LogicStateManager = GetComponent<LogicStateManager>();
        m_BaseProperties = GetComponent<BaseProperties>();
        agent = GetComponent<NavMeshAgent>();
        
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_LastPos = transform.position;
        m_NormalSpeed = agent.speed;
    }

    private void FixedUpdate()
    {
        if (!m_MoveEnabled||m_LogicStateManager.IncludeState(ELogicState.StunModifier))
        {
            return;
        }

        if (m_LogicStateManager.IncludeState(ELogicState.AIDashing))
        {
            agent.enabled = false; // 冲刺时要关闭AI导航
            m_dashDir = m_dashDir.normalized;
            var dashSpeed = m_DashSpeed;
            var v2 = m_Rigidbody.position;
            v2.x += m_dashDir.x * dashSpeed * Time.fixedDeltaTime;
            v2.y += m_dashDir.y * dashSpeed * Time.fixedDeltaTime;
            var hit = Physics2D.Raycast(m_Rigidbody.position, m_dashDir, Vector2.Distance(m_Rigidbody.position, v2) + 3,
                m_WallLayer);
            
            // 如果会穿墙
            if (hit && Vector2.Distance(m_Rigidbody.position, v2) >= Vector2.Distance(m_Rigidbody.position, hit.point))
            {
                v2 = hit.point - m_dashDir * 0.1f;
            }

            m_Rigidbody.MovePosition(v2);
        }
        else
        {
            agent.enabled = true;
        }
    }
    
    public void SetMoveEnabled(bool enabled)
    {
        agent.enabled = enabled;
        m_MoveEnabled = enabled;
    }
    

    /// <summary>
    /// 设置行走速度
    /// </summary>
    /// <param name="speed"></param> 这个参数是detla值
    public void SetSpeed(float speed)
    {
        agent.speed = agent.speed + speed > 0 ? agent.speed + speed : 0;
    }

    public void ResetSpeed()
    {
        agent.speed = m_NormalSpeed;
    }

    private void Update()
    {
        m_MoveDirection = (transform.position - m_LastPos).normalized;   
        m_LastPos = transform.position;
    
        //眩晕状态不能旋转
        if (m_LogicStateManager.IncludeState(ELogicState.StunModifier))
        {
            SetMoveEnabled(false);
            return;
        }
        
        SetMoveEnabled(true);
        
        

        if (m_LogicStateManager.IncludeState(ELogicState.AIPerceivedTarget)&&m_ChaseTarget!=null)
        {
           
            var targetDir = (m_ChaseTarget.transform.position - transform.position).normalized;

            Quaternion targetRotation;
            targetRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90.0f));
            
            var rotateTowards = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
            
            transform.rotation = rotateTowards;
        }
        else
        {   
            if (m_MoveDirection.magnitude > 0)
            {
                float angle = Vector2.Angle(new Vector2(0, 1), m_MoveDirection);
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(m_MoveDirection.y,m_MoveDirection.x) * Mathf.Rad2Deg -90 );
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public bool SetDestination(Vector3 target)
    {
        if (!agent.isOnNavMesh)
        {
            return false;
        }
        
        // 如果目标点有刚体占着位置 则把目标点设置到1个单位附近
        var dir = (target - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(target, dir, out hit, 1f))
        {
            if (hit.collider != null)
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    target = target - 1 * dir;
                }
            }
        }
        return agent.SetDestination(target);
    }
    
    /// <summary>
    /// 冲刺
    /// </summary>
    /// <param name="_dashDir"></param>
    public void Dash(Vector2 _dashDir)
    {
        
        if (!m_LogicStateManager.IncludeState(ELogicState.AIDashing))
        {
            var success = m_LogicStateManager.AddState(ELogicState.AIDashing);
            m_LogicStateManager.SetStateDuration(ELogicState.AIDashing, m_DashDuration);
            m_dashDir = _dashDir;
        }
        
    }
    
   

    /// <summary>
    /// AI会追到距离玩家一定位置的地方与玩家拉开距离开火，
    /// m_BaseProperties.m_Properties.m_EngagementPosRatio：
    /// 交战距离与攻击范围之比
    /// </summary>
    /// <param name="targetGameObject"></param>
    public virtual void Chase(GameObject targetGameObject)
    {
        m_ChaseTarget = targetGameObject;
        var offsetVec = (transform.position - targetGameObject.transform.position).normalized;
        offsetVec += m_EngagementDistance * offsetVec;
        var targetPos = targetGameObject.transform.position + offsetVec;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(targetPos);
        }
       
    }   

    protected IEnumerator ChaseCoroutine(GameObject target)
    {
        StartCoroutine(MoveToDestinationCoroutine(target.transform.position));
        yield break;
    }   

    public void PatrolWithFixedRoute(SplineContainer patrolRoute)
    {
        if (patrolRoute == null)
        {
            return;
        }

        StartCoroutine(PatrolWithFixedRouteCoroutine(patrolRoute));
    }

    protected IEnumerator PatrolWithFixedRouteCoroutine(SplineContainer patrolRoute)
    {
        if (!patrolRoute ||patrolRoute.Spline.Count <= 0)
        {
            yield break;
        }
        
        int currentSplineIndex = 0;// 当前前往的样条线节点下标

        m_LogicStateManager.AddState(ELogicState.AIPatroling);
        
        while(m_LogicStateManager.IncludeState(ELogicState.AIPatroling))
        {
            currentSplineIndex = currentSplineIndex % (patrolRoute.Spline.Count+1);
            float t = currentSplineIndex*1.0f/patrolRoute.Spline.Count;
            var target = patrolRoute.EvaluatePosition(t);
            currentSplineIndex += 1;
            yield return StartCoroutine(MoveToDestinationCoroutine(target));
        }
    }

    protected virtual IEnumerator MoveToDestinationCoroutine(Vector3 target)
    {
        if (!agent.isOnNavMesh)
        {
            yield return null;
        }


        SetDestination(target);
        yield return new WaitUntil(() =>
            {
                if (!agent.isOnNavMesh)
                {
                    return true;
                }
                return agent.remainingDistance == 0;
            }
        );

    }
}