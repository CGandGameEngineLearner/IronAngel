using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BaseProperties : NetworkBehaviour
{
    public Properties m_Properties = new Properties();
    public Properties Properties { set; get; }
}


public struct Properties
{
    public int m_Energy;
    public int m_BaseHP;
    public int m_Armor;
    public int m_EnergyShieldCount;

    public int m_CurrentHP;
    public int m_CurrentArmor;
}