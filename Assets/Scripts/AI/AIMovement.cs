using UnityEngine;
using UnityEngine.AI;

public class AIMovement
{
    private GameObject m_GameObject;
    private NavMeshAgent agent; 
    public AIMovement(GameObject gameObject)
    {
        m_GameObject = gameObject;
        agent = m_GameObject.GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }
       
}