using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WallToDestroyAmmunition : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        AmmunitionFactory ammunitionFactory = WeaponSystemCenter.GetAmmunitionFactory();
        ammunitionFactory.UnRegisterAmmunition(other.gameObject);
    }
}
