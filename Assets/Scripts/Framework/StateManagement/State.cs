

/// <summary>
/// 继承使用时必须自己重写int GetHashCode方法，否则不能正确区分各种状态。
/// </summary>
/// <value></value>
public abstract class State
{
    
    public State()
    {
        
    }
    
    public State(int hashCode)
    {
         m_HashCode = hashCode;
    }

    public State(int hashCode,IStateManager parent)
    {
        m_parent = parent == null ? null : parent;
        m_HashCode = hashCode;
    }

    virtual public void OnStateIn()
    {

    }

    virtual public void Update(float deltaTime)
    {

    }

    virtual public void OnStateOut()
    {
        
    }

    virtual public void SetActive(bool active)
    {
        m_Active = active;
    }
    virtual public bool GetActive()
    {
        return m_Active;
    }

    virtual public void SetParent(IStateManager parent)
    {
        m_parent = parent;
    }
    virtual public IStateManager GetParent()
    {
        return m_parent;
    }

    private bool m_Active = false;

    protected IStateManager m_parent = null;

    private int m_HashCode = 0;

    
    public void SetHashCode(int hashCode)
    {
        m_HashCode = hashCode;
    }

   
    public override int GetHashCode()
    {
        return m_HashCode;
    }



    public override bool Equals(object obj)
    {
        return GetType().MetadataToken == obj.GetType().MetadataToken;
    }
}
