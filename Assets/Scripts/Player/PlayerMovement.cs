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
        m_Player.transform.Translate(dir * m_Speed * Time.deltaTime, Space.World);
    }

    public void LookAt(Vector2 dir)
    {
        dir = dir.normalized;
        float angle = Vector2.Angle(new Vector2(0, 1), dir);
        m_Player.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f);
    }
}
