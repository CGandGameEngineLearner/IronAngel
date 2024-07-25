
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class LogicStateManager : MonoBehaviour, IStateManager
{
    private Dictionary<int,LogicState> m_LogicStateDic;
    private Queue<int> m_FutureStates;
    private Queue<int> m_AbandenStates;
    // Start is called before the first frame update

    public void AddState(int stateCode)
    {
        LogicStateRelation stateRelation = LogicStateConfig.GetLogicStateRelation(stateCode);
        if(CheckState(stateRelation.included,stateRelation.excluded))
        {
            m_FutureStates.Enqueue(stateCode);
        }
    }

    public void RemoveState(int stateCode)
    {
        m_AbandenStates.Enqueue(stateCode);
    }

    public bool IncludeState(int stateCode)
    {
        return m_LogicStateDic.ContainsKey(stateCode);
    }

    public bool CheckState(List<int> included, List<int> excluded)
    {
        foreach (int stateCode in included)
        {
            if(!IncludeState(stateCode))
            {
                return false;
            }
        }
        foreach (int stateCode in excluded)
        {
            if(IncludeState(stateCode))
            {
                return false;
            }
        }

        return true;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(int stateCode in m_LogicStateDic.Keys)
        {
            LogicStateRelation stateRelation = LogicStateConfig.GetLogicStateRelation(stateCode);
            if(!CheckState(stateRelation.included,stateRelation.excluded))
            {
                m_LogicStateDic[stateCode].SetActive(false);
                m_LogicStateDic[stateCode].OnStateOut();
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
            var stateCode = m_AbandenStates.Dequeue();
            RemoveStateImmediately(stateCode);
        }

        while(m_FutureStates.Count>0)
        {
            var stateCode = m_FutureStates.Dequeue();
            AddStateImmediately(stateCode);
        }

        
    }


    private void AddStateImmediately(int stateCode)
    {
        if(m_LogicStateDic.ContainsKey(stateCode))
        {
            m_LogicStateDic[stateCode].SetActive(true);
            m_LogicStateDic[stateCode].OnStateIn();
        }
        else
        {
            var stateTemplate = LogicStateConfig.LogicStateDictionary[stateCode];
            Type stateType = stateTemplate.GetType();
            
            LogicState newState = (LogicState)Activator.CreateInstance(stateType);
            newState.SetHashCode(stateCode);
            newState.SetParent(this);
            newState.SetActive(true);
            newState.OnStateIn();

            m_LogicStateDic[stateCode]=newState;
            
            
            
        }
    }
    
    private void RemoveStateImmediately(int stateCode)
    {
        if(m_LogicStateDic.ContainsKey(stateCode))
        {
            m_LogicStateDic[stateCode].SetActive(false);
            m_LogicStateDic[stateCode].OnStateOut();
        }
    }
    
}
