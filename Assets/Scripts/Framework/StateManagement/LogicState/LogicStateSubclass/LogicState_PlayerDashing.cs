using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_PlayerDashing : LogicState
{
    public LogicState_PlayerDashing(ELogicState stateEnum) : base(stateEnum) { }
    public override void OnStateIn()
    {
#if UNITY_EDITOR
        //Debug.Log("enter" + GetType());
#endif
        EventCenter.Broadcast<bool>(EventType.StateToGlobal_PlayerDashState, true);
    }

    public override void Update(float deltaTime)
    {
        
    }

    public override void FixedUpdate()
    {
        EventCenter.Broadcast<bool>(EventType.StateToGlobal_PlayerDashState, false);
    }

    public override void OnStateOut()
    {
#if UNITY_EDITOR
        //Debug.Log("exit" + GetType());
#endif
    }
}
