using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 枚举定义对象类型
public enum ObjectType
{
    Cube,
    Sphere
}

// 用来存储每个对象池的配置信息
[System.Serializable]
public struct ObjectPoolCategory
{
    public ObjectType objType;
    public GameObject prefab;
    public int defaultSize;
    public int maxSize;
}

// 管理所有对象池的类
public class ObjectPoolManager
{
    private Dictionary<ObjectType, ObjectPool<GameObject>> m_ObjectDictionary = new();

    // 初始化所有对象池
    public void Init(List<ObjectPoolCategory> categoriesToAdd)
    {
        foreach (var category in categoriesToAdd)
        {
            m_ObjectDictionary.Add(category.objType, new ObjectPool<GameObject>(
                createFunc: () => Object.Instantiate(category.prefab), // 创建新对象的方法
                actionOnGet: obj => obj.SetActive(true), // 从池中获取对象时的操作
                actionOnRelease: obj => obj.SetActive(false), // 释放回池中时的操作
                actionOnDestroy: Object.Destroy, // 销毁对象的方法
                collectionCheck: true, // 是否检查重复放回
                defaultCapacity: category.defaultSize, // 默认容量
                maxSize: category.maxSize // 最大容量
            ));
        }
    }

    public int GetObjectCount(ObjectType objectType)
    {
        return m_ObjectDictionary[objectType].CountAll;
    }

    // 获取特定类型的对象
    public GameObject GetObject(ObjectType type)
    {
        if (m_ObjectDictionary.TryGetValue(type, out var pool))
        {
            return pool.Get();
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"No pool found for object type {type}");
#endif
            return null;
        }
    }

    // 获取特定类型的对象
    public GameObject GetObject(ObjectType type, Vector3 position, Quaternion quaternion)
    {
        if (m_ObjectDictionary.TryGetValue(type, out var pool))
        {
            GameObject obj = pool.Get();
            obj.transform.position = position;
            obj.transform.rotation = quaternion;

            return obj;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"No pool found for object type {type}");
#endif
            return null;
        }
    }


    // 获取特定类型的对象
    public GameObject GetObject(ObjectType type, Vector3 position, Vector3 rotation)
    {
        if (m_ObjectDictionary.TryGetValue(type, out var pool))
        {
            GameObject obj = pool.Get();
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(rotation);

            return obj;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"No pool found for object type {type}");
#endif
            return null;
        }
    }

    // 释放特定类型的对象
    public void ReleaseObject(ObjectType type, GameObject obj)
    {
        if (m_ObjectDictionary.TryGetValue(type, out var pool))
        {
            pool.Release(obj);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"No pool found for object type {type}");
#endif
            Object.Destroy(obj); // Fallback to destroy if no pool is found
        }
    }
}