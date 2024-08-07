using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Enemy Wave Config", menuName = "ScriptableObjects/EnemyWaveConfig", order = 1)]
public class EnemyWaveConfig : ScriptableObject
{
    public List<EnemyListItemConfig> enemyList;
    
    [Tooltip("场上同时维持多少人")]
    public int fieldEnemyCount;
    
    [Tooltip("下一波延迟多久才开始,这是一波正常结束后会触发的")]
    public float delayTime;
    
    [Tooltip("这一波开始后，下一波过多久会强制开始")]
    public float pushTime;
}