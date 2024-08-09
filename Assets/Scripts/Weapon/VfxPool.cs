using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 只有客户端才会用到
/// </summary>
public class VfxPool : MonoBehaviour
{
    [Serializable]
    public struct VfxPair
    {
        public VfxType vfxType;
        public ObjectCategory vfxCategory;
    }

    
    public static VfxPool Instance { get; private set; }

    [SerializeField]private List<VfxPair> m_VfxPairs = new();
    private ObjectPoolManager<VfxType> m_VfxObjectPoolManager = new ObjectPoolManager<VfxType>();

    public void Awake()
    {
        Instance = this;
        
        // 添加对象池
        foreach (var fireVfxPair in m_VfxPairs)
        {
            m_VfxObjectPoolManager.AddPool(fireVfxPair.vfxType, fireVfxPair.vfxCategory);
        }
    }

    public void ReleaseVfx(VfxType vfxType, GameObject vfx)
    {
        m_VfxObjectPoolManager.ReleaseObject(vfxType, vfx);
    }

    public GameObject GetVfx(VfxType vfxType, Vector2 position, Quaternion quaternion)
    {
        return m_VfxObjectPoolManager.GetObject(vfxType, position, quaternion);
    }
}