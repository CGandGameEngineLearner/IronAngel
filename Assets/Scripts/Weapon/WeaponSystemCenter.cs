using System;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.PlayerSettings;
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
    private static WeaponSystemCenter m_Instance;

    /// <summary>
    /// 单例
    /// </summary>
    public static WeaponSystemCenter Instance
    {
        private set { m_Instance = value; }
        get { return m_Instance; }
    }

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

    private static Dictionary<GameObject, WeaponType> m_WeaponToTypeDic = new Dictionary<GameObject, WeaponType>();


    private static ObjectPoolManager<AmmunitionType> m_AmmunitionPool = new();
    private static AmmunitionFactory m_AmmunitionFactory = new(); // 弹药工厂


    private HashSet<AIController>
        m_RegisteredWeaponAI = new HashSet<AIController>(); // 注册的，需要武器的AI，注册在这里的AI 开始游戏时会给他们发武器。 

    public void RegisterAIWeapon(AIController aiController)
    {
        m_RegisteredWeaponAI.Add(aiController);
    }
    
    [ClientCallback]
    public SpawnWeapon(WeaponType weaponType, Vector3 pos)
    {
        CmdSpawnWeapon(weaponType, pos);
    }

    [Command]
    private void CmdSpawnWeapon(WeaponType weaponType, Vector3 pos)
    {
        var weaponConfig = m_WeaponConfigDic[weaponType];
        var prefab = weaponConfig.prefab;

        GameObject weapon = Instantiate(prefab, pos,
            UnityEngine.Quaternion.identity);

        weapon.GetComponent<WeaponInstance>().Init(weaponConfig);
        m_WeaponToConfigDic[weapon] = weaponConfig;
        m_WeaponToTypeDic[weapon] = weaponType;
        NetworkServer.Spawn(weapon);
        RpcWeaponDicUpdate(weapon,weaponType);
    }



    [ClientRpc]
    private void RpcWeaponDicUpdate(GameObject weapon, WeaponType weaponType)
    {
        m_WeaponToConfigDic[weapon] = m_WeaponConfigDic[weaponType];
        m_WeaponToTypeDic[weapon] = weaponType;
    }

    /// <summary>
    /// 给AI装备武器
    /// </summary>
    private void GiveAIWeapon()
    {
        foreach (var element in m_RegisteredWeaponAI)
        {
            // 给左手装备武器
            var weaponType = element.GetRightHandWeaponType();
            var leftWeapon = SpawnWeapon(weaponType, Vector3.zero);
            element.SetLeftHandWeapon(leftWeapon);


            // 给右手装备武器
            weaponType = element.GetRightHandWeaponType();
            var rightWeapon = SpawnWeapon(weaponType, Vector3.zero);
            element.SetRightHandWeapon(rightWeapon);

            RpcGiveAIWeapon(element, leftWeapon, rightWeapon);
        }
    }

    [ClientRpc]
    private void RpcGiveAIWeapon(AIController aiController, GameObject leftHandWeapon, GameObject rightHandWeapon)
    {
        aiController.SetLeftHandWeapon(leftHandWeapon);
        aiController.SetRightHandWeapon(rightHandWeapon);
    }


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
    [ServerCallback]
    public void CmdFire(GameObject character, GameObject weapon, Vector3 startPoint, Vector3 dir)
    {
        dir = dir.normalized;
#if UNITY_EDITOR
        //Debug.Log(GetType() + "Command" + "Fire");
#endif
        var weaponConfig = m_WeaponToConfigDic[weapon];
        var ammunitionType = m_WeaponToConfigDic[weapon].ammunitionType;
        var ammunitionConfig = m_AmmunitionConfigDic[ammunitionType];

        // 测试武器脚本
        if (!weapon.TryGetComponent<WeaponInstance>(out WeaponInstance weaponInstance))
        {
#if UNITY_EDITOR
            //Debug.LogError("武器没有挂载WeaponInstance脚本");
#endif
            return;
        }

        // 武器射击间隔
        if (!weaponInstance.TryFire())
        {
#if UNITY_EDITOR
            //Debug.LogWarning("开火间隔过短");
#endif
            return;
        }

        // 减少弹匣数量
        if (!weaponInstance.DecreaseMag())
        {
#if UNITY_EDITOR
            //Debug.LogWarning("子弹数不足");
#endif
            return;
        }

        // 散布
        dir = IronAngel.Utils.ApplyScatterZ(dir, weaponConfig.spreadAngle);

        // 服务端
        Fire(character, m_WeaponToTypeDic[weapon], ammunitionType, startPoint, dir);
        // Rpc调用客户端
        RPCFire(character, m_WeaponToTypeDic[weapon], ammunitionType, startPoint, dir);
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

        Fire(character, m_WeaponToTypeDic[weapon], ammunitionType, startPoint, dir);

        RPCFire(character, m_WeaponToTypeDic[weapon], ammunitionType, startPoint, dir);
    }

    [ClientRpc]
    public void RPCFire(GameObject character, WeaponType weaponType, AmmunitionType ammunitionType,
        Vector3 startPoint, Vector3 dir)
    {
        var weaponConfigData = m_WeaponConfigDic[weaponType].ToData();

        Fire(character, weaponType, ammunitionType, startPoint, dir);
    }

    /// <summary>
    /// 统一提供给RPC和server使用
    /// </summary>
    private void Fire(GameObject character, WeaponType weaponType, AmmunitionType ammunitionType,
        Vector3 startPoint, Vector3 dir)
    {
        var weaponConfigData = m_WeaponConfigDic[weaponType].ToData();
        //Debug.LogWarning(weaponConfigData.atkType);
        switch (weaponConfigData.atkType)
        {
            case AtkType.Laser:
                SetAmmunition(character, weaponConfigData, ammunitionType, startPoint, dir);
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

            // 限制角度在 [-spreadAngle/2, spreadAngle/2] 范围内是一个安全措施
            angle = Mathf.Clamp(angle, -spreadAngle / 2, spreadAngle / 2);

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 使用Z轴作为旋转轴
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
        Instance = this;
        foreach (var weaponConfigSetting in WeaponConfigSettings)
        {
            m_WeaponConfigDic[weaponConfigSetting.WeaponType] = weaponConfigSetting.WeaponConfig;
        }

        foreach (var ammunitionConfigSetting in AmmunitionConfigSettings)
        {
            m_AmmunitionConfigDic[ammunitionConfigSetting.AmmunitionType] = ammunitionConfigSetting.AmmunitionConfig;
        }

        m_AmmunitionFactory.Init(m_AmmunitionConfigDic,
            (ammunitionType, ammunition) => { m_AmmunitionPool.ReleaseObject(ammunitionType, ammunition); },
            m_AmmunitionPool);
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
                SpawnWeapon(weaponSpawnSetting.WeaponType, weaponSpawnSetting.Position);
            }

            GiveAIWeapon();

            StartGame = true;
        }
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// 初始化武器、弹药配置
    /// </summary>
    private void Init()
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