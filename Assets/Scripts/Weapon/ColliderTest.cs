using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Utils.playerController.WeaponSystemCenter.JudgeWithAmmunition(gameObject, other.gameObject);
    }
}
