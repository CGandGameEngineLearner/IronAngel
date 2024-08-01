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
    public int m_Energy;
    public int m_BaseHP;
    public int m_LeftHandWeaponHP;
    public int m_RightHandWeaponHP;
    public int m_Armor;
    public int m_EnergyShieldCount;

    public int m_CurrentHP;
    public int m_CurrentArmor;
}