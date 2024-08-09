using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand
{
    private GameObject m_Player;
    private GameObject m_PlayerLeftHand;
    private GameObject m_PlayerRightHand;

    private float m_DetectRange;
    private LayerMask m_WeaponLayer;

    private GameObject m_ObjectInLeftHand;
    private GameObject m_ObjectInRightHand;

    private BaseProperties m_BaseProperties;

//  public------------------------------------------------
    public void Init(PlayerHandSpec spec)
    {
        m_Player = spec.m_Player;
        m_PlayerLeftHand = spec.m_PlayerLeftHand ;
        m_PlayerRightHand = spec.m_PlayerRightHand;
        m_DetectRange = spec.m_DetectRange;
        m_WeaponLayer = spec.m_WeaponLayer;

        m_BaseProperties = m_Player.GetComponent<BaseProperties>();
    }

    public Vector3 GetPlayerLeftHandPosition()
    {
        return m_PlayerLeftHand.transform.position;
    }

    public Vector3 GetPlayerRightHandPosition()
    {
        return m_PlayerRightHand.transform.position;
    }

    public void SetPlayerLeftHandWeapon(GameObject obj)
    {
        m_ObjectInLeftHand = obj;
        if (m_ObjectInLeftHand != null)
        {
            if(obj.TryGetComponent<BoxCollider2D>(out var collider))
            {
                collider.enabled = false;
            }
            //obj.SetActive(false);
            m_PlayerLeftHand.SetActive(true);
            m_BaseProperties.m_Properties.m_LeftHandWeaponHP = obj.GetComponent<WeaponInstance>().GetWeaponHP();
            m_BaseProperties.m_Properties.m_LeftHandWeaponCurrentHP = obj.GetComponent<WeaponInstance>().GetWeaponCurrentHP();
            obj.transform.SetParent(m_PlayerLeftHand.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
    }

    public void SetPlayerRightHandWeapon(GameObject obj)
    {
        m_ObjectInRightHand = obj;
        if( m_ObjectInRightHand != null)
        {
            if (obj.TryGetComponent<BoxCollider2D>(out var collider))
            {
                collider.enabled = false;
            }
            //obj.SetActive(false);
            m_PlayerRightHand.SetActive(true);
            m_BaseProperties.m_Properties.m_RightHandWeaponHP = obj.GetComponent<WeaponInstance>().GetWeaponHP();
            m_BaseProperties.m_Properties.m_RightHandWeaponCurrentHP = obj.GetComponent<WeaponInstance>().GetWeaponCurrentHP();
            obj.transform.SetParent (m_PlayerRightHand.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
    }

    public GameObject GetPlayerLeftHandWeapon()
    {
        return m_ObjectInLeftHand;
    }

    public GameObject DropPlayerLeftHandWeapon(Vector3 pos)
    {
        if(m_ObjectInLeftHand != null)
        {
            m_ObjectInLeftHand.SetActive(true);
            m_ObjectInLeftHand.transform.position = pos;
            if(m_ObjectInLeftHand.TryGetComponent<BoxCollider2D>(out var collider))
            {
                collider.enabled = true;
            }
        }
        var g = m_ObjectInLeftHand;
        m_ObjectInLeftHand = null;
        m_PlayerLeftHand.SetActive(false);
        m_BaseProperties.m_Properties.m_LeftHandWeaponCurrentHP = 0;
        if(g != null)
            g.transform.parent = null;
        return g;
    }

    public GameObject GetPlayerRightHandWeapon()
    {
        return m_ObjectInRightHand;
    }

    public GameObject DropPlayerRightHandWeapon(Vector3 pos)
    {
        if(m_ObjectInRightHand != null)
        {
            m_ObjectInRightHand.SetActive(true);
            m_ObjectInRightHand.transform.position = pos;
            if(m_ObjectInRightHand.TryGetComponent<BoxCollider2D>(out var collider))
            {
                collider.enabled = true;
            }
        }
        var g = m_ObjectInRightHand;
        m_ObjectInRightHand = null;
        m_PlayerRightHand.SetActive(false);
        m_BaseProperties.m_Properties.m_RightHandWeaponCurrentHP = 0;
        if (g != null)
            g.transform.parent = null;
        return g;
    }

    public Collider2D[] DetectAllWeaponInCircle()
    {
        return Physics2D.OverlapCircleAll(m_Player.transform.position, m_DetectRange, m_WeaponLayer);
    }

    public GameObject GetNearestWeapon()
    {
        var colliders = DetectAllWeaponInCircle();
        if(colliders.Length != 0)
        {
            float minDis = float.PositiveInfinity;
            Collider2D nearestCollider = null;
            foreach (var collider in colliders)
            {
                var dis = Vector2.Distance(new Vector2(m_Player.transform.position.x, m_Player.transform.position.y), new Vector2(collider.transform.position.x, collider.transform.position.y));
                if (dis < minDis)
                {
                    minDis = dis;
                    nearestCollider = collider;
                }
            }
            return nearestCollider.gameObject;
        }
        return null;
    }

    public void FixedUpdate(Vector2 dir_left, Vector2 dir_right)
    {
        UpdateWeaponRotation(dir_left, dir_right);
    }

    public void UpdateWeaponRotation(Vector2 dir_left, Vector2 dir_right)
    {
        dir_left = dir_left.normalized;
        dir_right = dir_right.normalized;
        if (m_ObjectInLeftHand)
            m_ObjectInLeftHand.transform.rotation = Quaternion.Euler(new Vector3(0,0,Mathf.Atan2(dir_left.y, dir_left.x) * Mathf.Rad2Deg - 90.0f));
        if(m_ObjectInRightHand)
            m_ObjectInRightHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir_right.y, dir_right.x) * Mathf.Rad2Deg - 90.0f));
    }
}

public struct PlayerHandSpec
{
    public GameObject m_Player;
    public GameObject m_PlayerLeftHand;
    public GameObject m_PlayerRightHand;
    public float m_DetectRange;
    public LayerMask m_WeaponLayer;
}
