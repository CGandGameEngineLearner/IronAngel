using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand
{
    private GameObject m_PlayerLeftHand;
    private GameObject m_PlayerRightHand;

    private GameObject m_ObjectInLeftHand;
    private GameObject m_ObjectInRightHand;

//  public------------------------------------------------
    public void Init(PlayerHandSpec spec)
    {
        m_PlayerLeftHand = spec.m_PlayerLeftHand ;
        m_PlayerRightHand = spec.m_PlayerRightHand;
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
}

public struct PlayerHandSpec
{
    public GameObject m_PlayerLeftHand;
    public GameObject m_PlayerRightHand;
}
