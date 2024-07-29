using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using System.Collections;
public class AIMovement : MonoBehaviour
{
    
    private NavMeshAgent agent;
    private LogicStateManager m_LogicStateManager;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        m_LogicStateManager = GetComponent<LogicStateManager>();
    }

    public virtual bool SetDestination(Vector3 target)
    {
        return agent.SetDestination(target);
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

    public IEnumerator MoveToDestinationCoroutine(Vector3 target)
    {
        SetDestination(target);
        yield return new WaitUntil(() => agent.remainingDistance == 0);
    }
       
}