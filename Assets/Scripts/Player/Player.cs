using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private GameObject m_Player;
    private PlayerMovement m_PlayerMovement;

    //  public--------------------------------------------------
    public void Init(PlayerSpec spec)
    {
        m_Player = spec.m_Player;

        m_PlayerMovement = new PlayerMovement();
        PlayerMovementSpec moveSpec = new PlayerMovementSpec();
        moveSpec.m_Player = m_Player;
        moveSpec.m_NormalSpeed = spec.m_NormalSpeed;
        moveSpec.m_DashTime = spec.m_DashTime;
        moveSpec.m_DashSpeed = spec.m_DashSpeed;
        moveSpec.m_DashCoolDownTime = spec.m_DashCoolDownTime;
        moveSpec.m_DashCount = spec.m_DashCount;
        moveSpec.m_MaxDashCount = spec.m_MaxDashCount;
        m_PlayerMovement.Init(moveSpec);
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

    public void Dash(Vector2 dir)
    {
        m_PlayerMovement.Dash(dir);
    }
    public GameObject GetPlayer()
    {
        return m_Player;
    }

    public void Update()
    {
        m_PlayerMovement.Update();
    }

    public void FixedUpdate()
    {
        m_PlayerMovement.FixedUpdate();
    }
}

public struct PlayerSpec
{
    public GameObject m_Player;

    // Movement
    public float m_NormalSpeed;
    public float m_DashTime;
    public float m_DashSpeed;
    public float m_DashCoolDownTime;
    public int m_DashCount;
    public int m_MaxDashCount;
}
