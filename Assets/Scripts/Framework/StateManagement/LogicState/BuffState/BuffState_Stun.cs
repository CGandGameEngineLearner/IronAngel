using LogicState;
using UnityEngine;

namespace LogicState
{
    public class BuffState_Stun : LogicState
    {
        public BuffState_Stun(ELogicState stateEnum) : base(stateEnum)
        {
        }

        public override void Init()
        {
            
            
        }

        public override void OnStateIn()
        {
            
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void FixedUpdate()
        {
            
        }

        public override void OnStateOut()
        {
            EventCenter.Broadcast<GameObject, bool>(EventType.Buff_Stun, GetOwner().gameObject, false);
        }

        public override void UnInit()
        {
            
        }
    }
}