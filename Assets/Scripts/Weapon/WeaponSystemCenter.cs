using System;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// 武器生成地点配置
/// </summary>
[Serializable]
public class WeaponSpawnSetting
{
    [SerializeField] public WeaponType WeaponType;

    [FormerlySerializedAs("Transform")] [SerializeField]
    public Vector3 Position;
}

/// <summary>
/// 武器配置
/// </summary>
[Serializable]
public class WeaponConfigSetting
{
    [SerializeField] public WeaponType WeaponType;

    [SerializeField] public WeaponConfig WeaponConfig;
}

/// <summary>
/// 子弹配置
/// </summary>
[Serializable]
public class AmmunitionConfigSetting
{
    [SerializeField] public AmmunitionType AmmunitionType;
    [SerializeField] public AmmunitionConfig AmmunitionConfig;
}

public class WeaponSystemCenter : NetworkBehaviour
{
    public static WeaponSystemCenter Instance;

    public List<WeaponSpawnSetting> WeaponSpawnSettings = new List<WeaponSpawnSetting>();
    public List<WeaponConfigSetting> WeaponConfigSettings = new List<WeaponConfigSetting>();
    public List<AmmunitionConfigSetting> AmmunitionConfigSettings = new List<AmmunitionConfigSetting>();

    private static Dictionary<WeaponType, WeaponConfig> m_WeaponConfigDic = new Dictionary<WeaponType, WeaponConfig>();

    private static Dictionary<AmmunitionType, AmmunitionConfig> m_AmmunitionConfigDic =
        new Dictionary<AmmunitionType, AmmunitionConfig>();

    /// <summary>
    /// 武器GameObject到其配置的映射
    /// </summary>
    private static Dictionary<GameObject, WeaponConfig>
        m_WeaponToConfigDic = new Dictionary<GameObject, WeaponConfig>();


    private static ObjectPoolManager<AmmunitionType> m_AmmunitionPool = new();
    private static AmmunitionFactory m_AmmunitionFactory = new(); // 弹药工厂

    /// <summary>
    /// 获取AmmunitionFactory单例
    /// </summary>
    /// <returns></returns>
    public static AmmunitionFactory GetAmmunitionFactory()
    {
        return m_AmmunitionFactory;
    }


    public static WeaponConfig GetWeaponConfig(WeaponType weaponType)
    {
        if (!m_WeaponConfigDic.ContainsKey(weaponType))
        {
            throw new Exception("查询不到武器配置，武器类型枚举为：" + weaponType);
        }

        return m_WeaponConfigDic[weaponType];
    }

    public bool StartGame = false;

    /// <summary>
    /// 通知服务器要在指定地点和方向发射子弹
    /// </summary>
    /// <param name="character"></param> 开火的角色
    /// <param name="weapon"></param>
    /// <param name="startPoint"></param>
    /// <param name="dir"></param>
    public void CmdFire(GameObject character, GameObject weapon, Vector3 startPoint, Vector3 dir)
    {
        Debug.Log(GetType() + "Command" + "Fire");
        var weaponConfig = m_WeaponToConfigDic[weapon];
        var ammunitionType = m_WeaponToConfigDic[weapon].ammunitionType;
        var ammunitionConfig = m_AmmunitionConfigDic[ammunitionType];

        // 测试武器脚本
        if (!weapon.TryGetComponent<WeaponInstance>(out WeaponInstance weaponInstance))
        {
#if UNITY_EDITOR
            Debug.LogError("武器没有挂载WeaponInstance脚本");
#endif
            return;
        }

        // 武器射击间隔
        if (!weaponInstance.TryFire())
        {
#if UNITY_EDITOR
            Debug.LogWarning("开火间隔过短");
#endif
            return;
        }

        // 减少弹匣数量
        if (!weaponInstance.DecreaseMag())
        {
#if UNITY_EDITOR
            Debug.LogWarning("子弹数不足");
#endif
            return;
        }

        // 散布
        dir = Utils.ApplyScatterY(dir, weaponConfig.spreadAngle);
        
        Fire(character, weaponConfig.ToData(), ammunitionType, startPoint, dir);
        
        // GameObject ammunition = GetAmmunitionFromPool(ammunitionType, startPoint, dir);
        // m_AmmunitionFactory.ShootAmmunition(character, ammunition, ammunitionType, ammunitionConfig,
        //     weaponConfig.atkType, startPoint, dir);
        RPCFire(character, weaponConfig.ToData(), ammunitionType, startPoint, dir);
    }

    public void CmdFireWithOutDispersion(GameObject character, GameObject weapon, Vector3 startPoint, Vector3 dir)
    {
        Debug.Log(GetType() + "Command" + "Fire");
        var weaponConfig = m_WeaponToConfigDic[weapon];
        var ammunitionType = m_WeaponToConfigDic[weapon].ammunitionType;
        var ammunitionConfig = m_AmmunitionConfigDic[ammunitionType];

        // 测试武器脚本
        if (!weapon.TryGetComponent<WeaponInstance>(out WeaponInstance weaponInstance))
        {
#if UNITY_EDITOR
            Debug.LogError("武器没有挂载WeaponInstance脚本");
#endif
            return;
        }

        // 武器射击间隔
        if (!weaponInstance.TryFire())
        {
#if UNITY_EDITOR
            Debug.LogWarning("开火间隔过短");
#endif
            return;
        }

        // 减少弹匣数量
        if (!weaponInstance.DecreaseMag())
        {
#if UNITY_EDITOR
            Debug.LogWarning("子弹数不足");
#endif
            return;
        }
        
        Fire(character, weaponConfig.ToData(), ammunitionType, startPoint, dir);

        RPCFire(character, weaponConfig.ToData(), ammunitionType, startPoint, dir);
    }

    [ClientRpc]
    public void RPCFire(GameObject character, WeaponConfigData weaponConfigData, AmmunitionType ammunitionType,
        Vector3 startPoint, Vector3 dir)
    {
        Debug.LogError(weaponConfigData.atkType);
        Fire(character, weaponConfigData, ammunitionType, startPoint, dir);
    }

    /// <summary>
    /// 统一提供给RPC和server使用
    /// </summary>
    private void Fire(GameObject character, WeaponConfigData weaponConfigData, AmmunitionType ammunitionType,
        Vector3 startPoint, Vector3 dir)
    {
        Debug.LogWarning(weaponConfigData.atkType);
        switch (weaponConfigData.atkType)
        {
            case AtkType.Laser:
                break;
            case AtkType.Rifle:
                // 普通间隔发射
                SetAmmunition(character, weaponConfigData, ammunitionType, startPoint, dir);
                break;
            case AtkType.MissileLauncher:
                // 普通间隔发射
                break;
            case AtkType.ShotGun:
                // 扇形射出
                FireShotgun(character, weaponConfigData, ammunitionType, startPoint, dir);
                break;
            case AtkType.Default:
                break;
        }
    }

    private void FireShotgun(GameObject character, WeaponConfigData weaponConfigData, AmmunitionType ammunitionType,
        Vector3 startPoint, Vector3 dir)
    {
        int numberOfProjectiles = weaponConfigData.simShots;
        float spreadAngle = weaponConfigData.shotSpreadAngle;
        
        float sigma = spreadAngle / 6; // 选择标准差，使得范围在 [-spreadAngle/2, spreadAngle/2] 内
        
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // 使用 Box-Muller 变换生成高斯分布角度
            float u1 = Random.value;
            float u2 = Random.value;
            float z0 = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
            float angle = z0 * sigma;
        
            // 限制角度在 [-spreadAngle/2, spreadAngle/2] 范围内
            angle = Mathf.Clamp(angle, -spreadAngle / 2, spreadAngle / 2);
        
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up); // 假设上方为旋转轴
            Vector3 shotDirection = rotation * dir;
        
            // 发射子弹
            SetAmmunition(character, weaponConfigData, ammunitionType, startPoint, shotDirection);
        }
    }
    
    private void SetAmmunition(GameObject character, WeaponConfigData weaponConfigData, AmmunitionType ammunitionType,
        Vector3 startPoint, Vector3 dir)
    {
        var ammunitionConfig = m_AmmunitionConfigDic[ammunitionType];
        GameObject ammunition = GetAmmunitionFromPool(ammunitionType, startPoint, dir);
        m_AmmunitionFactory.ShootAmmunition(character, ammunition, ammunitionType, ammunitionConfig,
            weaponConfigData.atkType, startPoint, dir);
    }


    private void Awake()
    {
        foreach (var weaponConfigSetting in WeaponConfigSettings)
        {

            m_WeaponConfigDic.Add(weaponConfigSetting.WeaponType, weaponConfigSetting.WeaponConfig);
        }

        foreach (var ammunitionConfigSetting in AmmunitionConfigSettings)
        {
            m_AmmunitionConfigDic.Add(ammunitionConfigSetting.AmmunitionType, ammunitionConfigSetting.AmmunitionConfig);
        }

        m_AmmunitionFactory.Init(m_AmmunitionConfigDic,
            (ammunitionType, ammunition) => { m_AmmunitionPool.ReleaseObject(ammunitionType, ammunition); });
    }

    /// <summary>
    /// 通知服务器开始游戏
    /// </summary>
    public void CmdStartGame()
    {
        if (!StartGame)
        {
            foreach (var weaponSpawnSetting in WeaponSpawnSettings)
            {
                var weaponConfig = m_WeaponConfigDic[weaponSpawnSetting.WeaponType];
                var prefab = weaponConfig.prefab;

                GameObject weapon = Instantiate(prefab, weaponSpawnSetting.Position, UnityEngine.Quaternion.identity);

                // 测试武器挂载脚本
                weapon.GetComponent<WeaponInstance>().Init(weaponConfig);

                m_WeaponToConfigDic.Add(weapon, weaponConfig);
                NetworkServer.Spawn(weapon);
            }

            StartGame = true;
        }
    }

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化武器、弹药配置
    /// </summary>
    public void Init()
    {
        Instance = this;
        foreach (var ammunitionTypeToConfig in m_AmmunitionConfigDic)
        {
            m_AmmunitionPool.AddPool(ammunitionTypeToConfig.Key,
                new ObjectCategory()
                {
                    prefab = ammunitionTypeToConfig.Value.prefab,
                    defaultSize = ammunitionTypeToConfig.Value.minPoolSize,
                    maxSize = ammunitionTypeToConfig.Value.minPoolSize
                });
        }
    }


    /// <summary>
    /// 更新子弹飞行
    /// </summary>
    public void FixedUpdate()
    {
        m_AmmunitionFactory.FixedUpdate();
    }

    /// <summary>
    /// 从子弹对象池取出子弹
    /// </summary>
    /// <param name="ammunitionType"></param>
    /// <param name="startPoint"></param>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    private GameObject GetAmmunitionFromPool(AmmunitionType ammunitionType, Vector3 startPoint,
        Vector3 dir)
    {
        dir = dir.normalized;
        var quaternion = Quaternion.LookRotation(dir);
        return m_AmmunitionPool.GetObject(ammunitionType, startPoint, quaternion);
    }
}

[System.Serializable]
public struct WeaponCat
{
    public WeaponType weaponType;

    [FormerlySerializedAs("weaponSystemConfig")]
    public WeaponConfig weaponConfig;
}

[System.Serializable]
public struct AmmunitionCat
{
    public AmmunitionType ammunitionType;
    public AmmunitionConfig ammunitionConfig;
}