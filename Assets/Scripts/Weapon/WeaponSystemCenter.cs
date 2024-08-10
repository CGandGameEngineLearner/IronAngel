using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
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


    public void SpawnWeapon(WeaponType weaponType, Vector3 pos)
    {
        ServeSpawnWeapon(weaponType, pos);
    }

    [ServerCallback] ///改成ServerCallback看看
    private void ServeSpawnWeapon(WeaponType weaponType, Vector3 pos)
    {
        var weaponConfig = m_WeaponConfigDic[weaponType];
        var prefab = weaponConfig.prefab;

        GameObject weapon = Instantiate(prefab, pos,
            UnityEngine.Quaternion.Euler(0,0,Random.Range(0,360))); // 朝向随机

        weapon.GetComponent<WeaponInstance>().Init(weaponConfig);
        m_WeaponToConfigDic[weapon] = weaponConfig;
        m_WeaponToTypeDic[weapon] = weaponType;
        NetworkServer.Spawn(weapon);
        RpcWeaponDicUpdate(weapon, weaponType);
    }


    [ClientRpc]
    private void RpcWeaponDicUpdate(GameObject weapon, WeaponType weaponType)
    {
        m_WeaponToConfigDic[weapon] = m_WeaponConfigDic[weaponType];
        m_WeaponToTypeDic[weapon] = weaponType;
    }

    [ServerCallback]
    private GameObject AISpawnWeapon(WeaponType weaponType, Vector3 pos)
    {
        if (weaponType == WeaponType.None) return null;
        
        var weaponConfig = m_WeaponConfigDic[weaponType];
        var prefab = weaponConfig.prefab;

        GameObject weapon = Instantiate(prefab, pos, Quaternion.identity);

        weapon.GetComponent<WeaponInstance>().Init(weaponConfig);
        m_WeaponToConfigDic[weapon] = weaponConfig;
        m_WeaponToTypeDic[weapon] = weaponType;
        NetworkServer.Spawn(weapon);
        RpcWeaponDicUpdate(weapon, weaponType);
        return weapon;
    }

    
    
    /// <summary>
    /// 给AI装备武器
    /// </summary>
    [ServerCallback]
    private void GiveAIWeapon()
    {
        foreach (var element in m_RegisteredWeaponAI)
        {
            // 给左手装备武器
            var weaponType = element.GetRightHandWeaponType();
            var leftWeapon = AISpawnWeapon(weaponType, Vector3.zero);
            element.SetLeftHandWeapon(leftWeapon);


            // 给右手装备武器
            weaponType = element.GetRightHandWeaponType();
            var rightWeapon = AISpawnWeapon(weaponType, Vector3.zero);
            element.SetRightHandWeapon(rightWeapon);

            RpcGiveAIWeapon(element, leftWeapon, rightWeapon);
        }
    }

    [ServerCallback]
    public void GiveAIWeapon(GameObject enemy)
    {
        AIController aiController = enemy.GetComponent<AIController>();
        
        // 给左手装备武器
        var weaponType = aiController.GetRightHandWeaponType();
        var leftWeapon = AISpawnWeapon(weaponType, Vector3.zero);
        aiController.SetLeftHandWeapon(leftWeapon);
        
        // 给右手装备武器
        weaponType = aiController.GetRightHandWeaponType();
        var rightWeapon = AISpawnWeapon(weaponType, Vector3.zero);
        aiController.SetRightHandWeapon(rightWeapon);

        RpcGiveAIWeapon(aiController, leftWeapon, rightWeapon);
    }

    [ClientRpc]
    private void RpcGiveAIWeapon(AIController aiController, GameObject leftHandWeapon, GameObject rightHandWeapon)
    {
        aiController.SetLeftHandWeapon(leftHandWeapon);
        aiController.SetRightHandWeapon(rightHandWeapon);
        Debug.LogWarning("Give Weapon");
    }


    /// <summary>
    /// 获取AmmunitionFactory单例
    /// </summary>
    /// <returns></returns>
    public static AmmunitionFactory GetAmmunitionFactory()
    {
        return m_AmmunitionFactory;
    }

    /// <summary>
    /// 获取武器配置
    /// </summary>
    /// <param name="weaponType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static WeaponConfig GetWeaponConfig(WeaponType weaponType)
    {
        if (!m_WeaponConfigDic.ContainsKey(weaponType))
        {
            throw new Exception("查询不到武器配置，武器类型枚举为：" + weaponType);
        }

        return m_WeaponConfigDic[weaponType];
    }


    /// <summary>
    /// 获取武器配置
    /// </summary>
    /// <param name="weaponType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static WeaponConfig GetWeaponConfig(GameObject weapon)
    {
        if (!m_WeaponToTypeDic.ContainsKey(weapon))
        {
            throw new Exception("查询不到武器配置，武器为：" + weapon);
        }

        var weaponType = m_WeaponToTypeDic[weapon];

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
        RPCFire(character, weapon, startPoint, dir);
    }

    /// <summary>
    /// 客户端开火，要同步特效和动画
    /// </summary>
    /// <param name="character">开火者</param>
    /// <param name="weapon">开火武器</param>
    /// <param name="startPoint">起始点</param>
    /// <param name="dir">方向</param>
    [ClientRpc]
    public void RPCFire(GameObject character, GameObject weapon, Vector3 startPoint, Vector3 dir)
    {
        WeaponType weaponType = m_WeaponToTypeDic[weapon];
        WeaponConfig weaponConfig = m_WeaponConfigDic[weaponType];

        Fire(character, weaponType, weaponConfig.ammunitionType, startPoint, dir);

        // 调用客户端的开火特效
        if (!weapon.TryGetComponent<WeaponInstance>(out WeaponInstance weaponInstance)) return;
        weaponInstance.FireVfxAndAnimation(character, weaponType, weaponConfig, startPoint, dir);
    }


    /// <summary>
    /// 取消开火时通知客户端，用于关闭特效、关闭动画
    /// </summary>
    [ClientRpc]
    public void RpcUnFire(GameObject weapon)
    {
        if (!weapon.TryGetComponent<WeaponInstance>(out WeaponInstance weaponInstance)) return;
        weaponInstance.UnFireVfxAndAnimation();
    }

    /// <summary>
    /// 只有客户端才会调用的表现层
    /// </summary>
    [ClientCallback]
    public void StartLaserPointer(GameObject launchCharacter, GameObject weaponGo, Vector2 startPoint, Vector2 dir)
    {
        WeaponInstance weapon = weaponGo.GetComponent<WeaponInstance>();
        LineRenderer lineRenderer = weapon.lineRenderer;
        HashSet<GameObject> ignoredObjects = launchCharacter.GetComponent<AutoGetChild>().ignoredObjects;
        WeaponConfig weaponConfig = m_WeaponToConfigDic[weaponGo];

        // 设置镭射属性
        lineRenderer.startWidth = weaponConfig.LaserPointerWidth;
        lineRenderer.endWidth = weaponConfig.LaserPointerWidth;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;

        // 最大射线距离为子弹最远距离
        AmmunitionType ammunitionType = weaponConfig.ammunitionType;
        AmmunitionConfig ammunitionConfig = m_AmmunitionFactory.GetAmmunitionConfig(ammunitionType);
        int ignoreLayer = ~(LayerMask.GetMask("Bullet") | LayerMask.GetMask("Ground") | LayerMask.GetMask("Sensor"));

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

        StartCoroutine(DisableLineRenderer(m_WeaponToConfigDic[weaponGo].attackPreCastDelay, lineRenderer));
    }

    private IEnumerator DisableLineRenderer(float seconds, LineRenderer lineRenderer)
    {
        float counter = 0;
        while (counter < seconds)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        lineRenderer.positionCount = 0;
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
            m_AmmunitionPool, isClient);
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

        m_AmmunitionFactory.Clear();
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