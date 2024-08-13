using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyListItemConfig
{
    public EnemyType enemyType;
    public GameObject enemyPrefab;
    public Vector3 spawnPosition;
}

