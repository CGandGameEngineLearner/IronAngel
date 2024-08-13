using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogStruct
{
    [Tooltip("说什么")] public string stringToSays;
    [Tooltip("持续多久")] public float time;
    [Tooltip("延迟多久出现")] public float delayTime;
}

[Serializable]
public class WaveListItem
{
    [Tooltip("这一波加载的敌人")] public List<EnemyListItemConfig> enemyListConfigs;

    [Tooltip("场上同时有多少人")] public int onFieldEnemyCount;

    [Tooltip("这一波会说什么")] public List<DialogStruct> dialogStructs;

    [Tooltip("下一波多久到来")] public float waveDelay;

    [Tooltip("这一波放什么音乐")] public AudioClip thisWaveBGM;
}