using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public struct WeaponInstanceData
{
    public int currentMag;
    public float lastFiredTime;
}

public class WeaponInstance : NetworkBehaviour
{
    public Transform firePoint;
    
    private WeaponInstanceData m_WeaponInstanceData;
    private Animator m_Animator;
    private int fireHash = Animator.StringToHash("fire");

    [SyncVar] private int m_WeaponHP;
    [SyncVar] private int m_WeaponCurrentHP;
    [SyncVar] private int m_CurrentMag;

    private WeaponConfig m_WeaponConfig;

    private LineRenderer m_LineRenderer;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Init(WeaponConfig weaponConfig)
    {
        m_WeaponConfig = weaponConfig;
        m_WeaponHP = weaponConfig.weaponHp;
        m_WeaponCurrentHP = m_WeaponHP;
        m_WeaponInstanceData.currentMag = weaponConfig.magSize;
        m_CurrentMag = m_WeaponInstanceData.currentMag;
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    public LineRenderer lineRenderer => m_LineRenderer;

    /// <summary>
    /// 尝试开火，如果距离上一次开火时间大于武器设置开火间隔，则可以开火，将会重新设置时间并返回true
    /// </summary>
    /// <returns>是否可以开火</returns>
    public bool TryFire()
    {
        bool canFire = Time.time - m_WeaponInstanceData.lastFiredTime >= m_WeaponConfig.interval;

        // 只有在可以开火时才会重新设置时间
        if (canFire)
        {
            m_WeaponInstanceData.lastFiredTime = Time.time;
        }

        return canFire;
    }

    /// <summary>
    /// 消耗子弹数量，当子弹小于等于0时会返回false
    /// </summary>
    /// <returns></returns>
    public bool DecreaseMag()
    {
        m_WeaponInstanceData.currentMag--;
        m_CurrentMag--;
        return m_WeaponInstanceData.currentMag > 0;
    }

    public int GetWeaponHP()
    {
        return m_WeaponHP;
    }

    public int GetWeaponCurrentHP()
    {
        return m_WeaponCurrentHP;
    }

    public void SetWeaponCurrentHP(int val)
    {
        m_WeaponCurrentHP = val >= 0 ? val : 0;
    }

    public int GetCurrentMag()
    {
        return m_CurrentMag;
    }

    public void FireVfxAndAnimation(GameObject character, WeaponType weaponType, WeaponConfig weaponConfig, Vector3 startPoint,
        Vector3 dir)
    {
        var audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            //Debug.LogError(weaponType+"武器预制体未挂载音效组件");
        }

        if (audioSource.clip != null)
        {
            audioSource.Play();
            //Debug.LogError("Playing" + $"{audioSource.clip.name}");
        }
        
        
        
        // 普通武器开火动画，激光需要特殊处理
        if (weaponType != WeaponType.CombatLaserGun && weaponType != WeaponType.HeavyLaserCannon)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); // -90 to adjust from (0, 1) to (1, 0)
            VfxPool.Instance.GetVfx(weaponConfig.fireVfxType, startPoint, rotation);
        }
        else
        {
            // 仅限激光
            if (m_LineRenderer)
            {
                m_LineRenderer.startWidth = weaponConfig.LaserPointerWidth;
                m_LineRenderer.endWidth = weaponConfig.LaserPointerWidth;
                m_LineRenderer.startColor = Color.green;
                m_LineRenderer.endColor = Color.green;
                m_LineRenderer.positionCount = 2;
            
                // 最大射线距离为子弹最远距离
                AmmunitionType ammunitionType = weaponConfig.ammunitionType;
                AmmunitionConfig ammunitionConfig = WeaponSystemCenter.GetAmmunitionFactory().GetAmmunitionConfig(ammunitionType);
                int ignoreLayer = ~(LayerMask.GetMask("Bullet") | LayerMask.GetMask("Ground") | LayerMask.GetMask("Sensor"));

                HashSet<GameObject> ignoredObjects = character.GetComponent<AutoGetChild>().ignoredObjects;

                RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, dir, ammunitionConfig.lifeDistance, ignoreLayer);

                Vector2 endPoint = startPoint + dir.normalized * ammunitionConfig.lifeDistance;

                foreach (var hit in hits)
                {
                    if (hit.collider != null && !hit.collider.isTrigger && !ignoredObjects.Contains(hit.collider.gameObject))
                    {
                        // 如果碰撞到非触发器且不在忽略列表中的物体，使用碰撞点作为终点
                        endPoint = hit.point;
                        break; // 找到第一个非触发器碰撞后停止
                    }
                }

                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, endPoint);
            }
        }

        if (m_Animator)
        {
            m_Animator.SetBool(fireHash, true);
        }
    }

    public void UnFireVfxAndAnimation()
    {
        if (m_Animator)
        {
            m_Animator.SetBool(fireHash, false);
        }

        if (m_LineRenderer)
        {
            m_LineRenderer.positionCount = 0;
        }
    }
}