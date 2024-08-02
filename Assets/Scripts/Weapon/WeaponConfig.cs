using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/WeaponConfig", order = 1)]
public class WeaponConfig : ItemConfig
{
    // 武器类型
    public AtkType atkType => m_AtkType;
    public AmmunitionType ammunitionType => m_ammunitionType;
    public List<SpecialAtkType> SpecialAtkTypes => m_SpecialAtkTypes;
    
    public float interval => m_Interval;
    public float magSize => m_MagSize;
    public int simShots => m_SimShots;
    public AudioClip soundEffect => m_SoundEffect;
    public ParticleSystem effectPrefab => m_EffectPrefab;

    [Header("武器属性配置")] 
    [SerializeField] private AtkType m_AtkType;
    [SerializeField] private AmmunitionType m_ammunitionType;
    [SerializeField] private List<SpecialAtkType> m_SpecialAtkTypes;
    [SerializeField] private float m_Interval;
    [SerializeField] private float m_MagSize;
    [SerializeField] private int m_SimShots;
    [SerializeField] private AudioClip m_SoundEffect;
    [SerializeField] private ParticleSystem m_EffectPrefab;
    
}

