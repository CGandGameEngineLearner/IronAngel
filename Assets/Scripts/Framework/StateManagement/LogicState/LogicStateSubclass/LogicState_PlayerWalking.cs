using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_PlayerWalking : LogicState
{
    public LogicState_PlayerWalking(ELogicState stateEnum) : base(stateEnum) { }
    public override void OnStateIn()
    {
#if UNITY_EDITOR
        Debug.Log("enter" + GetType());
#endif
    }

    public override void Update(float deltaTime)
    {

    }

    public override void FixedUpdate()
    {
        EventCenter.Broadcast<bool>(EventType.StateToGlobal_PlayerWalkState, true);
    }

    public override void OnStateOut()
    {
#if UNITY_EDITOR
        Debug.Log("exit" + GetType());
#endif
    }
}
