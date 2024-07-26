using System.Collections.Generic;
public enum ELogicState
{
    Default = 0,
    Example = 1,
    PlayerMoving = 2,
    PlayerDashing = 3,
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
        new LogicState(ELogicState.PlayerMoving),
        new LogicState_PlayerDashing(ELogicState.PlayerDashing),
    };
}