
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
        m_parent = parent == null ? null : parent;
        m_HashCode = (int)stateEnum;
    }

    /// <summary>
    /// 状态进入时，初始化成员变量用。在此函数内初始化成员变量,OnStateIn()前调用
    /// </summary> <summary>
    /// 
    /// </summary>
    virtual public void Init()
    {

    }

    /// <summary>
    /// 状态进入时，在Init()后调用
    /// </summary>
    virtual public void OnStateIn()
    {

    }

    /// <summary>
    /// 状态激活后，每帧调用
    /// </summary>
    /// <param name="deltaTime"></param>
    virtual public void Update(float deltaTime)
    {

    }

    /// <summary>
    /// 物理帧调用
    /// </summary>
    virtual public void FixedUpdate()
    {

    }

    /// <summary>
    /// 状态退出时调用，然后会调用UnInit()
    /// </summary>
    virtual public void OnStateOut()
    {
        
    }

    /// <summary>
    /// 在此函数内析构调成员变量，使其恢复初始化值，在OnStateOut()后调用
    /// </summary> <summary>
    /// 
    /// </summary>
    virtual public void UnInit()
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