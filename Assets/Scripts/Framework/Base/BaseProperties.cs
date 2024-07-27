using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProperties
{
    protected int m_Energy;
    protected int m_BaseHP;
    protected int m_Armor;
    

    protected int m_CurrentHP;
    protected int m_CurrentArmor;
    //  public--------------------------------------
    // 继承需要自定义一个Init方法

    public virtual void SetEnergy(int val)
    {
        m_Energy = val >= 0 ? val : 0;
    }

    public int GetEnergy()
    {
        return m_Energy;
    }

    public virtual void ChangeEnergy(int val)
    {
        m_Energy = m_Energy + val >= 0 ? m_Energy + val : 0;
    }

    public void SetArmor(int val)
    {
        m_Armor = val >= 0 ? val : 0;
    }

    public int GetArmor()
    {
        return m_Armor;
    }

    public void SetBaseHP(int val)
    {
        m_BaseHP = val >= 0 ? val : 0;
    }

    public int GetBaseHP()
    {
        return m_BaseHP;
    }

    public int GetCurrentHP()
    {
        return m_CurrentHP;
    }

    public void SetCurrentHP(int val)
    {
        m_CurrentHP = val >= 0 ? val : 0;
        m_CurrentHP = m_CurrentHP <= m_BaseHP ? m_CurrentHP :  m_BaseHP;
    }

    public void ChangeCurrentHP(int val)
    {
        m_CurrentHP = m_CurrentHP + val >= 0 ? m_CurrentHP + val : 0;
        m_CurrentHP = m_CurrentHP <= m_BaseHP ? m_CurrentHP : m_BaseHP;
    }

    public void SetCurrentArmor(int val)
    {
        m_CurrentArmor = val >= 0 ? val : 0;
        m_CurrentArmor = m_CurrentArmor <= m_Armor ? m_CurrentArmor : m_Armor;
    }

    public int GetCurrentArmor()
    {
        return m_CurrentArmor;
    }

    public void ChangeCurrentArmor(int val)
    {
        m_CurrentArmor = m_CurrentArmor + val >= 0 ? m_CurrentArmor + val : 0;
        m_CurrentArmor = m_CurrentArmor <= m_Armor ? m_CurrentArmor : m_Armor;
    }
}
