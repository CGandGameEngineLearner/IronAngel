using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Action_Chase : Action
{
    private AIController m_AIController;
    public override void OnAwake()
    {
        m_AIController = GetComponent<AIController>();
    }
    public override void OnStart()
    {
        //m_AIController.Chase();
    }
}
