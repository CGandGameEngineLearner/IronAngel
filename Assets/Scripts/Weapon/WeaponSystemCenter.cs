using System;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class WeaponSpawnSetting
{
    [SerializeField] public WeaponType WeaponType;

    [FormerlySerializedAs("Transform")] [SerializeField] public Vector3 Position;
}

[Serializable]
public class WeaponConfigSetting
{
    [SerializeField]public WeaponType WeaponType;
    
    [SerializeField]public WeaponConfig WeaponConfig ;
}

[Serializable]
public class AmmunitionConfigSetting
{
    [SerializeField] public AmmunitionType AmmunitionType;
    [SerializeField] public AmmunitionConfig AmmunitionConfig;
}

public class WeaponSystemCenter: NetworkBehaviour
{
    public static WeaponSystemCenter Instance;

    public List<WeaponSpawnSetting> WeaponSpawnSettings = new List<WeaponSpawnSetting>();
    public List<WeaponConfigSetting> WeaponConfigSettings = new List<WeaponConfigSetting>();
    public List<AmmunitionConfigSetting> AmmunitionConfigSettings = new List<AmmunitionConfigSetting>();
    
    private Dictionary<WeaponType, WeaponConfig> m_WeaponConfigDic = new Dictionary<WeaponType, WeaponConfig>();

    private Dictionary<AmmunitionType, AmmunitionConfig> m_AmmunitionConfigDic =
        new Dictionary<AmmunitionType, AmmunitionConfig>();
    
    /// <summary>
    /// 武器GameObject到其配置的映射
    /// </summary>
    private Dictionary<GameObject, WeaponConfig> m_WeaponToConfigDic = new Dictionary<GameObject, WeaponConfig>();

    
    
    private ObjectPoolManager<AmmunitionType> m_AmmunitionPool = new();
    private AmmunitionFactory m_AmmunitionFactory = new(); // 弹药工厂

    public bool StartGame = false;
    
    
    public void Fire(GameObject weapon, Vector3 startPoint, Vector3 dir)
    {
        Debug.Log(GetType()+"Command"+"Fire");
        var weaponConfig = m_WeaponToConfigDic[weapon];
        var ammunitionType = m_WeaponToConfigDic[weapon].ammunitionType;
        var ammunitionConfig = m_AmmunitionConfigDic[ammunitionType];
        GameObject ammunition = GetAmmunitionFromPool(ammunitionType, startPoint, dir);
        m_AmmunitionFactory.ShootAmmunition(ammunition,ammunitionType,ammunitionConfig,weaponConfig.atkType,startPoint,dir);

        RPCFire(weaponConfig, ammunitionType, startPoint, dir);
    }

    [ClientRpc]
    public void RPCFire(WeaponConfig weaponConfig, AmmunitionType ammunitionType, Vector3 startPoint, Vector3 dir)
    {
        var ammunitionConfig = m_AmmunitionConfigDic[ammunitionType];
        GameObject ammunition = GetAmmunitionFromPool(ammunitionType, startPoint, dir);
        m_AmmunitionFactory.ShootAmmunition(ammunition,ammunitionType,ammunitionConfig,weaponConfig.atkType,startPoint,dir);
    }


    

    private void Awake()
    {
        foreach (var weaponConfigSetting in WeaponConfigSettings)
        {
            m_WeaponConfigDic.Add(weaponConfigSetting.WeaponType,weaponConfigSetting.WeaponConfig);
        }

        foreach (var ammunitionConfigSetting in AmmunitionConfigSettings)
        {
            m_AmmunitionConfigDic.Add(ammunitionConfigSetting.AmmunitionType,ammunitionConfigSetting.AmmunitionConfig);
        }
        m_AmmunitionFactory.Init(m_AmmunitionConfigDic,
            (ammunitionType, ammunition) => { m_AmmunitionPool.ReleaseObject(ammunitionType, ammunition); });
    }

    
    public void CmdStartGame()
    {
        
        if(!StartGame)
        {
            foreach (var weaponSpawnSetting in WeaponSpawnSettings)
            {
                var weaponConfig = m_WeaponConfigDic[weaponSpawnSetting.WeaponType];
                var prefab = weaponConfig.prefab;
           
                GameObject weapon = Instantiate(prefab,weaponSpawnSetting.Position,UnityEngine.Quaternion.identity);
                m_WeaponToConfigDic.Add(weapon,weaponConfig);
                NetworkServer.Spawn(weapon);
            }

            StartGame = true;
        }
       
    }

    public override void OnStartServer()
    {
        
        
    }

    public override void OnStartClient()
    {
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
                    prefab = ammunitionTypeToConfig.Value.prefab, defaultSize = ammunitionTypeToConfig.Value.minPoolSize,
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
    [FormerlySerializedAs("weaponSystemConfig")] public WeaponConfig weaponConfig;
}

[System.Serializable]
public struct AmmunitionCat
{
    public AmmunitionType ammunitionType;
    public AmmunitionConfig ammunitionConfig;
}