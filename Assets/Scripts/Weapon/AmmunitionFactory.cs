using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionHandle
{
    public bool active;
    public GameObject ammunition;
    public Rigidbody2D rigidbody2D;
    public Vector2 startPoint;
    public Vector2 dir;
    public AtkType atkType;
    public AmmunitionType ammunitionType;
    public AmmunitionConfig ammunitionConfig;

    public void Init(GameObject ammunition, AmmunitionConfig ammunitionConfig, AtkType atkType,
        Vector2 startPoint, Vector2 dir)
    {
        active = true;

        this.ammunition = ammunition;
        this.atkType = atkType;
        this.rigidbody2D = ammunition.GetComponent<Rigidbody2D>();
        this.ammunitionConfig = ammunitionConfig;
        this.startPoint = startPoint;
        this.dir = dir;
    }

    public void Clear()
    {
        active = false;

        ammunition = null;
        rigidbody2D = null;
        ammunitionConfig = null;
        startPoint = Vector2.zero;
        dir = Vector2.up;
    }
}


/// <summary>
/// 更新子弹和路径
/// </summary>
public class AmmunitionFactory
{
    public delegate void RecycleHandle(AmmunitionType ammunitionType, GameObject ammunition);

    // Data table
    private readonly Dictionary<AmmunitionType, AmmunitionConfig> m_AmmunitionConfigs = new();

    // HandleReuse--------------------------
    private Queue<AmmunitionHandle> m_UsableQueue = new();

    // AmmunitionStore----------------------
    private HashSet<GameObject> m_ActiveAmmunition = new();
    private Queue<AmmunitionHandle> m_HandlesToAdd = new();
    private Queue<GameObject> m_AmmunitionToRemove = new();

    private Dictionary<GameObject, AmmunitionHandle> m_AmmunitionToUpdate = new();

    // recycle------------------------------
    private RecycleHandle onRecycle;

    public void Init(List<KeyValuePair<AmmunitionType, AmmunitionConfig>> ammunitionConfigPairs,
        RecycleHandle recycleHandle)
    {
        onRecycle = recycleHandle;

        foreach (var ammunitionConfigPair in ammunitionConfigPairs)
        {
            m_AmmunitionConfigs.Add(ammunitionConfigPair.Key, ammunitionConfigPair.Value);
        }
    }


    public AmmunitionConfig GetAmmunitionConfig(AmmunitionType ammunitionType)
    {
        if (!m_AmmunitionConfigs.ContainsKey(ammunitionType))
            throw new Exception("This ammunitionType has not been register to ConfigsTable!");

        return m_AmmunitionConfigs[ammunitionType];
    }


    public void RegisterAmmunition(GameObject ammunition, AmmunitionConfig ammunitionConfig, AtkType atkType,
        Vector2 startPoint,
        Vector2 dir)
    {
        if (m_ActiveAmmunition.Contains(ammunition)) return;

        m_HandlesToAdd.Enqueue(InternalGetAmmunitionHandle(ammunition, ammunitionConfig, atkType, startPoint, dir));
    }


    public void UnRegisterAmmunition(GameObject ammunition)
    {
        if (!m_ActiveAmmunition.Contains(ammunition)) return;

        m_AmmunitionToRemove.Enqueue(ammunition);
    }

    public void Update()
    {
        // 回收
        while (m_AmmunitionToRemove.Count > 0)
        {
            GameObject ammunition = m_AmmunitionToRemove.Dequeue();
            // get handle
            if (m_AmmunitionToUpdate.ContainsKey(ammunition))
            {
                AmmunitionHandle handle = m_AmmunitionToUpdate[ammunition];
                InternalRecycleAmmunitionHandle(handle);
            }

            m_ActiveAmmunition.Remove(ammunition);
            m_AmmunitionToUpdate.Remove(ammunition);
        }

        // 添加
        while (m_HandlesToAdd.Count > 0)
        {
            AmmunitionHandle ammunitionHandle = m_HandlesToAdd.Dequeue();
            m_AmmunitionToUpdate.Add(ammunitionHandle.ammunition, ammunitionHandle);
        }

        foreach (var ammunition in m_AmmunitionToUpdate)
        {
            if (ammunition.Value.active == false) continue;
            switch (ammunition.Value.atkType)
            {
                case AtkType.ShotGun:
                    // 执行路线
                    InternalProcessShotGunAmmunition(ammunition.Key, ammunition.Value);
                    break;
                default: break;
            }
        }
    }


    private AmmunitionHandle InternalGetAmmunitionHandle(GameObject ammunition, AmmunitionConfig ammunitionConfig,
        AtkType atkType,
        Vector2 startPoint, Vector2 dir)
    {
        AmmunitionHandle handle = m_UsableQueue.Count > 0 ? m_UsableQueue.Dequeue() : new AmmunitionHandle();

        handle.Init(ammunition, ammunitionConfig, atkType, startPoint, dir);
        m_ActiveAmmunition.Add(ammunition);

        return handle;
    }

    private void InternalRecycleAmmunitionHandle(AmmunitionHandle ammunitionHandle)
    {
        onRecycle?.Invoke(ammunitionHandle.ammunitionType, ammunitionHandle.ammunition);

        ammunitionHandle.Clear();
        m_UsableQueue.Enqueue(ammunitionHandle);
    }

    private void InternalProcessShotGunAmmunition(GameObject gameObject, AmmunitionHandle ammunitionHandle)
    {
        // gameObject.GetComponent<Rigidbody2D>()
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
            UnRegisterAmmunition(gameObject);
        }
    }
}