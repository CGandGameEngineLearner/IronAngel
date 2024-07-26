
public class LogicState
{
    public LogicState()
    {
        
    }
    
    public LogicState(int hashCode)
    {
         m_HashCode = hashCode;
    }

    public LogicState(int hashCode,LogicStateManager parent)
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

    virtual public void SetParent(LogicStateManager parent)
    {
        m_parent = parent;
    }
    virtual public LogicStateManager GetParent()
    {
        return m_parent;
    }

    private bool m_Active = false;

    protected LogicStateManager m_parent = null;

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