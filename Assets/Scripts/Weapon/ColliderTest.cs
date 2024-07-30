using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ColliderTest : NetworkBehaviour
{
    [Server]
    private void OnTriggerEnter2D(Collider2D other)
    {
        WeaponSystemCenter.Instance.JudgeWithAmmunition(gameObject, other.gameObject);
    }
}