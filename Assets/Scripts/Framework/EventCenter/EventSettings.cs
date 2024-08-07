//事件列表
public enum EventType
{
    None,
    StateToGlobal_PlayerDashState,
    StateToGlobal_PlayerWalkState,

    LogicState_Example_StateIn,                             // LogicState发送状态进入事件通知外部模块的样例
    LogicState_AIAttacking_StateOut,                        // AI攻击状态结束
    CharacterDied,                                          // 角色死亡

    // buff
    // 减速效果
    Buff_Speed,
    // 眩晕效果
    Buff_Stun,


    // UI 
    PauseMenu,
}

public delegate void Callback();
public delegate void Callback<T1>(T1 arg1);
public delegate void Callback<T1,T2>(T1 arg1,T2 arg2);
public delegate void Callback<T1,T2,T3>(T1 arg1,T2 arg2,T3 arg3);
public delegate void Callback<T1,T2,T3,T4>(T1 arg1,T2 arg2,T3 arg3,T4 arg4);