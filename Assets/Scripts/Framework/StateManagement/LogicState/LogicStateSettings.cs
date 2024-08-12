using System;
using System.Collections.Generic;

namespace LogicState
{
    /// <summary>
    /// 逻辑状态类型枚举
    /// 新增状态先在这里添加
    /// 然后再在LogicStateTemplates的List中添加LogicState的模板对象
    /// 状态会以模板对象的类型创建
    /// </summary>
    public enum ELogicState
    {
        Default,
        Example,                            // 状态示例
        PlayerWalking,                      // 玩家步行中
        PlayerDashing,                      // 玩家冲刺中
        PlayerShooting,                     // 玩家开枪中
        AIPatroling,                        // AI巡逻中
        AIPerceivedTarget,                  // AI察觉到目标
        EnemyInRangeOfAttack,               // 敌人进入了攻击范围
        AIAttacking,                        // AI攻击中
        AIDashing,                          // AI冲刺中
        AIAttackPreCastDelay,               // AI攻击前摇状态
        CoreDamaged,                        // 核心受到了伤害 (用于受伤特效)
        LeftHandDamaged,                    // 左手受到了伤害 (用于受伤特效)
        RightHandDamaged,                   // 右手受到了伤害 (用于受伤特效)
        // ---------------------------------- 以上为LogicState ---------------------------------- 
        LogicStateSplitter,        
        // ---------------------------------- 以下为BuffState  ---------------------------------- 
        SpeedModifier,                      // 修改移动速度
        StunModifier,                       // 设置角色是否眩晕
        // ---------------------------------- 以上为BuffState  ----------------------------------
        BuffStateSplitter,
        // ---------------------------------- 以下为State计数  ----------------------------------
        Count //此枚举的值为ELogicState的枚举种类的计数 所有的新增枚举不要加在它后面
    }

    public static class LogicStatesSettings
    {
        private static Dictionary<ELogicState,LogicState> m_LogicStateTemplateDic;
        /// <summary>
        /// 新增状态模板对象实例
        /// </summary> <summary>
        /// 
        /// </summary>
        /// <typeparam name="LogicState"></typeparam>
        /// <returns></returns>
        private static List<LogicState> LogicStateTemplates = new List<LogicState>()
        {
            new LogicState(ELogicState.Default),
            new LogicState_Example(ELogicState.Example),
            new LogicState_PlayerWalking(ELogicState.PlayerWalking),
            new LogicState_PlayerDashing(ELogicState.PlayerDashing),
            new LogicState(ELogicState.AIPatroling),
            new LogicState(ELogicState.AIPerceivedTarget),
            new LogicState_PlayerShooting(ELogicState.PlayerShooting),
            new LogicState(ELogicState.EnemyInRangeOfAttack),
            new LogicState_AIAttacking(ELogicState.AIAttacking),
            new LogicState(ELogicState.AIDashing),
            new LogicState(ELogicState.AIAttackPreCastDelay),
            new LogicState_Damaged(ELogicState.CoreDamaged),
            new LogicState_Damaged(ELogicState.LeftHandDamaged),
            new LogicState_Damaged(ELogicState.RightHandDamaged),
            // 以上是LogicState ---------------------
            
            // 以下是BuffState ----------------------
            new Buff_Speed(ELogicState.SpeedModifier),
            new Buff_Stun(ELogicState.StunModifier),
        };

        public static LogicState GetStateTemplate(ELogicState eLogicState)
        {
            if (m_LogicStateTemplateDic == null)
            {
                m_LogicStateTemplateDic = new Dictionary<ELogicState, LogicState>();
                foreach (var state in LogicStateTemplates)
                {
                    m_LogicStateTemplateDic[state.LogicStateEnum] = state;
                }
            }

            if (!m_LogicStateTemplateDic.ContainsKey(eLogicState))
            {
                throw new Exception("未在LogicStateTemplates创建state模板对象");
            }

            return m_LogicStateTemplateDic[eLogicState];
        }
    }
}
