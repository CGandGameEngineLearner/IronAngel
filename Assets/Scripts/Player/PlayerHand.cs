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

//  public------------------------------------------------
    public void Init(PlayerHandSpec spec)
    {
        m_Player = spec.m_Player;
        m_PlayerLeftHand = spec.m_PlayerLeftHand ;
        m_PlayerRightHand = spec.m_PlayerRightHand;
        m_DetectRange = spec.m_DetectRange;
        m_WeaponLayer = spec.m_WeaponLayer;
    }

    public Vector3 GetPlayerLeftHandPosition()
    {
        return m_PlayerLeftHand.transform.position;
    }

    public Vector3 GetPlayerRightHandPosition()
    {
        return m_PlayerRightHand.transform.position;
    }

    public void SetPlayerLeftHandObject(GameObject obj)
    {
        m_ObjectInLeftHand = obj;
    }

    public void SetPlayerRightHandObject(GameObject obj)
    {
        m_ObjectInRightHand = obj;
    }

    public GameObject GetPlayerLeftHandObject()
    {
        return m_ObjectInLeftHand;
    }

    public GameObject GetPlayerRightHandObject()
    {
        return m_ObjectInRightHand;
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
}

public struct PlayerHandSpec
{
    public GameObject m_Player;
    public GameObject m_PlayerLeftHand;
    public GameObject m_PlayerRightHand;
    public float m_DetectRange;
    public LayerMask m_WeaponLayer;
}
