using UnityEngine;

namespace LogicState
{
    /// <summary>
    /// 受伤状态 （用于播放受伤特效）
    /// </summary>
    public class LogicState_Damaged:LogicState
    {
        public LogicState_Damaged(ELogicState stateEnum):base(stateEnum){}
        public LogicState_Damaged(ELogicState stateEnum, LogicStateManager parent) : base(stateEnum, parent){}

        private GameObject m_LeftWeapon, m_RightWeapon;
        
        private SpriteRenderer m_SpriteRenderer;

        private Color m_OriginColor;

        private BaseProperties m_BaseProperties;
        public override void Init()
        {
            m_BaseProperties = gameObject.GetComponent<BaseProperties>();
            if (m_BaseProperties==null)
            {
                return;
            }

            m_LeftWeapon = m_BaseProperties.m_Properties.m_LeftWeaponGO;
            m_RightWeapon = m_BaseProperties.m_Properties.m_RightWeaponGO;
        }
        
        public override void OnStateIn()
        {
            Debug.Log(GetType()+"OnStateIn()");
            
            if (GetOwner().IncludeState(LogicStateEnum))
            {
                return;
            }

            
            if (LogicStateEnum == ELogicState.CoreDamaged)
            {
                var baseProperties = gameObject.GetComponent<BaseProperties>();
                if (baseProperties == null)
                {
                    return;
                }

                m_SpriteRenderer = baseProperties.m_Properties.m_CoreSprite;
            }
            else if (LogicStateEnum == ELogicState.LeftHandDamaged)
            {
                m_SpriteRenderer = m_LeftWeapon?.GetComponent<SpriteRenderer>();
            }
            else if (LogicStateEnum == ELogicState.RightHandDamaged)
            {
                m_SpriteRenderer = m_RightWeapon?.GetComponent<SpriteRenderer>();
            }
            else
            {
                return;
            }

            if (m_SpriteRenderer == null)
            {
                return;
            }

            m_OriginColor = m_SpriteRenderer.color;
        }
        
        public override void FixedUpdate()
        {
            if (m_OriginColor == null||m_SpriteRenderer==null)
            {
                return;
            }
            if (m_SpriteRenderer.color != m_OriginColor)
            {
                m_SpriteRenderer.color = m_OriginColor;
            }
            else
            {
                m_SpriteRenderer.color = Color.red;
            }
        }
        
        public override void OnStateOut()
        {
            if (m_SpriteRenderer == null)
            {
                return;
            }
            m_SpriteRenderer.color = m_OriginColor;
            Debug.Log(GetType()+"OnStateOut()");
        }
    }
}