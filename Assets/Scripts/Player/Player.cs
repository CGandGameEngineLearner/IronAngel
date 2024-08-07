using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private GameObject m_Player;
    private PlayerMovement m_PlayerMovement;
    private PlayerHand m_PlayerHand;

    //  public--------------------------------------------------
    public void Init(PlayerSpec spec)
    {
        m_Player = spec.m_Player;

        m_PlayerMovement = new PlayerMovement();
        PlayerMovementSpec moveSpec = new PlayerMovementSpec();
        moveSpec.m_Player = m_Player;
        moveSpec.m_NormalSpeed = spec.m_NormalSpeed;
        moveSpec.m_DashSpeed = spec.m_DashSpeed;
        moveSpec.m_DashCoolDownTime = spec.m_DashCoolDownTime;
        moveSpec.m_DashCount = spec.m_DashCount;
        moveSpec.m_MaxDashCount = spec.m_MaxDashCount;
        moveSpec.m_WallLayer = spec._WallLayer;
        m_PlayerMovement.Init(moveSpec);

        m_PlayerHand = new PlayerHand();
        PlayerHandSpec handSpec = new PlayerHandSpec();
        handSpec.m_Player = m_Player;
        handSpec.m_PlayerLeftHand = spec.m_PlayerLeftHand;
        handSpec.m_PlayerRightHand = spec.m_PlayerRightHand;
        handSpec.m_DetectRange = spec.m_DetectRange;
        handSpec.m_WeaponLayer = spec.m_WeaponLayer;
        m_PlayerHand.Init(handSpec);
    }

    public void SetPlayer(GameObject player)
    {
        m_Player = player;

    }

    public int GetDashCount()
    {
        return m_PlayerMovement.GetDashCount();
    }

    public Vector3 GetPlayerPosition()
    {
        return m_Player.transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return m_Player.transform.rotation;
    }

    public void SetSpeed(float speed)
    {
        m_PlayerMovement.SetSpeed(speed);
    }

    public void ResetSpeed()
    {
        m_PlayerMovement.ResetSpeed();
    }

    public void Move(Vector2 dir)
    {
        m_PlayerMovement.Move(dir);
    }

    public void LookAt(Vector2 dir)
    {
        m_PlayerMovement.LookAt(dir);
    }

    public void Dash()
    {
        m_PlayerMovement.Dash();
    }
    public GameObject GetPlayer()
    {
        return m_Player;
    }

    public void SetDashDirection(Vector2 dir)
    {
        m_PlayerMovement.SetDashDirection(dir);
    }

    public void Update()
    {
        m_PlayerMovement.Update();
    }

    public void FixedUpdate()
    {
        m_PlayerMovement.FixedUpdate();
    }

    public bool StartDash()
    {
        return m_PlayerMovement.StartDash();
    }

    public void ChangeDashCount(int val)
    {
        m_PlayerMovement.ChangeDashCount(val);
    }

    public void SetPlayerLeftHandWeapon(GameObject obj)
    {
        m_PlayerHand.SetPlayerLeftHandWeapon(obj);
    }

    public void SetPlayerRightHandWeapon(GameObject obj)
    {
        m_PlayerHand.SetPlayerRightHandWeapon(obj);
    }

    public GameObject GetPlayerLeftHandWeapon()
    {
        return m_PlayerHand.GetPlayerLeftHandWeapon();
    }

    public GameObject GetPlayerRightHandWeapon()
    {
        return m_PlayerHand.GetPlayerRightHandWeapon();
    }

    public Vector3 GetPlayerLeftHandPosition()
    {
        return m_PlayerHand.GetPlayerLeftHandPosition();
    }

    public Vector3 GetPlayerRightHandPosition()
    {
        return m_PlayerHand.GetPlayerRightHandPosition();
    }

    public GameObject GetNearestWeapon()
    {
        return m_PlayerHand.GetNearestWeapon();
    }

    public GameObject DropPlayerLeftHandWeapon(Vector3 pos)
    {
        return m_PlayerHand.DropPlayerLeftHandWeapon(pos);
    }

    public GameObject DropPlayerRightHandWeapon(Vector3 pos)
    {
        return m_PlayerHand.DropPlayerRightHandWeapon(pos);
    }
}


[Serializable]
public struct PlayerSpec
{
    public GameObject m_Player;

    // Movement
    public float m_NormalSpeed;
    public float m_DashSpeed;
    public float m_DashCoolDownTime;
    public int m_DashCount;
    public int m_MaxDashCount;

    // Hand
    public GameObject m_PlayerLeftHand;
    public GameObject m_PlayerRightHand;
    public float m_DetectRange;
    public LayerMask m_WeaponLayer;

    // wall layer
    public LayerMask _WallLayer;
}
