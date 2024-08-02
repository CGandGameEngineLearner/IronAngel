using System.Collections.Generic;
using UnityEngine;

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

    // [SerializeField] [ConditionalHide("m_AtkType", (int)AtkType.ShotGun)]
    private float m_ShotSpreadAngle;

    // [SerializeField] private AudioClip m_SoundEffect;
    // [SerializeField] private ParticleSystem m_EffectPrefab;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/WeaponConfig", order = 1)]
public class WeaponConfig : ItemConfig
{
    // 武器类型
    public AtkType atkType => m_AtkType;
    public AmmunitionType ammunitionType => m_ammunitionType;

    public int weaponHp => m_WeaponHp;

    public float interval => m_Interval;
    public int magSize => m_MagSize;
    public int simShots => m_SimShots;
    public float ShotSpreadAngle => m_ShotSpreadAngle;
    public float spreadAngle => m_SpreadAngle;

    public AudioClip soundEffect => m_SoundEffect;
    public ParticleSystem effectPrefab => m_EffectPrefab;

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
        };
    }

    [Header("武器属性配置")] [SerializeField] private AtkType m_AtkType;
    [SerializeField] private AmmunitionType m_ammunitionType;
    [SerializeField] private int m_WeaponHp;
    [SerializeField] private float m_Interval;
    [SerializeField] private int m_MagSize;
    [SerializeField] private int m_SimShots;
    [SerializeField] private float m_SpreadAngle;

    [SerializeField] [ConditionalHide("m_AtkType", (int)AtkType.ShotGun)]
    private float m_ShotSpreadAngle;

    [SerializeField] private AudioClip m_SoundEffect;
    [SerializeField] private ParticleSystem m_EffectPrefab;
}