
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LogicStateManager : MonoBehaviour
{
    /// <summary>
    /// 状态字典的哈希桶初始容量
    /// </summary>
    public int StateDictionaryCapacity = (int)ELogicState.Count;
    private Dictionary<ELogicState, LogicState> m_LogicStateDic;
    private Dictionary<ELogicState, LogicState> m_FutureStatesBuffer;
    
    
    public LogicStateConfig LogicStateConfig;
    

    public bool AddState(ELogicState stateEnum)
    {
        LogicStateSetting stateSetting = LogicStateConfig.GetLogicStateSetting(stateEnum);
        if(CheckState(stateSetting.included,stateSetting.excluded))
        {
            
            if(m_LogicStateDic.ContainsKey(stateEnum))
            {
                var state = m_LogicStateDic[stateEnum];
                state.StartTime = Time.time;
                state.Init();
                state.OnStateIn();
                m_FutureStatesBuffer[stateEnum] = state;
            }
            else
            {
                LogicState stateTemplate = LogicStateConfig.GetLogicStateTemplate(stateEnum);
                Type stateType = stateTemplate.GetType();
                LogicState newState = (LogicState)(Activator.CreateInstance(stateType,stateEnum));
                newState.SetOwner(this);
                newState.Duration = stateTemplate.Duration;
                newState.StartTime = Time.time;
                newState.Init();
                newState.OnStateIn();
                m_FutureStatesBuffer[stateEnum] = newState;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveState(ELogicState stateEnum)
    {
        if (m_FutureStatesBuffer.ContainsKey(stateEnum))
        {
            var state = m_FutureStatesBuffer[stateEnum];
            state.SetActive(false);
            state.OnStateOut();
            state.UnInit();
            m_FutureStatesBuffer.Remove(stateEnum);
        }
        else if(m_LogicStateDic.ContainsKey(stateEnum))
        {
            var state = m_LogicStateDic[stateEnum];
            state.SetActive(false);
            state.OnStateOut();
            state.UnInit();
        }
        else
        {
            return false;
        }

        return true;
    }

    public bool IncludeState(ELogicState stateEnum)
    {
        if (m_FutureStatesBuffer.ContainsKey(stateEnum))
        {
            return true;
        }
        return m_LogicStateDic.ContainsKey(stateEnum)&&m_LogicStateDic[stateEnum].GetActive();
    }

    public bool CheckState(List<ELogicState> included, List<ELogicState> excluded)
    {
        foreach (var stateEnum in included)
        {
            if(!IncludeState(stateEnum))
            {
                return false;
            }
        }
        foreach (var stateEnum in excluded)
        {
            if(IncludeState(stateEnum))
            {
                return false;
            }
        }

        return true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        m_LogicStateDic = new Dictionary<ELogicState, LogicState>(StateDictionaryCapacity);
        m_FutureStatesBuffer = new Dictionary<ELogicState, LogicState>(StateDictionaryCapacity);
    }

    // Update is called once per frame
    void Update()
    {
        // 不满足容斥配置或达到结束时间的状态会被立即移除
        foreach(ELogicState stateEnum in m_LogicStateDic.Keys)
        {
            var state = m_LogicStateDic[stateEnum];
            if(state.GetActive())
            {
                LogicStateSetting stateSetting = LogicStateConfig.GetLogicStateSetting(stateEnum);
                state.Duration = stateSetting.Duration;
                if(stateSetting.AutoStateOut&&!CheckState(stateSetting.included,stateSetting.excluded))
                {
                    RemoveState(stateEnum);
                }
                if(Time.time >= state.EndTime)
                {
                    RemoveState(stateEnum);
                    //Debug.Log("remove state" + stateEnum);
                }
            }
            
        }
        foreach(var stateEnum in m_LogicStateDic.Keys)
        {
            var state = m_LogicStateDic[stateEnum];
            if(state.GetActive())
            {
                state.Update(Time.deltaTime);
            }
        }
    }

    void LateUpdate()
    {
        foreach (var futureStatePair in m_FutureStatesBuffer)
        {
            futureStatePair.Value.SetActive(true);
            m_LogicStateDic[futureStatePair.Key] = futureStatePair.Value;
        }
        m_FutureStatesBuffer.Clear();
    }

    void FixedUpdate()
    {
        foreach(var state in m_LogicStateDic.Values)
        {
            if(state.GetActive())
            {
                state.FixedUpdate();
            }
        }
    }
}
