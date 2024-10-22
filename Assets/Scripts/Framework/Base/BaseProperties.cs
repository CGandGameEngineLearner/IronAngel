using System;
using Mirror;
using UnityEngine;


/// <summary>
/// 所有的角色都会挂载此基础属性组件
/// </summary>
public class BaseProperties : NetworkBehaviour
{
    public Properties m_Properties;

    [Tooltip("死亡特效种类")]
    public VfxType DiedVFX;

    [Tooltip("死亡音效")]
    public AudioClip DiedAudioClip;
}

/// <summary>
/// 阵营
/// </summary>
public enum ECamp
{
    Player,
    Enemy,
    Team1,
    Team2,
    Team3,
    Team4,
    Team5,
    Team6,
    Team7,
    Team8,
    Team9,
    
    Count,           // 用于记录枚举数量
}

/// <summary>
/// 所有角色都有的属性
/// 这里以后需要重构，都应该作为BaseProperties的成员变量，方便使用Mirror的SyncVar功能
/// </summary>
[Serializable]
public struct Properties
{
    [Tooltip("能量")]
    public int m_Energy;
    
    [Tooltip("基础血量")]
    public int m_BaseHP;
    
    [Tooltip("左手武器血量")]
    public int m_LeftHandWeaponHP;

    [Tooltip("左手武器当前血量")]
    public int m_LeftHandWeaponCurrentHP;
    
    [Tooltip("右手武器血量")]
    public int m_RightHandWeaponHP;

    [Tooltip("右手武器当前血量")]
    public int m_RightHandWeaponCurrentHP;
    
    [Tooltip("左手武器种类")]
    public WeaponType m_LeftHandWeapon;
    
    [Tooltip("右手武器种类")]
    public WeaponType m_RightHandWeapon;

    [Tooltip("左手武器GameObject")]
    public GameObject m_LeftWeaponGO;
    
    [Tooltip("右手武器GameObject")]
    public GameObject m_RightWeaponGO;
    
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
    
    [Tooltip("阵营")]
    public ECamp m_Camp;

    [Tooltip("武器被破坏后掉落武器")]
    public bool m_DropWeapon_WeaponDestroy;

    [Tooltip("角色死亡后掉落武器")]
    public bool m_DropWeapon_CharacterDied;
    
    [Tooltip("核心的SpriteRenderer，受伤时会让它变红")]
    public SpriteRenderer m_CoreSprite;
}