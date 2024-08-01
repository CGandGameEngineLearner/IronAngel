
using BehaviorDesigner.Runtime.Tasks;
using LogicState;
namespace AI.BehaviourTree.Actions
{
    public class Action_Chase : Action
    {
        private AIController m_AIController;
        private LogicStateManager m_LogicStateManager;

        public override void OnAwake()
        {
            m_AIController = GetComponent<AIController>();
            m_LogicStateManager = GetComponent<LogicStateManager>();
        }

        public override TaskStatus OnUpdate()
        {
            m_AIController.Chase();
            return TaskStatus.Running;
        }
    }
}