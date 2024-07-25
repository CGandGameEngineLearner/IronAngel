/// <summary>
/// 状态抽象类，继承使用时必须自己重写int GetHashCode方法，否则不能正确区分各种状态。
/// </summary>
/// <value></value>
public abstract class State
{

    public override int GetHashCode()
    {
        return GetType().MetadataToken;
    }

    public override bool Equals(object obj)
    {
        return GetType().MetadataToken == obj.GetType().MetadataToken;
    }

    public void StateIn(){}

    public void Update(float deltaTime){}

    public void StateOut(){}
}
