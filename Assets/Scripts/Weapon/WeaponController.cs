using System;
using System.Collections.Generic;
using Mirror.BouncyCastle.Bcpg.Sig;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private ObjectPoolCategory m_ObjectPoolCategory;


    private AWeapon m_Weapon = new LazerWeapon();

    private ObjectPoolManager m_PoolManager = new ObjectPoolManager();

    private void Start()
    {
        m_PoolManager.Init(new() { m_ObjectPoolCategory });
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -10; // 设置深度值

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            m_Weapon.LogicShoot(character.position, worldPosition);
            m_Weapon.AppearanceUpdate(character.position, worldPosition);
        }
    }
}