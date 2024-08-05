
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

    private void Start()
    {
        m_BaseProperties = GetComponent<BaseProperties>();
        agent = GetComponent<NavMeshAgent>();
        m_LogicStateManager = GetComponent<LogicStateManager>();
        m_LastPos = transform.position;
    }

    private void Update()
    {
        m_MoveDirection = (transform.position - m_LastPos).normalized;   
        m_LastPos = transform.position;
        if (m_MoveDirection.magnitude > 0)
        {
            float angle = Vector2.Angle(new Vector2(0, 1), m_MoveDirection);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(m_MoveDirection.y,m_MoveDirection.x) * Mathf.Rad2Deg );
        }
    }

    public virtual bool SetDestination(Vector3 target)
    {
        return agent.SetDestination(target);
    }
    
    /// <summary>
    /// AI会追到距离玩家一定位置的地方与玩家拉开距离开火，
    /// m_BaseProperties.m_Properties.m_EngagementPosRatio：
    /// 交战距离与攻击范围之比
    /// </summary>
    /// <param name="targetGameObject"></param>
    public virtual void Chase(GameObject targetGameObject)
    {
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