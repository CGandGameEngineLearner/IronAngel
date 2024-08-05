using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AmmunitionHandle
{
    public bool active;
    public GameObject ammunition;
    public GameObject launcherCharacter; // 发射这个子弹的角色GameObject
    public Rigidbody2D rigidbody2D;
    public Vector2 startPoint;
    public Vector2 dir;
    public AtkType atkType;
    public AmmunitionType ammunitionType;
    public AmmunitionConfig ammunitionConfig;
    public int liveFrameCount;

    public void Init(GameObject owner, GameObject ammunition, AmmunitionType ammunitionType,
        AmmunitionConfig ammunitionConfig, AtkType atkType,
        Vector2 startPoint, Vector2 dir)
    {
        active = true;
        liveFrameCount = 0;
        this.launcherCharacter = owner;
        this.ammunitionType = ammunitionType;
        this.ammunition = ammunition;
        this.atkType = atkType;
        this.rigidbody2D = ammunition.GetComponent<Rigidbody2D>();
        this.ammunitionConfig = ammunitionConfig;
        this.startPoint = startPoint;
        this.dir = dir;

        ammunition.transform.position = startPoint;
    }
    
    public void Clear()
    {
        active = false;

        ammunition = null;
        rigidbody2D = null;
        ammunitionConfig = null;
        startPoint = Vector2.zero;
        dir = Vector2.up;
        this.ammunitionType = AmmunitionType.Bullet;
        liveFrameCount = 0;
    }
}


/// <summary>
/// 更新子弹和路径
/// </summary>
public class AmmunitionFactory
{
    public delegate void RecycleHandle(AmmunitionType ammunitionType, GameObject ammunition);

    private ObjectPoolManager<AmmunitionType> m_AmmunitionPool;

    // Data table
    private readonly Dictionary<AmmunitionType, AmmunitionConfig> m_AmmunitionConfigs = new();

    // HandleReuse--------------------------
    private Queue<AmmunitionHandle> m_UsableQueue = new();

    // AmmunitionStore----------------------
    private Queue<AmmunitionHandle> m_HandlesToAdd = new();
    private Dictionary<GameObject, AmmunitionHandle> m_AmmunitionsDict = new();

    private Queue<AmmunitionHandle>[] m_AmmunitionQueueSwapChain =
        new[] { new Queue<AmmunitionHandle>(), new Queue<AmmunitionHandle>() };

    private int m_ChainIdx = 0;

    // recycle------------------------------
    private RecycleHandle onRecycle;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="AmmunitionConfigDic">子弹配置映射</param> 
    /// <param name="recycleHandle">子弹回收处理委托</param> 
    public void Init(Dictionary<AmmunitionType, AmmunitionConfig> AmmunitionConfigDic,
        RecycleHandle recycleHandle, ObjectPoolManager<AmmunitionType> ammunitionPool)
    {
        onRecycle = recycleHandle;

        foreach (var ammunitionConfigPair in AmmunitionConfigDic)
        {
            m_AmmunitionConfigs[ammunitionConfigPair.Key] = ammunitionConfigPair.Value;
        }

        m_AmmunitionPool = ammunitionPool;
    }


    public AmmunitionConfig GetAmmunitionConfig(AmmunitionType ammunitionType)
    {
        if (!m_AmmunitionConfigs.ContainsKey(ammunitionType))
            throw new Exception("This ammunitionType has not been register to ConfigsTable!");

        return m_AmmunitionConfigs[ammunitionType];
    }

    /// <summary>
    /// 获取子弹对象的对应的Handle
    /// </summary>
    /// <param name="ammunition"></param>
    /// <returns></returns>
    public AmmunitionHandle GetAmmunitionHandle(GameObject ammunition)
    {
        if (!m_AmmunitionsDict.ContainsKey(ammunition))
        {
            return null;
        }

        return m_AmmunitionsDict[ammunition];
    }

    public bool GetAmmunitionHandleActive(GameObject ammunition)
    {
        return m_AmmunitionsDict[ammunition].active;
    }

    private void RegisterAmmunition(GameObject owner, GameObject ammunition, AmmunitionType ammunitionType,
        AmmunitionConfig ammunitionConfig, AtkType atkType,
        Vector2 startPoint, Vector2 dir)
    {
        if (m_AmmunitionsDict.ContainsKey(ammunition)) return;

        m_HandlesToAdd.Enqueue(InternalGetAmmunitionHandle(owner, ammunition, ammunitionType, ammunitionConfig, atkType,
            startPoint, dir));
    }

    /// <summary>
    /// 射子弹
    /// </summary>
    /// <param name="ammunition"></param>
    /// <param name="ammunitionType"></param>
    /// <param name="ammunitionConfig"></param>
    /// <param name="atkType"></param>
    /// <param name="startPoint"></param>
    /// <param name="dir"></param>
    public void ShootAmmunition(GameObject owner, GameObject ammunition, AmmunitionType ammunitionType,
        AmmunitionConfig ammunitionConfig, AtkType atkType,
        Vector2 startPoint, Vector2 dir)
    {
        ammunition.transform.rotation = Quaternion.identity;
        RegisterAmmunition(owner, ammunition, ammunitionType, ammunitionConfig, atkType, startPoint, dir);
    }


    /// <summary>
    /// 注销子弹，并处理子弹后处理效果的逻辑(爆炸等)
    /// </summary>
    /// <param name="ammunition"></param>
    public void UnRegisterAmmunition(GameObject ammunition)
    {
        if (!m_AmmunitionsDict.ContainsKey(ammunition)) return;

        // 仅产生一次后处理效果
        if (m_AmmunitionsDict[ammunition].active)
        {
            m_AmmunitionsDict[ammunition].active = false;
            
            var handle = GetAmmunitionHandle(ammunition);
            var postType = handle.ammunitionConfig.postAmmunitionType;
            if (postType != AmmunitionType.None)
            {
                // m_AmmunitionPool.GetObject(postType, handle.rigidbody2D.transform.position, Quaternion.identity);
                ShootAmmunition(handle.launcherCharacter, m_AmmunitionPool.GetObject(postType), postType, m_AmmunitionConfigs[postType],
                    AtkType.Default, handle.rigidbody2D.transform.position, Vector2.zero);
            }
        }
    }

    [ClientRpc]
    private void RpcShootAmmunition(GameObject owner, GameObject ammunition, AmmunitionType ammunitionType,
        AmmunitionConfig ammunitionConfig, AtkType atkType,
        Vector2 startPoint, Vector2 dir)
    {
        ShootAmmunition(owner, ammunition, ammunitionType, ammunitionConfig, atkType, startPoint, dir);
    }

    public void FixedUpdate()
    {
        // 交换链
        var ammunitionQueueToUpdate = m_AmmunitionQueueSwapChain[m_ChainIdx];
        m_ChainIdx = (m_ChainIdx + 1) % 2;
        var ammunitionQueueToAddNextFrame = m_AmmunitionQueueSwapChain[m_ChainIdx];


        // 添加
        while (m_HandlesToAdd.Count > 0)
        {
            AmmunitionHandle ammunitionHandle = m_HandlesToAdd.Dequeue();
            if (m_AmmunitionsDict.ContainsKey(ammunitionHandle.ammunition)) continue;

            m_AmmunitionsDict.Add(ammunitionHandle.ammunition, ammunitionHandle);
            ammunitionQueueToUpdate.Enqueue(ammunitionHandle);
        }

        // lastChainIdx是上一帧的子弹，更行完后加入到这一帧的m_ChainIdx
        while (ammunitionQueueToUpdate.Count > 0)
        {
            // 如果handle激活态,就进行更新，如果非激活态，就删除
            var ammunitionHandle = ammunitionQueueToUpdate.Dequeue();
            if (ammunitionHandle.active)
            {
                // update
                switch (ammunitionHandle.atkType)
                {
                    case AtkType.Default:
                        InternalProcessDefaultAmmunition(ammunitionHandle);
                        break;
                    case AtkType.Rifle:
                        InternalProcessShotGunAmmunition(ammunitionHandle);
                        break;
                    case AtkType.Laser:
                        break;
                    case AtkType.ShotGun:
                        InternalProcessShotGunAmmunition(ammunitionHandle);
                        break;
                }

                if (ammunitionHandle.active == false)
                {
                    InternalRecycleAmmunitionHandle(ammunitionHandle);
                }
                else
                {
                    ammunitionQueueToAddNextFrame.Enqueue(ammunitionHandle);
                }
            }
            else
            {
                // release 
                GameObject ammunition = ammunitionHandle.ammunition;
                // get handle
                if (m_AmmunitionsDict.ContainsKey(ammunition))
                {
                    AmmunitionHandle handle = m_AmmunitionsDict[ammunition];
                    InternalRecycleAmmunitionHandle(handle);
                }
            }
        }
    }


    private AmmunitionHandle InternalGetAmmunitionHandle(GameObject owner, GameObject ammunition,
        AmmunitionType ammunitionType, AmmunitionConfig ammunitionConfig, AtkType atkType,
        Vector2 startPoint, Vector2 dir)
    {
        AmmunitionHandle handle = m_UsableQueue.Count > 0 ? m_UsableQueue.Dequeue() : new AmmunitionHandle();
        handle.Init(owner, ammunition, ammunitionType, ammunitionConfig, atkType, startPoint, dir);

        return handle;
    }

    private void InternalRecycleAmmunitionHandle(AmmunitionHandle ammunitionHandle)
    {
        m_AmmunitionsDict.Remove(ammunitionHandle.ammunition);
        onRecycle?.Invoke(ammunitionHandle.ammunitionType, ammunitionHandle.ammunition);

        ammunitionHandle.Clear();
        m_UsableQueue.Enqueue(ammunitionHandle);
    }

    //
    // Process Ammunition -----------------------------------------------------------------
    //

    /// <summary>
    /// 专门用于处理后处理弹药(后续可能加入多帧判定)
    /// </summary>
    /// <param name="ammunitionHandle"></param>
    private void InternalProcessDefaultAmmunition(AmmunitionHandle ammunitionHandle)
    {
        // 超出了存活帧就会删除
        if (++ammunitionHandle.liveFrameCount > ammunitionHandle.ammunitionConfig.m_LeastLiveFixedFrameCount)
        {
            UnRegisterAmmunition(ammunitionHandle.ammunition);
        }
    }
    
    /// <summary>
    /// 处理ShotGun、Rifle类型的子弹
    /// </summary>
    /// <param name="ammunitionHandle"></param>
    private void InternalProcessShotGunAmmunition(AmmunitionHandle ammunitionHandle)
    {
        // gameObject.GetComponent<Rigidbody2D>()
        GameObject ammunition = ammunitionHandle.ammunition;
        Rigidbody2D rigidbody2D = ammunitionHandle.rigidbody2D;
        float speed = ammunitionHandle.ammunitionConfig.speed;
        float lifeDis = ammunitionHandle.ammunitionConfig.lifeDistance;
        Vector2 dir = ammunitionHandle.dir.normalized;
        Vector2 startPoint = ammunitionHandle.startPoint;

        var position = rigidbody2D.position;
        position.x += dir.x * speed * Time.deltaTime;
        position.y += dir.y * speed * Time.deltaTime;
        rigidbody2D.MovePosition(position);

        if ((position - startPoint).magnitude > lifeDis)
        {
            UnRegisterAmmunition(ammunition);
        }
    }
}