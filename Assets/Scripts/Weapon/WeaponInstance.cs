using Mirror;
using UnityEngine;

public struct WeaponInstanceData
{
    public int currentMag;
    public float lastFiredTime;
    public int weaponHp;
}

public class WeaponInstance : NetworkBehaviour
{
    [SyncVar]
    private WeaponInstanceData m_WeaponInstanceData;
    
    private WeaponConfig m_WeaponConfig;

    public void Init(WeaponConfig weaponConfig)
    {
        m_WeaponConfig = weaponConfig;

        m_WeaponInstanceData.currentMag = weaponConfig.magSize;
        m_WeaponInstanceData.weaponHp = weaponConfig.weaponHp;
    }

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
        m_WeaponInstanceData.currentMag --;
        return m_WeaponInstanceData.currentMag > 0;
    }
}