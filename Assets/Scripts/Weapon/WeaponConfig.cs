using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct WeaponConfigData
{
    public AtkType atkType;
    public AmmunitionType ammunitionType;
    public float interval;
    public int magSize;
    public int simShots;
    public float spreadAngle;
    public float shotSpreadAngle;
    public int WeaponHP;
    public bool anticipation;
    public float attackPreCastDelay;

    public float m_ShotSpreadAngle;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/WeaponConfig", order = 1)]
[Serializable]
public class WeaponConfig : ItemConfig
{
    public string weaponName;
    // 武器类型
    public AtkType atkType => m_AtkType;

    public AmmunitionType ammunitionType => m_ammunitionType;

    public int weaponHp => m_WeaponHp;

    public float interval => m_Interval;
    public int magSize => m_MagSize;
    public int simShots => m_SimShots;
    public float ShotSpreadAngle => m_ShotSpreadAngle;
    public float spreadAngle => m_SpreadAngle;

    public float LaserPointerWidth => laserPointerWidth;
    
    public float attackPreCastDelay => m_AttackPreCastDelay; // 攻击前摇时长

    public VfxType fireVfxType => m_FireVfxType;

    public WeaponConfigData ToData()
    {
        return new WeaponConfigData
        {
            atkType = this.m_AtkType,
            ammunitionType = this.m_ammunitionType,
            WeaponHP = this.m_WeaponHp,
            interval = this.m_Interval,
            magSize = this.m_MagSize,
            simShots = this.m_SimShots,
            spreadAngle = this.m_SpreadAngle,
            shotSpreadAngle = this.m_ShotSpreadAngle,
            attackPreCastDelay = this.m_AttackPreCastDelay,
        };
    }

    [Header("武器基础属性配置")] [SerializeField] private AtkType m_AtkType;

    [SerializeField] [EnumRange((int)AmmunitionType.Start + 1, (int)AmmunitionType.PostExplodeSplitter - 1)]
    private AmmunitionType m_ammunitionType;

    [SerializeField] private int m_WeaponHp;
    [SerializeField] private float m_Interval;
    [SerializeField] private int m_MagSize;
    [SerializeField] private int m_SimShots;
    [SerializeField] private float m_SpreadAngle;

    [Header("特殊属性配置")] 
    [SerializeField] private float m_ShotSpreadAngle;
    [SerializeField] private float laserPointerWidth;

    [FormerlySerializedAs("m_AnticipationDuration")] [Tooltip("攻击前摇时长/镭射提示多久消失/敌人会蹲多久")] [SerializeField]
    private float m_AttackPreCastDelay;

    [Header("音效和特效配置")] [SerializeField] private VfxType m_FireVfxType;
    [SerializeField] private SfxType m_FireSfxType;
}