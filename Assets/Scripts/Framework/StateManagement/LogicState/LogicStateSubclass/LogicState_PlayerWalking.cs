using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_PlayerWalking : LogicState
{
    public LogicState_PlayerWalking(ELogicState stateEnum) : base(stateEnum) { }
    public override void OnStateIn()
    {
        Debug.Log("enter" + GetType());
        
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
        Debug.Log("exit" + GetType());
    }
}
