
using UnityEngine;

namespace LogicState
{
    public class LogicState
    {
        private GameObject m_GameObject;
        public GameObject gameObject
        {
            get {return m_GameObject;}
            private set {m_GameObject=value;}
        }

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

        private ELogicState m_LogicStateEnum;

        public ELogicState LogicStateEnum
        {
            get { return m_LogicStateEnum;}
            private set { m_LogicStateEnum = value; }
        }
        public LogicState()
        {
            
        }
        
        public LogicState(ELogicState stateEnum)
        {
            LogicStateEnum = stateEnum;
            m_HashCode = (int)stateEnum;
        }
        

        public LogicState(ELogicState stateEnum,LogicStateManager parent)
        {
            gameObject = parent.gameObject;
            m_Owner = parent == null ? null : parent;
            LogicStateEnum = stateEnum;
            m_HashCode = (int)stateEnum;
        }

        /// <summary>
        /// 第一次创建此状态时调用
        /// 为了防止高频创建和析构对象导致的性能问题 逻辑状态第一次创建实例后都会一直留在内存中
        /// 只是通过对应的方法控制其状态的进入和退出
        /// </summary>
        virtual public void FirstCreated()
        {
            
        }
        
        /// <summary>
        /// 每个状态进入时先调用Init初始化成员变量，再调用OnStateIn,
        /// 所以请在这里初始化成员变量到初始状态
        /// </summary>
        virtual public void Init()
        {

        }

        /// <summary>
        /// Init完后调用OnStateIn,此时已被设为激活状态
        /// </summary>
        virtual public void OnStateIn()
        {

        }

        /// <summary>
        /// 状态被激活后，每帧调用
        /// </summary>
        /// <param name="deltaTime"></param>

        virtual public void Update(float deltaTime)
        {

        }


        virtual public void FixedUpdate()
        {

        }

        /// <summary>
        /// 先调StateOut，再会掉UnInit，析构成员变量。
        /// </summary>
        virtual public void OnStateOut()
        {
            
        }
        
        /// <summary>
        /// 析构成员变量
        /// </summary>
        virtual public void UnInit()
        {
            
        }

        public void SetActive(bool active)
        {
            m_Active = active;
        }
        public bool GetActive()
        {
            return m_Active;
        }


        /// <summary>
        /// 设置状态所在的LogicStateManager
        /// </summary>
        /// <param name="owner"></param>
        public void SetOwner(LogicStateManager owner)
        {
            m_Owner = owner;
        }

        /// <summary>
        /// 获取状态所在的LogicStateManager
        /// </summary>
        /// <returns></returns>
        public LogicStateManager GetOwner()
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
}
