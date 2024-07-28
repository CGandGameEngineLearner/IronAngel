using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Unity.Mathematics;
using UnityEngine;

public class WeaponSystemCenter
{
    private ObjectPoolManager<WeaponType> m_WeaponPool = new();
    private ObjectPoolManager<AmmunitionType> m_AmmunitionPool = new();
    private WeaponFactory m_WeaponFactory = new();
    private AmmunitionFactory m_AmmunitionFactory = new();

    /// <summary>
    /// 更新子弹
    /// </summary>
    public void Update()
    {
        m_AmmunitionFactory.Update();
    }

    public void Init(List<KeyValuePair<WeaponType, WeaponConfig>> weaponConfigList,
        List<KeyValuePair<AmmunitionType, AmmunitionConfig>> ammunitionConfigList)
    {
        foreach (var weaponConfig in weaponConfigList)
        {
            m_WeaponPool.AddPool(weaponConfig.Key,
                new ObjectCategory()
                {
                    prefab = weaponConfig.Value.prefab, defaultSize = weaponConfig.Value.minPoolSize,
                    maxSize = weaponConfig.Value.minPoolSize
                });
        }
        
        foreach (var ammunitionConfig in ammunitionConfigList)
        {
            m_AmmunitionPool.AddPool(ammunitionConfig.Key,
                new ObjectCategory()
                {
                    prefab = ammunitionConfig.Value.prefab, defaultSize = ammunitionConfig.Value.minPoolSize,
                    maxSize = ammunitionConfig.Value.minPoolSize
                });
        }

        m_WeaponFactory.Init(weaponConfigList,
            (weaponType, weapon) => { m_WeaponPool.ReleaseObject(weaponType, weapon); });

        m_AmmunitionFactory.Init(ammunitionConfigList,
            (ammunitionType, ammunition) => { m_AmmunitionPool.ReleaseObject(ammunitionType, ammunition); });
    }

    public (GameObject, WeaponConfig) GetWeapon(WeaponType weaponType)
    {
        return (m_WeaponPool.GetObject(weaponType),
            m_WeaponFactory.GetWeaponConfig(weaponType));
    }

    // TODO:爆炸物可能同时影响到多个unit，内部已经处理ammunition重复加入的问题，这里只是需要拿到表
    // 但是之前就被注销掉的ammunition，后续就拿不到映射关系

    /// <summary>
    /// 伤害判定流程
    /// </summary>
    public void JudgeWithAmmunition(GameObject unitAtker, GameObject unitToDamage, GameObject ammunition)
    {
        // TODO: 结算流程
        InternalUnRegisterAmmunition(ammunition);
    }

    public void FireWith(GameObject weapon, Vector2 startPoint, Vector2 dir)
    {
        if (!m_WeaponFactory.HasWeapon(weapon)) throw new Exception("This weapon is not in WeaponUpdater");

        WeaponConfig weaponConfig = m_WeaponFactory.GetWeaponHandle(weapon).weaponConfig;
        AmmunitionType ammunitionType = weaponConfig.ammunitionType;
        // 注册子弹
        // TODO;修改方向
        var (ammunition, ammunitionConfig) = InternalGetAmmunition(ammunitionType, startPoint, quaternion.identity);
        InternalRegisterAmmunition(ammunition, ammunitionConfig, weaponConfig.atkType, startPoint, dir);
    }

    public void RegisterWeapon(GameObject weapon, WeaponConfig weaponConfig)
    {
        m_WeaponFactory.RegisterWeapon(weapon, weaponConfig);
    }

    public void UnRegisterWeapon(GameObject weapon)
    {
        WeaponType weaponType = m_WeaponFactory.GetWeaponType(weapon);
        m_WeaponFactory.UnRegisterWeapon(weapon);
    }

    private void InternalUnRegisterAmmunition(GameObject ammunition)
    {
        m_AmmunitionFactory.UnRegisterAmmunition(ammunition);
    }

    private void InternalRegisterAmmunition(GameObject ammunition, AmmunitionConfig ammunitionConfig,
        AtkType atkType, Vector2 startPoint,
        Vector2 dir)
    {
        m_AmmunitionFactory.RegisterAmmunition(ammunition, ammunitionConfig, atkType, startPoint, dir);
    }

    private (GameObject, AmmunitionConfig) InternalGetAmmunition(AmmunitionType ammunitionType, Vector3 startPoint, Quaternion quaternion)
    {
        return (m_AmmunitionPool.GetObject(ammunitionType ,startPoint, quaternion),
            m_AmmunitionFactory.GetAmmunitionConfig(ammunitionType));
    }
}