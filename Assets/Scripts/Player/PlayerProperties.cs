using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : BaseProperties
{
    private int m_EnergyThreshold;
    private int m_EnergyLimition;
    //  public-----------------------------------------------
    public void Init(PlayerPropertiesSpec spec)
    {
        m_Energy = spec.m_Energy;
        m_BaseHP = spec.m_BaseHP;
        m_Armor = spec.m_Armor;
        m_EnergyLimition = spec.m_EnergyLimition;
        m_EnergyThreshold = spec.m_EnergyThreshold;

        m_CurrentArmor = m_Armor;
        m_CurrentHP = m_BaseHP;
    }

    public void SetEnergyThreshold(int val)
    {
        m_EnergyThreshold = val;
    }

    public int GetEnergyThreshold()
    {
        return m_EnergyThreshold;
    }

    public void SetEnergyLimition(int val)
    {
        m_EnergyLimition = val >= 0 ? val : 0;
    }

    public int GetEnergyLimition()
    {
        return m_EnergyLimition;
    }

    public override void SetEnergy(int val)
    {
        m_Energy = val >= 0 ? val : 0;
        m_Energy = m_Energy <= m_EnergyLimition ? m_Energy : m_EnergyLimition;
    }

    public override void ChangeEnergy(int val)
    {
        m_Energy = m_Energy + val >= 0 ? m_Energy + val : 0;
        m_Energy = m_Energy <= m_EnergyLimition ? m_Energy : m_EnergyLimition;
    }
}

public struct PlayerPropertiesSpec
{
    public int m_Energy;
    public int m_EnergyThreshold;
    public int m_EnergyLimition;
    public int m_BaseHP;
    public int m_Armor;
}
