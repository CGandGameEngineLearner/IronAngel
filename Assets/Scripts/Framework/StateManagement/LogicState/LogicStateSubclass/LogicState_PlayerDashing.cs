using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_PlayerDashing : LogicState
{
    public LogicState_PlayerDashing(ELogicState stateEnum) : base(stateEnum) { }
    public override void OnStateIn()
    {
        GetParent().RemoveState(ELogicState.PlayerMoving);
        EventCenter.Broadcast<bool>(EventType.StateToGlobal_PlayerDashState, true);
    }

    public override void Update(float deltaTime)
    {
        EventCenter.Broadcast<bool>(EventType.StateToGlobal_PlayerDashState, false);
    }

    public override void OnStateOut()
    {

    }
}
