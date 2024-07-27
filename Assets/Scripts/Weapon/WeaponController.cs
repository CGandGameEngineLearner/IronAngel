using System;
using System.Collections.Generic;
using Mirror.BouncyCastle.Bcpg.Sig;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct WeaponPoolCategory
{
    public WeaponConfig weaponConfig;
    public GameObject prefab;
    public int defaultSize;
    public int maxSize;
}
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private List<WeaponPoolCategory> m_WeaponPoolCategories; 
    

    private void Start()
    {
        foreach (var weaponCategory in m_WeaponPoolCategories)
        {
            ObjectPoolCategory objectPoolCategory = new ObjectPoolCategory() {objType = weaponCategory.weaponConfig.weaponType.ToString(), };
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -10; // 设置深度值

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
           
        }
    }
}