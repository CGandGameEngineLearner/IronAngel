using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level/EnemySettingConfig", order = 1)]
public class EnemySettingConfig:ScriptableObject
{
    [SerializeField]
    public List<EnemySetting> EnemyTypeSettings = new List<EnemySetting>();
    
    private static EnemySetting DefaultEnemySetting = new EnemySetting();
    public EnemySetting GetLogicStateSetting(EnemyType enemyType)
    {
        if (!m_EnemyPrefabDic.ContainsKey(enemyType))
        {
#if UNITY_EDITOR
            Debug.LogWarning(enemyType+"使用了默认DefaultEnemySetting!");
#endif
            return DefaultEnemySetting;
        }
        var result = m_EnemyPrefabDic[enemyType];
        return result;
    }
        
    public EnemySetting GetEnemySetting(EnemyType enemyType)
    {
        if (!m_EnemyPrefabDic.ContainsKey(enemyType))
        {
            Debug.LogWarning("未配置敌人种类"+enemyType+"对应的设置");
            return DefaultEnemySetting;
        }
        return m_EnemyPrefabDic[enemyType];
    }

    public EnemySettingConfig()
    {
        Init();
    }

    void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        foreach(var enemyTypeSetting in EnemyTypeSettings)
        {
            m_EnemyPrefabDic[enemyTypeSetting.EnemyType]=enemyTypeSetting;
        }
    }

    private Dictionary<EnemyType,EnemySetting> m_EnemyPrefabDic  = new Dictionary<EnemyType,EnemySetting>();
}