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
    private LayerMask m_WallLayer;


    private float _dashCoolDownRemainTime = -1;
    private Vector2 _dashDir = Vector2.zero;
//  public-----------------------------------------
    public void Init(PlayerMovementSpec spec)
    {
        m_Player = spec.m_Player;
        m_NormalSpeed = spec.m_NormalSpeed;
        m_Speed = spec.m_NormalSpeed;
        m_DashSpeed = spec.m_DashSpeed;
        m_DashCoolDownTime = spec.m_DashCoolDownTime;
        m_DashCount = spec.m_DashCount;
        m_MaxDashCount = spec.m_MaxDashCount;
        m_WallLayer = spec.m_WallLayer;

        m_Rigidbody = m_Player.GetComponent<Rigidbody2D>();
        _dashCoolDownRemainTime = m_DashCoolDownTime;
    }

    /// <summary>
    /// 改变速度
    /// </summary>
    /// <param name="speed"></param> 这个是delta值
    public void SetSpeed(float speed)
    {
        m_Speed += speed;
        m_Speed = m_Speed > 0 ? m_Speed : 0;
    }

    public void ResetSpeed()
    {
        m_Speed = m_NormalSpeed;
    }

    public void Move(Vector2 dir)
    {
        m_Rigidbody = m_Player.GetComponent<Rigidbody2D>();
        var v2 = m_Rigidbody.position;
        dir = dir.normalized;
        v2.x += dir.x * m_Speed * Time.deltaTime;
        v2.y += dir.y * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(v2);
    }

    public void LookAt(Vector2 dir)
    {
        dir = dir.normalized;
        
        // 用鼠标转换出来的坐标计算会差90°
        m_Rigidbody.rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f;
    }

    public void Dash()
    {
        var v2 = m_Rigidbody.position;
        v2.x += _dashDir.x * m_DashSpeed * Time.fixedDeltaTime;
        v2.y += _dashDir.y * m_DashSpeed * Time.fixedDeltaTime;
        var hit = Physics2D.Raycast(m_Rigidbody.position, _dashDir, Vector2.Distance(m_Rigidbody.position, v2) + 1, m_WallLayer);
        if(hit && Vector2.Distance(m_Rigidbody.position, v2) >= Vector2.Distance(m_Rigidbody.position, hit.point))
        {
            v2 = hit.point - _dashDir * 0.1f;
        }
        m_Rigidbody.MovePosition(v2);
    }

    public void SetDashDirection(Vector2 dir)
    {
        _dashDir = dir.normalized;
    }

    public int GetDashCount()
    {
        return m_DashCount;
    }

    public bool StartDash()
    {
        return m_DashCount > 0;
    }

    public Vector3 GetPlayerPosition()
    {
        return m_Player.transform.position;
    }

    public Quaternion GetPlayerRotation()
    {
        return m_Player.transform.rotation;
    }

    public void ChangeDashCount(int val)
    {
        m_DashCount = m_DashCount + val >= 0 ?  m_DashCount + val : 0;
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
    public float m_DashSpeed;
    public float m_DashCoolDownTime;
    public int m_DashCount;
    public int m_MaxDashCount;
    public LayerMask m_WallLayer;
}
