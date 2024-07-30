// WeaponHandle: HP Sheild Mag Object
// AmmuntionHandle: Start Dir Object

using UnityEngine;

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