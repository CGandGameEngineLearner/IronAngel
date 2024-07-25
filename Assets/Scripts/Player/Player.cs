using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private GameObject m_Player;

//  public--------------------------------------------------
    public void Init(GameObject player)
    {
        m_Player = player;
    }

    public Vector3 GetPlayerPosition()
    {
        return m_Player.transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return m_Player.transform.rotation;
    }
}
