namespace LogicState
{
    /// <summary>
    /// buff的基类
    /// </summary>
    public class Buff:LogicState
    {
        public Buff(ELogicState stateEnum):base(stateEnum){}
        public Buff(ELogicState stateEnum, LogicStateManager parent) : base(stateEnum, parent){}
    }
}