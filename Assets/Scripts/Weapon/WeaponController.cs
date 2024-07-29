using System;
using System.Collections.Generic;
using Mirror.BouncyCastle.Bcpg.Sig;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private List<WeaponCat> m_WeaponCats = new List<WeaponCat>();
    [SerializeField] private List<AmmunitionCat> m_AmmunitionCats = new();

    [Space] 
    [SerializeField] private GameObject weapon;


    private WeaponSystemCenter m_WeaponSystemCenter = new();

    private void Start()
    {
        List<KeyValuePair<WeaponType, WeaponConfig>> weaponConfigList = new();
        List<KeyValuePair<AmmunitionType, AmmunitionConfig>> ammunitionConfigList = new();

        foreach (var weaponCat in m_WeaponCats)
        {
            weaponConfigList.Add(
                new KeyValuePair<WeaponType, WeaponConfig>(weaponCat.weaponType, weaponCat.weaponConfig));
        }

        foreach (var ammunitionCat in m_AmmunitionCats)
        {
            ammunitionConfigList.Add(new KeyValuePair<AmmunitionType, AmmunitionConfig>(ammunitionCat.ammunitionType,
                ammunitionCat.ammunitionConfig));
        }

        m_WeaponSystemCenter.Init(weaponConfigList, ammunitionConfigList);


        var (newWeapon, newConfig) = m_WeaponSystemCenter.GetWeapon(WeaponType.Glock);
        this.weapon = newWeapon;
        m_WeaponSystemCenter.RegisterWeapon(weapon, newConfig);

        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        m_WeaponSystemCenter.Update();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -10; // 设置深度值
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            m_WeaponSystemCenter.FireWith(weapon, transform.position, worldPosition - transform.position);
        }
    }
}