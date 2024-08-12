using UnityEngine;

namespace LogicState
{
    public class Buff_Speed : Buff
    {
        public Buff_Speed(ELogicState stateEnum) : base(stateEnum) {}
        public Buff_Speed(ELogicState stateEnum, LogicStateManager parent) : base(stateEnum, parent){}

        public override void Init()
        {
            //Debug.Log(GetType()+"Init()");
        }

        public override void OnStateIn()
        {
            //Debug.Log(GetType()+"OnStateIn()");
        }

        public override void Update(float deltaTime)
        {
           
        }

        public override void FixedUpdate()
        {
           
        }

        public override void OnStateOut()
        {
            //Debug.Log(GetType()+"OnStateOut()");
            EventCenter.Broadcast<GameObject, float, bool>(EventType.Buff_Speed, GetOwner().gameObject, 0, false);
        }

        public override void UnInit()
        {
            
        }
    }
}