using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyUI : MonoBehaviour
{
    public EnemyUIType m_Type;
    private SpriteRenderer m_SpriteRender;
    private BaseProperties m_Properties;
    [SerializeField]
    private Vector3 m_Offset;
    [SerializeField]
    private float m_SpriteAlphaFlash = 1.0f;
    [SerializeField]
    private float m_FadeSpeed = 1.0f;

    private float m_CurrentAlpha = 0;
    private void Start()
    {
        m_Properties = transform.parent.GetComponent<BaseProperties>();
        m_SpriteRender = GetComponent<SpriteRenderer>();
        if (m_Type == EnemyUIType.Blood)
            m_SpriteRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(m_Type == EnemyUIType.ArmorPic)
        {
            if (m_Properties.m_Properties.m_CurrentArmor <= 0)
            {
                m_SpriteRender.color = new Color(1, 1, 1, 0);
            }
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.position = transform.parent.transform.position + m_Offset;
        }
        if(m_Type == EnemyUIType.ArmorMaterial)
        {
            var c = m_SpriteRender.color;
            c.a = m_CurrentAlpha;
            m_SpriteRender.color = c;
            if(m_CurrentAlpha > 0)
            {
                m_CurrentAlpha -= Time.deltaTime * m_FadeSpeed;
            }
        }
        if(m_Type == EnemyUIType.Blood)
        {
            m_SpriteRender.gameObject.transform.localScale = new Vector3(m_Properties.m_Properties.m_CurrentHP * 1.0f / m_Properties.m_Properties.m_BaseHP * 2, 1, 1);
            transform.rotation = Quaternion.identity;
            transform.position = transform.parent.transform.position + m_Offset;
        }
    }

    public void ArmorFlash()
    {
        m_CurrentAlpha = m_SpriteAlphaFlash;
    }
}

[Serializable]
public enum EnemyUIType
{
    Canvas,
    ArmorPic,  
    ArmorMaterial,
    Blood,
}
