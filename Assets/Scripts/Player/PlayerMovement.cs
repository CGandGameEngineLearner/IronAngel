using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private GameObject m_Player;
    private float m_Speed;

//  public-----------------------------------------
    public void Init(GameObject player, float speed)
    {
        m_Player = player;
        m_Speed = speed;
    }

    public void Move(Vector3 dir)
    {
        m_Player.transform.Translate(dir * m_Speed);
    }

    public void LookAt(Vector3 dir)
    {
        m_Player.transform.LookAt(dir);
    }
}
