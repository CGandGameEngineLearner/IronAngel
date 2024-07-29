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

    public virtual void Patrol(SplineContainer PatrolRoute)
    {
        if (PatrolRoute == null)
        {
            return;
        }

        StartCoroutine(PatrolWithFixedRouteCoroutine(PatrolRoute));
    }

    protected IEnumerator PatrolWithFixedRouteCoroutine(SplineContainer PatrolRoute)
    {
        if (PatrolRoute.Spline.Count <= 0)
        {
            yield break;
        }
        
        Vector3 firstPoint = PatrolRoute.Spline[0].Position;
        yield return StartCoroutine(MoveToDestinationCoroutine(firstPoint));//等待走到目的位置
        m_LogicStateManager.AddState(ELogicState.AIPatroling);
        while (m_LogicStateManager.IncludeState(ELogicState.AIPatroling))
        {
            
            //yield return 
        }
    }

    public IEnumerator MoveToDestinationCoroutine(Vector3 target)
    {
        SetDestination(target);
        yield return null;
        yield return new WaitUntil(() => agent.remainingDistance == 0);
    }
       
}