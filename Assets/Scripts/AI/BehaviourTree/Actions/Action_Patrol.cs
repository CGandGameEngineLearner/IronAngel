using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using LogicState;
namespace AI.BehaviourTree.Actions
{
    public class Action_Patrol : Action
    {
        private AIController m_AIController;
        private LogicStateManager m_LogicStateManager;

        public override void OnAwake()
        {
            Debug.Log(GetType() + "OnEnable()");
            m_AIController = GetComponent<AIController>();
            m_LogicStateManager = GetComponent<LogicStateManager>();
        }

        public override void OnStart()
        {
            Debug.Log(GetType()+"OnStart()");
            m_AIController.Patrol();
        }

        public override TaskStatus OnUpdate()
        {
            if (m_LogicStateManager.IncludeState(ELogicState.AIVisionPerceived))
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
    }
}