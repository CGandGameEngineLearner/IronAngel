using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BaseProperties : NetworkBehaviour
{
    public Properties m_Properties;

    public bool ChangeCurentHP(int val)
    {
        m_Properties.m_CurrentHP = m_Properties.m_CurrentHP + val < m_Properties.m_BaseHP ? m_Properties.m_CurrentHP + val : m_Properties.m_BaseHP;
        if(m_Properties.m_CurrentHP <= 0)
        {
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}


[Serializable]
public struct Properties
{
    [Tooltip("能量")]
    public int m_Energy;
    
    [Tooltip("基础血量")]
    public int m_BaseHP;
    
    [Tooltip("左手武器血量")]
    public int m_LeftHandWeaponHP;
    
    [Tooltip("右手武器血量")]
    public int m_RightHandWeaponHP;
    
    [Tooltip("护甲")]
    public int m_Armor;
    
    [Tooltip("能量盾的次数")]
    public int m_EnergyShieldCount;
    
    [Tooltip("当前血量")]
    public int m_CurrentHP;
    
    [Tooltip("当前护甲")]
    public int m_CurrentArmor;

    [Tooltip("攻击范围")]
    public float m_AttackRange;

    [Tooltip("AI抢攻击Token的优先权重"),Range(0,1)]
    public float m_TokenWeight;

    [Tooltip("AI的射击角度误差范围"),Range(0,90)]
    public float m_RangeOfAimingError;
}