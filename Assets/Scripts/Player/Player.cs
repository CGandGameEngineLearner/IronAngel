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
        m_PlayerHand.Init(handSpec);

        m_PlayerProperties = new PlayerProperties();
        PlayerPropertiesSpec propertiesSpec = new PlayerPropertiesSpec();
        propertiesSpec.m_Energy = spec.m_Energy;
        propertiesSpec.m_EnergyThreshold = spec.m_EnergyThreshold;
        propertiesSpec.m_EnergyLimition = spec.m_EnergyLimition;
        propertiesSpec.m_BaseHP = spec.m_BaseHP;
        propertiesSpec.m_Armor = spec.m_Armor;
        m_PlayerProperties.Init(propertiesSpec);
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

    public void SetPlayerEnergy(int val)
    {
        m_PlayerProperties.SetEnergy(val);
    }

    public int GetPlayerEnergy()
    {
        return m_PlayerProperties.GetEnergy();
    }

    public void ChangePlayerEnergy(int val)
    {
        m_PlayerProperties.ChangeEnergy(val);
    }

    public void SetPlayerArmor(int val)
    {
        m_PlayerProperties.SetArmor(val);
    }

    public int GetPlayerArmor()
    {
        return m_PlayerProperties.GetArmor();
    }

    public void SetPlayerBaseHP(int val)
    {
        m_PlayerProperties.SetBaseHP(val);
    }

    public int GetPlayerBaseHP()
    {
        return m_PlayerProperties.GetBaseHP();
    }

    public int GetPlayerCurrentHP()
    {
        return m_PlayerProperties.GetCurrentHP();
    }

    public void SetPlayerCurrentHP(int val)
    {
        m_PlayerProperties.SetCurrentHP(val);
    }

    public void ChangePlayerCurrentHP(int val)
    {
        m_PlayerProperties.ChangeCurrentHP(val);
    }

    public void SetPlayerCurrentArmor(int val)
    {
        m_PlayerProperties.SetCurrentArmor(val);
    }

    public int GetPlayerCurrentArmor()
    {
        return m_PlayerProperties.GetCurrentArmor();
    }

    public void ChangePlayerCurrentArmor(int val)
    {
        m_PlayerProperties.ChangeCurrentArmor(val);
    }

    public void SetPlayerEnergyThreshold(int val)
    {
        m_PlayerProperties.SetEnergyThreshold(val);
    }

    public int GetPlayerEnergyThreshold()
    {
        return m_PlayerProperties.GetEnergyThreshold();
    }

    public void SetPlayerEnergyLimition(int val)
    {
        m_PlayerProperties.SetEnergyLimition(val);
    }

    public int GetPlayerEnergyLimition()
    {
        return m_PlayerProperties.GetEnergyLimition();
    }

    public Vector3 GetPlayerLeftHandPosition()
    {
        return m_PlayerHand.GetPlayerLeftHandPosition();
    }

    public Vector3 GetPlayerRightHandPosition()
    {
        return m_PlayerHand.GetPlayerRightHandPosition();
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
    public int m_Energy;
    public int m_EnergyThreshold;
    public int m_EnergyLimition;
    public int m_BaseHP;
    public int m_Armor;
}
