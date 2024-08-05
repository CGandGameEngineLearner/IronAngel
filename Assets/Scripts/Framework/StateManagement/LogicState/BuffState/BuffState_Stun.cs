using LogicState;

namespace LogicState
{
    public class BuffState_Stun : LogicState
    {
        public BuffState_Stun(ELogicState stateEnum) : base(stateEnum)
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
        }

        public override void UnInit()
        {
            base.UnInit();
        }
    }
}