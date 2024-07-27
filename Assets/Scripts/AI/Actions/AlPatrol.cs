using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AIPatrol : Action
{
    private AIController m_AIController;
    void OnEnable()
    {
        m_AIController = GetComponent<AIController>();
    }
    void OnStart()
    {
        
    }
}