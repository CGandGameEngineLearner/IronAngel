using System.Collections.Generic;
public enum ELogicState
{
    Default,
    Example,
    PlayerWalking,
    PlayerDashing,
    AIPatroling,
    AIVisionPerceived,
    
    
    Count //此枚举的值为ELogicState的枚举种类的计数 所有的新增枚举不要加在它后面
}

public static class LogicStatesSettings
{
    /// <summary>
    /// 新增状态模板对象实例
    /// </summary> <summary>
    /// 
    /// </summary>
    /// <typeparam name="LogicState"></typeparam>
    /// <returns></returns>
    public static List<LogicState> LogicStateTemplates = new List<LogicState>()
    {
        new LogicState(ELogicState.Default),
        new LogicState_Example(ELogicState.Example),
        new LogicState_PlayerWalking(ELogicState.PlayerWalking),
        new LogicState_PlayerDashing(ELogicState.PlayerDashing),
        new LogicState(ELogicState.AIPatroling),
        new LogicState(ELogicState.AIVisionPerceived),
    };
}