using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandle
{
    public GameObject weapon;
    public WeaponConfig weaponConfig;
    public int currentMag;

    public WeaponHandle Init(GameObject weapon, WeaponConfig weaponConfig)
    {
        this.weapon = weapon;
        this.weaponConfig = weaponConfig;
        return this;
    }

    public WeaponHandle Clear()
    {
        this.weapon = null;
        this.weaponConfig = null;
        this.currentMag = 0;

        return this;
    }
}


public class WeaponUpdater
{
    private Dictionary<GameObject, WeaponHandle> m_WeaponHandles = new();
    private Queue<WeaponHandle> m_UsableHandles = new();

    public bool HasWeapon(GameObject weapon) => m_WeaponHandles.ContainsKey(weapon);

    public WeaponHandle GetWeaponHandle(GameObject weapon) => m_WeaponHandles[weapon];

    public void RegisterWeapon(GameObject weapon, WeaponConfig weaponConfig)
    {
        if (m_WeaponHandles.ContainsKey(weapon)) throw new Exception("This weapon already in WeaponUpdater!");

        m_WeaponHandles.Add(weapon, InternalGetWeaponHandle(weapon, weaponConfig));
    }

    public void UnRegisterWeapon(GameObject weapon)
    {
        if (!m_WeaponHandles.ContainsKey(weapon)) throw new Exception("This weapon is not in WeaponUpdater!");
        WeaponHandle weaponHandle = m_WeaponHandles[weapon];

        m_WeaponHandles.Remove(weapon);

        ObjectPoolManager.Instance.ReleaseObject(weaponHandle.weaponConfig.weaponType.ToString(), weapon);
        m_UsableHandles.Enqueue(weaponHandle.Clear());
    }

    private WeaponHandle InternalGetWeaponHandle(GameObject weapon, WeaponConfig weaponConfig)
    {
        return m_UsableHandles.Count > 0
            ? m_UsableHandles.Dequeue().Init(weapon, weaponConfig)
            : (new WeaponHandle()).Init(weapon, weaponConfig);
    }
}