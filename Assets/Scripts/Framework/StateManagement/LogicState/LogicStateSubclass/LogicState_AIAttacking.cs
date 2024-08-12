using AI.TokenPool;
using UnityEngine;
namespace LogicState
{
    public class LogicState_AIAttacking:LogicState
    {
        public LogicState_AIAttacking(ELogicState stateEnum):base(stateEnum){}
        public LogicState_AIAttacking(ELogicState stateEnum, LogicStateManager parent) : base(stateEnum, parent){}

        private BaseProperties m_BaseProperties;
        private AIController m_AIController;
        
        public override void FirstCreated()
        {
            m_BaseProperties = gameObject.GetComponent<BaseProperties>();
            m_AIController = gameObject.GetComponent<AIController>();
        }
        
        public override void Init()
        {
            //Debug.Log(GetType()+"Init()");
        }
        public override void OnStateIn()
        {
            if (TokenPool.ApplyToken(m_AIController.m_TokenWeight) == false)
            {
                GetOwner().RemoveState(ELogicState.AIAttacking);
                return;
            }
            Debug.Log(GetType()+"OnStateIn()");
        }
    
        public override void Update(float deltaTime)
        {
            //Debug.Log(GetType()+"Update(float deltaTime)"+deltaTime);
        }
    
        public override void FixedUpdate()
        {
            //Debug.Log(GetType()+"FixedUpdate()");
        }
    
        public override void OnStateOut()
        {
            Debug.Log(GetType()+"OnStateOut()");
            TokenPool.ReturnToken();
        }
    
        public override void UnInit()
        {
            //Debug.Log(GetType()+"UnInit()");
        }
        
    }
}