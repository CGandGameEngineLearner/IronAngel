using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private GameObject m_Player;
    private PlayerMovement m_PlayerMovement;
    private PlayerHand m_PlayerHand;
    private PlayerProperties m_PlayerProperties;

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
        m_PlayerMovement.Init(moveSpec);

        m_PlayerHand = new PlayerHand();
        PlayerHandSpec handSpec = new PlayerHandSpec();
        handSpec.m_PlayerLeftHand = spec.m_PlayerLeftHand;
        handSpec.m_PlayerRightHand = spec.m_PlayerRightHand;

        m_PlayerProperties = new PlayerProperties();
        PlayerPropertiesSpec propertiesSpec = new PlayerPropertiesSpec();
        propertiesSpec.m_Energy = spec.m_Energy;
        propertiesSpec.m_EnergyThreshold = spec.m_EnergyThreshold;
    }

    public Vector3 GetPlayerPosition()
    {
        return m_Player.transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return m_Player.transform.rotation;
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

    public void SetPlayerLeftHandObject(GameObject obj)
    {
        m_PlayerHand.SetPlayerLeftHandObject(obj);
    }

    public void SetPlayerRightHandObject(GameObject obj)
    {
        m_PlayerHand.SetPlayerRightHandObject(obj);
    }

    public GameObject GetPlayerLeftHandObject()
    {
        return m_PlayerHand.GetPlayerLeftHandObject();
    }

    public GameObject GetPlayerRightHandObject()
    {
        return m_PlayerHand.GetPlayerRightHandObject();
    }
}

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

    // Properties
    public float m_Energy;
    public float m_EnergyThreshold;
}
