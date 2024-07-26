using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private GameObject m_Player;
    private PlayerMovement m_PlayerMovement;

    //  public--------------------------------------------------
    public void Init(GameObject player, float moveSpeed)
    {
        m_Player = player;

        m_PlayerMovement = new PlayerMovement();
        m_PlayerMovement.Init(player, moveSpeed);
    }

    public Vector3 GetPlayerPosition()
    {
        return m_Player.transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return m_Player.transform.rotation;
    }

    public void Move(Vector3 dir)
    {
        m_PlayerMovement.Move(dir);
    }

    public void LookAt(Vector2 dir)
    {
        m_PlayerMovement.LookAt(dir);
    }
}
