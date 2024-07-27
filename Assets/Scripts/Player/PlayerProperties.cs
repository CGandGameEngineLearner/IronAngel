using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties
{
    private float m_Energy;
    private float m_EnergyThreshold;
    private float m_BaseHP;
    private float m_Armor;

//  public--------------------------------------
    public void Init(PlayerPropertiesSpec spec)
    {
        m_Energy = spec.m_Energy;
        m_EnergyThreshold = spec.m_EnergyThreshold;
    }

    public void SetEnergy(float val)
    {
        m_Energy = val;
    }

    public float GetEnergy()
    {
        return m_Energy;
    }

    public void ChangeEnergy(float val)
    {
        m_Energy = m_Energy + val >= 0 ? m_Energy + val : 0;
    }

    public void SetEnergyThreshold(float val)
    {
        m_EnergyThreshold = val;
    }

    public float GetEnergyThreshold()
    {
        return m_EnergyThreshold;
    }
}

public struct PlayerPropertiesSpec
{
    public float m_Energy;
    public float m_EnergyThreshold;
}
