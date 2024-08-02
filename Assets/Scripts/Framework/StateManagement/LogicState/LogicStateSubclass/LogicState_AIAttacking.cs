using AI.TokenPool;
using UnityEngine;
namespace LogicState
{
    public class LogicState_AIAttacking:LogicState
    {
        public LogicState_AIAttacking(ELogicState stateEnum):base(stateEnum){}
    
        public override void Init()
        {
            Debug.Log(GetType()+"Init()");
            
        }
        public override void OnStateIn()
        {
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
            
            // 通知攻击状态结束
            EventCenter.Broadcast<LogicStateManager,ELogicState>(
                EventType.LogicState_AIAttacking_StateOut,
                GetOwner(),
                ELogicState.AIAttacking
            );
        }
    
        public override void UnInit()
        {
            Debug.Log(GetType()+"UnInit()");
        }
        
    }
}