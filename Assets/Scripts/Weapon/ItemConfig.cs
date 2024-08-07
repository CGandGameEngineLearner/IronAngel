using System;
using UnityEngine;

[Serializable]
public class ItemConfig : ScriptableObject
{
    public GameObject prefab => m_Prefab;
    public int minPoolSize => m_MinPoolSize;
    public int maxPoolSize => m_MaxPoolSize;
    
    [Header("对象池配置")] 
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private int m_MinPoolSize;
    [SerializeField] private int m_MaxPoolSize;
}