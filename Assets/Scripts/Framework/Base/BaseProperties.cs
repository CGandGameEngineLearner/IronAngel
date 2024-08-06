using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Range = DG.DemiLib.Range;

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

/// <summary>
/// 阵营
/// </summary>
public enum ECamp
{
    Player,
    Enemy,
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
    
    [Tooltip("左手武器种类")]
    public WeaponType m_LeftHandWeapon;
    
    [Tooltip("右手武器种类")]
    public WeaponType m_RightHandWeapon;
    
    [Tooltip("左手武器每次攻击的持续时长")]
    public float m_LeftHandWeaponAttackingDuration;
    
    [Tooltip("右手武器每次攻击的持续时长")]
    public float m_RightHandWeaponAttackingDuration;

    [Tooltip("左手武器使用概率"),Range(0,1)]
    public float m_ProbabilityOfLeftWeapon;

    [Tooltip("右手武器使用概率"),Range(0,1)]
    public float m_ProbabilityOfRightWeapon;

    [Tooltip("AI与玩家交战的距离")]
    public float m_EngagementDistance;
    
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

    [Tooltip("阵营")]
    public ECamp m_Camp;

    [Tooltip("武器被破坏后掉落武器")]
    public bool m_DropWeapon_WeaponDestroy;

    [Tooltip("角色死亡后掉落武器")]
    public bool m_DropWeapon_CharacterDied;

    [Tooltip("AI是否可以在受到伤害时自动躲闪以下")]
    public bool m_AutoAvoid;

    [Tooltip("AI躲避的概率"),Range(0,1)]
    public float m_AutoAvoid_Probability;

    [Tooltip("允许AI躲避的剩余次数")]
    public float m_AutoAvoid_RestTimes;

    [Tooltip("冲刺速度"),Range(0,float.PositiveInfinity)]
    public float m_DashSpeed;
    
    [Tooltip("冲刺时长"),Range(0,float.PositiveInfinity)]
    public float m_DashDuration;
    
    [Tooltip("墙体图层")]
    public LayerMask m_WallLayer;

}