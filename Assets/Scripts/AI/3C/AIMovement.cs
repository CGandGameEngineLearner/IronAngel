
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using System.Collections;
using LogicState;

public class AIMovement : MonoBehaviour
{
    
    private NavMeshAgent agent;
    private LogicStateManager m_LogicStateManager;
    private Vector3 m_LastPos;
    private Vector3 m_MoveDirection = Vector3.zero;
    private BaseProperties m_BaseProperties;
    private Rigidbody2D m_Rigidbody;

    private float m_NormalSpeed;
    private GameObject m_ChaseTarget;

    private void Start()
    {
        m_BaseProperties = GetComponent<BaseProperties>();
        agent = GetComponent<NavMeshAgent>();
        m_LogicStateManager = GetComponent<LogicStateManager>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_LastPos = transform.position;
        m_NormalSpeed = agent.speed;
    }

    /// <summary>
    /// 
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

    public virtual bool SetDestination(Vector3 target)
    {
        return agent.SetDestination(target);
    }
    
    /// <summary>
    /// 冲刺
    /// </summary>
    /// <param name="_dashDir"></param>
    public virtual void Dash(Vector3 _dashDir)
    {
        
        if (!m_LogicStateManager.IncludeState(ELogicState.AIDashing))
        {
            var success = m_LogicStateManager.AddState(ELogicState.AIDashing);
            m_LogicStateManager.SetStateDuration(ELogicState.AIDashing, m_BaseProperties.m_Properties.m_DashDuration);
            
            StartCoroutine(DashCoroutine(_dashDir));
            
        }
        
    }
    
    /// <summary>
    /// 冲刺协程
    /// </summary>
    /// <param name="_dashDir"></param>
    /// <returns></returns>
    protected IEnumerator DashCoroutine(Vector2 _dashDir)
    {
        
        var m_WallLayer = m_BaseProperties.m_Properties.m_WallLayer;
        agent.enabled = false;
        
        //当冲刺状态的持续时间结束时停止
        while (m_LogicStateManager.IncludeState(ELogicState.AIDashing))
        {
            _dashDir = _dashDir.normalized;
            var dashSpeed = m_BaseProperties.m_Properties.m_DashSpeed;
            var v2 = m_Rigidbody.position;
            v2.x += _dashDir.x * dashSpeed * Time.fixedDeltaTime;
            v2.y += _dashDir.y * dashSpeed * Time.fixedDeltaTime;
            var hit = Physics2D.Raycast(m_Rigidbody.position, _dashDir, Vector2.Distance(m_Rigidbody.position, v2) + 1,
                m_WallLayer);
            if (hit && Vector2.Distance(m_Rigidbody.position, v2) >= Vector2.Distance(m_Rigidbody.position, hit.point))
            {
                v2 = hit.point - _dashDir * 0.1f;
            }

            m_Rigidbody.MovePosition(v2);
            yield return null;
        }

        agent.enabled = true;
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
        offsetVec += m_BaseProperties.m_Properties.m_EngagementDistance * offsetVec;
        var targetPos = targetGameObject.transform.position + offsetVec;
        agent.SetDestination(targetPos);
    }   

    protected IEnumerator ChaseCoroutine(GameObject target)
    {
        StartCoroutine(MoveToDestinationCoroutine(target.transform.position));
        yield break;
    }   

    public virtual void PatrolWithFixedRoute(SplineContainer patrolRoute)
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

    protected IEnumerator MoveToDestinationCoroutine(Vector3 target)
    { 
        SetDestination(target);
        yield return new WaitUntil(() => agent.remainingDistance == 0);
    }
       
}