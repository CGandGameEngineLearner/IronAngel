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
            base.Init();
        }

        public override void OnStateIn()
        {
            base.OnStateIn();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            EventCenter.Broadcast<GameObject, float, bool>(EventType.Buff_Speed, GetOwner().gameObject, 0, false);
        }

        public override void UnInit()
        {
            base.UnInit();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}