using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicState_PlayerDashing : LogicState
{
    public LogicState_PlayerDashing(int hashCode) : base(hashCode) { }
    public override void OnStateIn()
    {
        Debug.Log(GetType() + " OnStateIn");
    }

    public override void Update(float deltaTime)
    {
        Debug.Log(" Update:" + deltaTime);
    }

    public override void OnStateOut()
    {
        Debug.Log(GetType() + " OnStateOut");
    }
}
