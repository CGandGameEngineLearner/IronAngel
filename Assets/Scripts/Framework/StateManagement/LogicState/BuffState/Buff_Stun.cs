using LogicState;
using UnityEngine;

namespace LogicState
{
    /// <summary>
    /// 眩晕buff
    /// </summary>
    public class Buff_Stun : Buff
    {
        public Buff_Stun(ELogicState stateEnum) : base(stateEnum) {}
        public Buff_Stun(ELogicState stateEnum, LogicStateManager parent) : base(stateEnum, parent){}

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