using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveListItem
{
    [Tooltip("这一波加载的敌人")]
    public List<EnemyListItemConfig> enemyListConfigs;

    [Tooltip("场上同时有多少人")]
    public int onFieldEnemyCount;

    [Tooltip("这一波最大持续时间，超过后会强行启动下一波")]
    public float timeOut;
    
    [Tooltip("下一波多久到来")]
    public float waveDelay;
}