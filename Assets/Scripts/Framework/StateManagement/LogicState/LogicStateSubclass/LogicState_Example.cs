
using UnityEngine;

public class LogicState_Example:LogicState
{
    public LogicState_Example(ELogicState stateEnum):base(stateEnum){}
    public override void OnStateIn()
    {
        Debug.Log(GetType()+" OnStateIn");
    }

    public override void Update(float deltaTime)
    {
        Debug.Log(GetType()+" Update:"+deltaTime);
    }

    public override void OnStateOut()
    {
        Debug.Log(GetType()+" OnStateOut");
    }
}