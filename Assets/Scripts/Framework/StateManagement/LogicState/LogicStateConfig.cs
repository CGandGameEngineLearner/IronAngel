using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class LogicStateConfig:ScriptableObject
{
    

    /// <summary>
    /// 状态容斥关系配置
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <typeparam name="StateRelation"></typeparam>
    /// <returns></returns>
    [SerializeField]
    public List<LogicStateSetting> LogicStateSettings = new List<LogicStateSetting>();

    private static LogicStateSetting DefaultLogicStateSetting = new LogicStateSetting();
    public LogicStateSetting GetLogicStateSetting(ELogicState stateEnum)
    {
        if (!m_LogicStateEnumDic.ContainsKey(stateEnum))
        {
            #if UNITY_EDITOR
            //Debug.Log(stateEnum.ToString()+"使用了默认设置" );
            #endif
            return DefaultLogicStateSetting;
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
        Init();
    }

    void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        foreach(var stateSetting in LogicStateSettings)
        {
            m_LogicStateEnumDic[stateSetting.stateEnum]=stateSetting;
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
    /// 不满足容斥条件时，是否自动退出状态
    /// </summary>
    public bool AutoStateOut = false;

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