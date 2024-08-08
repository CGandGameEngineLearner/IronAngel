using Mirror;
using UnityEngine;

public struct WeaponInstanceData
{
    public int currentMag;
    public float lastFiredTime;

}

public class WeaponInstance : NetworkBehaviour
{
    private WeaponInstanceData m_WeaponInstanceData;

    [SyncVar] private int m_WeaponHP;
    [SyncVar] private int m_WeaponCurrentHP;
    [SyncVar] private int m_CurrentMag;
    
    private WeaponConfig m_WeaponConfig;

    private LineRenderer m_LineRenderer;

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

    [ClientCallback]
    public WeaponConfig GetConfig() => m_WeaponConfig;
    
    /// <summary>
    /// 消耗子弹数量，当子弹小于等于0时会返回false
    /// </summary>
    /// <returns></returns>
    public bool DecreaseMag()
    {
        m_WeaponInstanceData.currentMag --;
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
}