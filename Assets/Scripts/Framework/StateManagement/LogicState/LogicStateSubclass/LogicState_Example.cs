
using UnityEngine;

namespace LogicState
{
    public class LogicState_Example:LogicState
    {
        public LogicState_Example(ELogicState stateEnum):base(stateEnum){}
        public LogicState_Example(ELogicState stateEnum, LogicStateManager parent) : base(stateEnum, parent){}
        public override void Init()
        {
            Debug.Log(GetType()+"Init()");
            //调用LogicStateManager的外部模块，遵循谁调用谁监听原则，
            //前两个参数必须为所属的LogicStateManager和状态类型枚举，
            //监听对LogicStateManager进行操作后得到的事件。
            EventCenter.AddListener<LogicStateManager,ELogicState,string>(
                EventType.LogicState_Example_StateIn,
                OnEventLogicStateExampleStateIn
            );
        }
        public override void OnStateIn()
        {
            Debug.Log(GetType()+"OnStateIn()");
            EventCenter.Broadcast<LogicStateManager,ELogicState,string>(
                EventType.LogicState_Example_StateIn,
                GetOwner(),
                ELogicState.Example,
                "LogicState_Example.OnStateIn()"
            );
        }
    
        public override void Update(float deltaTime)
        {
            Debug.Log(GetType()+"Update(float deltaTime)"+deltaTime);
        }
    
        public override void FixedUpdate()
        {
            Debug.Log(GetType()+"FixedUpdate()");
        }
    
        public override void OnStateOut()
        {
            Debug.Log(GetType()+"OnStateOut()");
        }
    
        public override void UnInit()
        {
            Debug.Log(GetType()+"UnInit()");
        }
    
        /// <summary>
        /// 假设这是外部模块监听事件绑定的函数
        /// </summary>
        /// <param name="logicStateManager"></param>
        /// <param name="eLogicState"></param>
        /// <param name="info"></param>
        private void OnEventLogicStateExampleStateIn(LogicStateManager logicStateManager,ELogicState eLogicState, string info)
        {
            Debug.Log("ListendEvent,EventType.LogicState_Example_StateIn "+logicStateManager+eLogicState+info);
        }
    }
}
