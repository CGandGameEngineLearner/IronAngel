using UnityEngine;
using UnityEngine.Serialization;

public enum AtkType
{
    ShotGun = 0,
    Laser = 1,
    MissileLauncher = 2,
    Default,
}

public enum AmmunitionType
{
    // PostProcess
    PostProcess_Expolde = 0,
    Bullet,
    None,
}

public enum WeaponType
{
    Glock,
}

public class ItemConfig : ScriptableObject
{
    public GameObject prefab => m_Prefab;
    public int minPoolSize => m_MinPoolSize;
    public int maxPoolSize => m_MaxPoolSize;
    
    [Header("对象池配置")] 
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private int m_MinPoolSize;
    [SerializeField] private int m_MaxPoolSize;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/WeaponConfig", order = 1)]
public class WeaponConfig : ItemConfig
{
    // 武器类型
    public AtkType atkType => m_AtkType;
    public AmmunitionType ammunitionType => m_ammunitionType;
    public float interval => m_Interval;
    public float magSize => m_MagSize;
    public int simShots => m_SimShots;
    public AudioClip soundEffect => m_SoundEffect;
    public ParticleSystem effectPrefab => m_EffectPrefab;

    [Header("武器属性配置")]
    [SerializeField] private AtkType m_AtkType;
    [SerializeField] private AmmunitionType m_ammunitionType;
    [SerializeField] private float m_Interval;
    [SerializeField] private float m_MagSize;
    [SerializeField] private int m_SimShots;
    [SerializeField] private AudioClip m_SoundEffect;
    [SerializeField] private ParticleSystem m_EffectPrefab;
    
}

// WeaponHandle: HP Sheild Mag Object
// AmmuntionHandle: Start Dir Object

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/AmmunitionConfig", order = 1)]
public class AmmunitionConfig : ItemConfig
{
    public float speed => m_Speed;
    public float lifeDistance => m_LifeDistance;
    public AmmunitionType postAmmunitionType => m_PostAmmunitionType;

    [Header("子弹属性配置")]
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_LifeDistance;
    [SerializeField] private AmmunitionType m_PostAmmunitionType;

}