using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Utils.GlobalController.WeaponSystemCenter.JudgeWithAmmunition(gameObject, other.gameObject);
    }
}
