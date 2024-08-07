using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BattleZoneTrigger : NetworkBehaviour
{
    public EnemyWaveConfig enemyWaveConfig;
    private LevelManager m_LevelManager;
    
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        // 提交到LevelManager
        
    }
}