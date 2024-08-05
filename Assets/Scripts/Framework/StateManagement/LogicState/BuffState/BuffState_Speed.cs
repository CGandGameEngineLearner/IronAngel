using UnityEngine;

namespace LogicState
{
    public class BuffState_Speed : LogicState
    {
        public BuffState_Speed(ELogicState stateEnum) : base(stateEnum)
        {
        }

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