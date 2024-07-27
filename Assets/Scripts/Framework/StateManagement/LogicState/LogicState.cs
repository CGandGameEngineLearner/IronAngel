
using UnityEngine;

public class LogicState
{


    /// <summary>
    /// 默认的状态持续时长是无限
    /// </summary>
    public float Duration 
    {
        get {return m_Duration;}
        set {m_Duration=value;}
    }

    public float StartTime
    {
        get { return m_StartTime; }
        set {m_StartTime=value;}
    }

    public float EndTime 
    {
        get { return StartTime+Duration; }
    }
    public LogicState()
    {
        
    }
    
    public LogicState(ELogicState stateEnum)
    {
        m_HashCode = (int)stateEnum;
    }

    public LogicState(ELogicState stateEnum,LogicStateManager parent)
    {
        m_Owner = parent == null ? null : parent;
        m_HashCode = (int)stateEnum;
    }

    virtual public void OnStateIn()
    {

    }

    virtual public void Update(float deltaTime)
    {

    }


    virtual public void FixedUpdate()
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


    /// <summary>
    /// 设置状态所在的LogicStateManager
    /// </summary>
    /// <param name="owner"></param>
    virtual public void SetOwner(LogicStateManager owner)
    {
        m_Owner = owner;
    }

    /// <summary>
    /// 获取状态所在的LogicStateManager
    /// </summary>
    /// <returns></returns>
    virtual public LogicStateManager GetOwner()
    {
        return m_Owner;
    }

    private bool m_Active = false;

    protected LogicStateManager m_Owner = null;

    private int m_HashCode = 0;

    private float m_StartTime = Time.time;
    
    private float m_Duration = float.PositiveInfinity;

    
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