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
    private void Start()
    {
        m_Properties = transform.parent.GetComponent<BaseProperties>();
        m_SpriteRender = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(m_Properties.m_Properties.m_CurrentArmor <= 0)
        {
            m_SpriteRender.color = new Color(1, 1, 1, 0);
        }
    }
}

[Serializable]
public enum EnemyUIType
{
    ArmorPic,
}
