using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Action_Patrol : Action
{
    private AIController m_AIController;
    public override void OnAwake()
    {
        Debug.Log(GetType()+"OnEnable()");
        m_AIController = GetComponent<AIController>();
    }
    public override void OnStart()
    {
        Debug.Log(GetType()+"OnStart()");
        m_AIController.Patrol();
    }
}