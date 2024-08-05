
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LogicState
{
    public class LogicStateManager : MonoBehaviour
    {
        /// <summary>
        /// 状态字典的哈希桶初始容量
        /// </summary>
        public int StateDictionaryCapacity = (int)ELogicState.Count;
        private Dictionary<ELogicState, LogicState> m_LogicStateDic;
        private Dictionary<ELogicState, LogicState> m_FutureStatesBuffer;
        private Dictionary<ELogicState, bool> m_StateInit;
        
        
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

        public bool IncludeState(List<ELogicState> stateEnums)
        {
            foreach (var stateEnum in stateEnums)
            {
                if (!IncludeState(stateEnum))
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// 检查逻辑状态是否满足逻辑状态管理器中有included中的状态，且没有exclude中的状态的情况
        /// </summary>
        /// <param name="included"></param>
        /// <param name="excluded"></param>
        /// <returns></returns>
        public bool CheckState(List<ELogicState> included, List<ELogicState> excluded)
        {
            if (included != null)
            {
                foreach (var stateEnum in included)
                {
                    if(!IncludeState(stateEnum))
                    {
                        return false;
                    }
                }
            }

            if (excluded != null)
            {
                foreach (var stateEnum in excluded)
                {
                    if(IncludeState(stateEnum))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// 设置状态的持续时间
        /// </summary>
        /// <param name="eLogicState"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public bool SetStateDuration(ELogicState eLogicState, float duration)
        {
            if (!m_LogicStateDic.ContainsKey(eLogicState))
            {
                return false;
            }

            m_LogicStateDic[eLogicState].Duration = duration;
            return true;
        }
        
        /// <summary>
        /// 获取状态的持续时间
        /// 如果没有这个状态则会返回0.0f
        /// </summary>
        /// <param name="eLogicState"></param>
        /// <returns>float</returns>
        public float GetStateDuration(ELogicState eLogicState)
        {
            if (!IncludeState(eLogicState))
            {
                return 0.0f;
            }

            return m_LogicStateDic[eLogicState].Duration;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            m_StateInit = new Dictionary<ELogicState, bool>();
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
                    
                    // ScriptAbleObject的问题，运行时得从这里动态更新，否则会是默认的无限长的持续时间
                    if (!m_StateInit.ContainsKey(stateEnum) || m_StateInit[stateEnum] == false)
                    {
                        state.Duration = stateSetting.Duration;
                        m_StateInit[stateEnum] = true;
                    }
                     
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
}


