
using System;
using System.Collections.Generic;
using UnityEngine;


public class LogicStateManager : MonoBehaviour
{
    private Dictionary<ELogicState,LogicState> m_LogicStateDic = new Dictionary<ELogicState, LogicState>();
    private Queue<ELogicState> m_FutureStates = new Queue<ELogicState>();
    private Queue<ELogicState> m_AbandenStates = new Queue<ELogicState>();

    
    public LogicStateConfig LogicStateConfig;
    // Start is called before the first frame update

    public bool AddState(ELogicState stateEnum)
    {
        LogicStateSetting stateSetting = LogicStateConfig.GetLogicStateSetting(stateEnum);
        if(CheckState(stateSetting.included,stateSetting.excluded))
        {
            m_FutureStates.Enqueue(stateEnum);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveState(ELogicState stateEnum)
    {
        m_AbandenStates.Enqueue(stateEnum);
    }

    public bool IncludeState(ELogicState stateEnum)
    {
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
                // if(!CheckState(stateSetting.included,stateSetting.excluded))
                // {
                //     RemoveStateImmediately(stateEnum);
                // }
                if(Time.time >= state.EndTime)
                {
                    RemoveStateImmediately(stateEnum);
                    //Debug.Log("remove state" + stateEnum);
                }
            }
            
        }
        foreach(var state in m_LogicStateDic.Values)
        {
            if(state.GetActive())
            {
                state.Update(Time.deltaTime);
            }
        }
    }

    void LateUpdate()
    {
        while(m_AbandenStates.Count>0)
        {
            var stateEnum = m_AbandenStates.Dequeue();
            RemoveStateImmediately(stateEnum);
        }

        while(m_FutureStates.Count>0)
        {
            var stateEnum = m_FutureStates.Dequeue();
            AddStateImmediately(stateEnum);
        }

        
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


    private void AddStateImmediately(ELogicState stateEnum)
    {
        if(m_LogicStateDic.ContainsKey(stateEnum))
        {
            var state = m_LogicStateDic[stateEnum];
            state.StartTime = Time.time;
            state.Init();
            state.SetActive(true);
            state.OnStateIn();
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
            newState.SetActive(true);
            newState.OnStateIn();

            m_LogicStateDic[stateEnum]=newState;
            
            
            
        }
    }
    
    private void RemoveStateImmediately(ELogicState stateEnum)
    {
        if(m_LogicStateDic.ContainsKey(stateEnum))
        {
            var state = m_LogicStateDic[stateEnum];

            state.SetActive(false);
            state.UnInit();
            state.OnStateOut();
        }
    }
    
}
