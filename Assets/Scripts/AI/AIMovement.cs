using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using System.Collections;
using UnityEngine.Animations;
public class AIMovement
{
    private GameObject m_GameObject;
    private NavMeshAgent agent; 
    public AIMovement(GameObject gameObject)
    {
        m_GameObject = gameObject;
        agent = m_GameObject.GetComponent<NavMeshAgent>();
    }

    public virtual bool SetDestination(Vector3 target)
    {
        return agent.SetDestination(target);
    }

    public virtual void Patrol(SplineContainer PatrolRoute)
    {

    }

    // public virtual IEnumerator RunPatrolWithFixedRoute(SplineContainer PatrolRoute)
    // {
    //     Vector3 firstPoint = PatrolRoute.GetPoint(0);
    
    // }
       
}