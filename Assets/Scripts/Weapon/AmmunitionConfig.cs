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
    public float m_Speed;
    public float m_LifeDistance;
    public int m_Damage;
    public AmmunitionType m_PostAmmunitionType;

}