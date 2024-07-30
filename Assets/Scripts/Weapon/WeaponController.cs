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

    private void Start()
    {
        List<KeyValuePair<WeaponType, WeaponSystemConfig>> weaponConfigList = new ();
        List<KeyValuePair<AmmunitionType, AmmunitionConfig>> ammunitionConfigList = new();

        foreach (var weaponCat in m_WeaponCats)
        {
            weaponConfigList.Add(new KeyValuePair<WeaponType, WeaponSystemConfig>(weaponCat.weaponType, weaponCat.weaponSystemConfig));
        }

        foreach (var ammunitionCat in m_AmmunitionCats)
        {
            ammunitionConfigList.Add(new KeyValuePair<AmmunitionType, AmmunitionConfig>(ammunitionCat.ammunitionType, ammunitionCat.ammunitionConfig));
        }
        
        WeaponSystemCenter.Instance.Init(weaponConfigList, ammunitionConfigList);
    }
}