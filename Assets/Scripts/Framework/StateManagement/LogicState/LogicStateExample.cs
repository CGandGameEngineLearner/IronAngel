
using UnityEngine;

public class LogicStateExample:LogicState
{
    public LogicStateExample(int hashCode):base(hashCode){}
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