using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 新增状态先在这里加枚举,再添加到LogicStateArray
/// </summary> <summary>
/// 
/// </summary>
public enum ELogicState
{
    None = 0,
    Example = 1,
    SimpleExample = 2,
}


public static class LogicStateConfig
{
    /// <summary>
    /// 新增状态记得添加到这个数组，构造参数和枚举值一致
    /// </summary>
    /// <value></value>
    static private LogicState[] LogicStateArray = new LogicState[]
    {
        null,
        new LogicStateExample((int)ELogicState.Example), // 新增状态在以类似的方式枚举
    };

    /// <summary>
    /// 状态容斥关系配置
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <typeparam name="StateRelation"></typeparam>
    /// <returns></returns>
    private static Dictionary<int, LogicStateRelation> StateRelationDic = new Dictionary<int, LogicStateRelation>()
    {
        { (int)ELogicState.Example, new LogicStateRelation { included = new List<int>(), excluded = new List<int>() } }
    };


    static public LogicStateRelation GetLogicStateRelation(int stateCode)
    {
        if (!StateRelationDic.ContainsKey(stateCode))
        {
            Debug.LogError("未查询到stateCode为" + stateCode + "的逻辑状态的容斥关系，请查看StateRelationDic是否配置");
            return null;
        }

        return StateRelationDic[stateCode];
    }


    static LogicStateConfig()
    {
        foreach (var state in LogicStateArray)
        {
            m_LogicStateDictionary[state.GetHashCode()] = state;
        }
    }

    static private Dictionary<int, LogicState> m_LogicStateDictionary;

    static public Dictionary<int, LogicState> LogicStateDictionary
    {
        get { return m_LogicStateDictionary; }
        private set { m_LogicStateDictionary = value; }
    }
}


public class LogicStateRelation
{
    public List<int> included { get; set; }
    public List<int> excluded { get; set; }
}