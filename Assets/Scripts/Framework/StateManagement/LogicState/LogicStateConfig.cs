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
    public List<LogicStateRelation> StateRelations = new List<LogicStateRelation>();

    private static LogicStateRelation DefaultLogicStateRelation = new LogicStateRelation();
    public LogicStateRelation GetLogicStateRelation(ELogicState stateEnum)
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

    private Dictionary<ELogicState, LogicStateRelation> m_LogicStateEnumDic  = new Dictionary<ELogicState, LogicStateRelation>();
}

[Serializable]
public class LogicStateRelation
{

    public ELogicState stateEnum = ELogicState.Default;

    [SerializeField]
    public List<ELogicState> included = new List<ELogicState>();

    [SerializeField]
    public List<ELogicState> excluded = new List<ELogicState>();
}