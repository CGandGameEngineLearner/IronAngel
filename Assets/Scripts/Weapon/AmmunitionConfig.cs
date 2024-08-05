// WeaponHandle: HP Sheild Mag Object
// AmmuntionHandle: Start Dir Object

using System.Collections.Generic;
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
    [Tooltip("子弹是否有爆炸衰减，计算方式为平方衰减，普通实弹武器不需要勾选")]public bool m_IsExplodeDamage;
    public List<SpecialAtkType> m_specialAtkTypes;
    [Tooltip("子弹最少存活的物理帧数，用于提供给后处理子弹使用，普通子弹忽视即可")]public int m_LeastLiveFixedFrameCount;
}