using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LevelManager : NetworkBehaviour
{
    [Tooltip("用于配置每一波的敌人")]
    public List<EnemyListItemConfig> enemyListItemConfigs;

    public void SwitchConfig()
    {
        
    }
}