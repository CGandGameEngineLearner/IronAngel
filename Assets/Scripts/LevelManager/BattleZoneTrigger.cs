using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BattleZoneTrigger : NetworkBehaviour
{
    public WaveConfig enemyWaveConfig;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        // 测试，后续想让服务端RPC生成角色，但是播报应该是本地的
        // if (!isServer) return;
        
        if (!other.gameObject.CompareTag("Player")) return;
        
        // 提交到LevelManager
        LevelManager.Instance.StartBattleZoneWave(enemyWaveConfig);
        
        // 提交后就不再可激活
        this.gameObject.SetActive(false);
    }
}