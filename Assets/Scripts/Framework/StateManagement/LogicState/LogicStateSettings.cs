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
        AIVisionPerceived,                  // AI视觉察觉到目标
        EnemyInRangeOfAttack,               // 敌人进入了攻击范围
    
    
        Count //此枚举的值为ELogicState的枚举种类的计数 所有的新增枚举不要加在它后面
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
            new LogicState_PlayerWalking(ELogicState.PlayerWalking),
            new LogicState_PlayerDashing(ELogicState.PlayerDashing),
            new LogicState(ELogicState.AIPatroling),
            new LogicState(ELogicState.AIVisionPerceived),
            new LogicState_PlayerShooting(ELogicState.PlayerShooting),
            new LogicState(ELogicState.EnemyInRangeOfAttack),
        };
    }
}
