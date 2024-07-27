using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerMovement
{
    private GameObject m_Player;
    private float m_NormalSpeed;
    private float m_Speed;
    private float m_DashTime;
    private float m_DashSpeed;
    private float m_DashCoolDownTime;
    private int m_DashCount;
    private int m_MaxDashCount;
    private Rigidbody2D m_Rigidbody;


    private float _dashRemainTime = -1;
    private float _dashCoolDownRemainTime = -1;
    private Vector2 _dashDir = Vector2.zero;
//  public-----------------------------------------
    public void Init(PlayerMovementSpec spec)
    {
        m_Player = spec.m_Player;
        m_NormalSpeed = spec.m_NormalSpeed;
        m_Speed = spec.m_NormalSpeed;
        m_DashTime = spec.m_DashTime;
        m_DashSpeed = spec.m_DashSpeed;
        m_DashCoolDownTime = spec.m_DashCoolDownTime;
        m_DashCount = spec.m_DashCount;
        m_MaxDashCount = spec.m_MaxDashCount;

        m_Rigidbody = m_Player.GetComponent<Rigidbody2D>();
        _dashCoolDownRemainTime = m_DashCoolDownTime;
    }

    public void Move(Vector2 dir)
    {
        var v2 = m_Rigidbody.position;
        dir = dir.normalized;
        v2.x += dir.x * m_Speed * Time.deltaTime;
        v2.y += dir.y * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(v2);
    }

    public void LookAt(Vector2 dir)
    {
        dir = dir.normalized;
        float angle = Vector2.Angle(new Vector2(0, 1), dir);
        m_Player.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f);
    }

    public void Dash()
    {
        var v2 = m_Rigidbody.position;
        v2.x += _dashDir.x * m_DashSpeed * Time.fixedDeltaTime;
        v2.y += _dashDir.y * m_DashSpeed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(v2);
    }

    public void SetDashDirection(Vector2 dir)
    {
        _dashDir = dir.normalized;
    }

    public bool StartDash()
    {
        if(m_DashCount > 0)
        {
            m_DashCount--;
            return true;
        }
        return false;
    }

    public Vector3 GetPlayerPosition()
    {
        return m_Player.transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return m_Player.transform.rotation;
    }


    public void Update()
    {
        UpdateDashCount();
    }

    public void FixedUpdate()
    {
;
    }

//  private-----------------------------------------------------------------------

    private void UpdateDashCount()
    {
        if(m_DashCount < m_MaxDashCount)
        {
            _dashCoolDownRemainTime -= Time.deltaTime;
            if(_dashCoolDownRemainTime < 0)
            {
                m_DashCount++;
                _dashCoolDownRemainTime = m_DashCoolDownTime;
            }
        }
    }
}

public struct PlayerMovementSpec
{
    public GameObject m_Player;
    public float m_NormalSpeed;
    public float m_DashTime;
    public float m_DashSpeed;
    public float m_DashCoolDownTime;
    public int m_DashCount;
    public int m_MaxDashCount;
}
