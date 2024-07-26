using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LogicStateConfig:ScriptableObject
{
    

    /// <summary>
    /// 状态容斥关系配置
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <typeparam name="StateRelation"></typeparam>
    /// <returns></returns>
    [SerializeField]
    public List<LogicStateSetting> StateRelations = new List<LogicStateSetting>();

    private static LogicStateSetting DefaultLogicStateRelation = new LogicStateSetting();
    public LogicStateSetting GetLogicStateRelation(ELogicState stateEnum)
    {
        if (!m_LogicStateEnumDic.ContainsKey(stateEnum))
        {
            return DefaultLogicStateRelation;
        }
        var result = m_LogicStateEnumDic[stateEnum];
        return result;
    }
    
    public LogicState GetLogicStateTemplate(ELogicState stateEnum)
    {
        return LogicStatesSettings.LogicStateTemplates[(int)stateEnum];
    }

    public LogicStateConfig()
    {
        foreach(var stateRelation in StateRelations)
        {
            m_LogicStateEnumDic[stateRelation.stateEnum]=stateRelation;
        }
    }

    private Dictionary<ELogicState, LogicStateSetting> m_LogicStateEnumDic  = new Dictionary<ELogicState, LogicStateSetting>();
}

[Serializable]
public class LogicStateSetting
{
    /// <summary>
    /// 状态类型枚举
    /// </summary>
    public ELogicState stateEnum = ELogicState.Default;

    /// <summary>
    /// 状态持续时长
    /// </summary>
    public float Duration = float.PositiveInfinity;

    /// <summary>
    /// 必须有这些状态，这个状态才能存在
    /// </summary>
    /// <typeparam name="ELogicState"></typeparam>
    /// <returns></returns>
    [SerializeField]
    public List<ELogicState> included = new List<ELogicState>();

    /// <summary>
    /// 必须没有这些状态，这个状态才能存在
    /// </summary>
    /// <typeparam name="ELogicState"></typeparam>
    /// <returns></returns>
    [SerializeField]
    public List<ELogicState> excluded = new List<ELogicState>();
}