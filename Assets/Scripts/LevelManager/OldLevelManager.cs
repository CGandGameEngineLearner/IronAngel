// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Mirror;
//
// /// <summary>
// /// 当前正在执行的小波次实例
// /// </summary>
// public class WaveInstance
// {
//     public Action<WaveInstance> onWaveFinished;
//
//     // 是第几波
//     public int waveIdx = -1;
//
//     // 已经经过时间
//     public float currentTime = 0;
//
//     public int CurrentEnemyCount => enemySet.Count;
//     
//     // 当前场上的敌人数量
//     private HashSet<GameObject> enemySet = new HashSet<GameObject>();
//
//     // 当前波的配置
//     public WaveListItem waveListItem;
//     private Queue<EnemyListItemConfig> m_EnemyConfigs = new Queue<EnemyListItemConfig>();
//
//     public WaveInstance(Action<WaveInstance> onWaveFinished, int waveIdx, WaveListItem waveListItem)
//     {
//         this.onWaveFinished = onWaveFinished;
//         this.waveIdx = waveIdx;
//         this.waveListItem = waveListItem;
//     }
//
//     /// <summary>
//     /// 手动启动波次，同时监听死亡
//     /// </summary>
//     public void Init()
//     {
//         // 监听敌人死亡
//         EventCenter.AddListener<GameObject>(EventType.CharacterDied, OnEnemyDeath);
//     }
//     
//     /// <summary>
//     /// 自动生成敌人，维持场上敌人数量
//     /// </summary>
//     public void GenerateEnemy()
//     {
//         // 生成新的敌人
//         while (m_EnemyConfigs.Count > 0 && CurrentEnemyCount < waveListItem.onFieldEnemyCount)
//         {
//             EnemyListItemConfig enemyConfig = m_EnemyConfigs.Dequeue();
//                 
//             // 服务端生成
//             GameObject enemy = GameObject.Instantiate(enemyConfig.enemyPrefab, enemyConfig.spawnPosition, Quaternion.identity);
//             
//             // 客户端生成
//             NetworkServer.Spawn(enemy);
//         }
//     }
//     
//     /// <summary>
//     /// 敌人死亡时的回调函数
//     /// </summary>
//     /// <param name="gameObject"></param>
//     private void OnEnemyDeath(GameObject gameObject)
//     {
//         if(!enemySet.Contains(gameObject)) return;
//         
//         enemySet.Remove(gameObject);
//         if (CurrentEnemyCount <= waveListItem.onFieldEnemyCount)
//         {
//             // 生成新的敌人
//             GenerateEnemy();
//         }
//         else if (CurrentEnemyCount <= 0)
//         {
//             Clear();
//             // 通知外部推进波次
//             onWaveFinished?.Invoke(this);
//         }
//     }
//
//     private void Clear()
//     {
//         // 取消监听敌人死亡事件
//         EventCenter.RemoveListener<GameObject>(EventType.CharacterDied, OnEnemyDeath);
//     }
// }
//
// /// <summary>
// /// 区块波次实例
// /// </summary>
// public class BattleZoneWaveHandle
// {
//     public int CurrentWaveIdx => m_CurrentWaveIdx;
//     public bool finished => waveQueue.Count > 0; 
//
//     // 用于触发波次
//     public Queue<WaveInstance> waveQueue;
//
//     // 配置表
//     private WaveConfig m_WaveConfig;
//     
//     // 记录已经触发的波次
//     private int m_CurrentWaveIdx = -1;
//
//     private HashSet<GameObject> enemySet = new HashSet<GameObject>();
//     
//     public BattleZoneWaveHandle(WaveConfig waveConfig, Action<WaveInstance> onWaveFinished)
//     {
//         m_WaveConfig = waveConfig;
//
//         // 转换为队列
//         waveQueue = new Queue<WaveInstance>();
//         for (int i = 0; i < waveConfig.waveConfigs.Count; i++)
//         {
//             waveQueue.Enqueue(new WaveInstance(onWaveFinished, i, waveConfig.waveConfigs[i]));
//         }
//     }
//
//     /// <summary>
//     /// 推进波次，注意参数是当前波次，所以第一次调用需要给-1
//     /// </summary>
//     /// <param name="requestIdx">发起请求的波次号，如果是timeout的波次，那就不会执行</param>
//     /// <returns>下一波的实例</returns>
//     public WaveInstance GetNextWave(int requestIdx)
//     {
//         if (requestIdx != m_CurrentWaveIdx || waveQueue.Count == 0) return null;
//         
//         m_CurrentWaveIdx++;
//         return waveQueue.Dequeue();
//     }
// }
//
// public class LevelManager : NetworkBehaviour
// {
//     private float m_Counter;
//     private bool m_IsRunning = false;
//     
//     // 同一时间只会用同一个战斗波次
//     private BattleZoneWaveHandle m_BattleZoneWaveHandle;
//
//     // 当前正在执行的波次
//     private HashSet<WaveInstance> m_WaveInstancesToUpdate = new HashSet<WaveInstance>();
//
//     // 延迟加入波次
//     private Queue<WaveInstance> m_WaveInstancesToAdd = new Queue<WaveInstance>();
//
//     /// <summary>
//     /// 启动一个区域
//     /// </summary>
//     /// <param name="enemyWaveConfig"></param>
//     public void StartBattleZoneWave(WaveConfig enemyWaveConfig)
//     {
//         m_BattleZoneWaveHandle = new BattleZoneWaveHandle(enemyWaveConfig, OnWaveFinished);
//         
//         // 同时启动空气墙
//         
//         m_IsRunning = true;
//         m_WaveInstancesToAdd.Clear();
//         m_WaveInstancesToUpdate.Clear();
//         
//         AddWaveInstance(m_BattleZoneWaveHandle.GetNextWave(-1));
//     }
//
//     private void Update()
//     {
//         if (!m_IsRunning) return;
//         
//         m_Counter += Time.deltaTime;
//
//         // 更新波次时间
//         foreach (var waveInstance in m_WaveInstancesToUpdate)
//         {
//             waveInstance.currentTime += Time.deltaTime;
//
//             
//             // TODO:如果超时就会直接召唤下一波
//             // if (waveInstance.currentTime > waveInstance.waveListItem.timeOut)
//             // {
//             //     WaveInstance newWaveInstance = m_BattleZoneWaveHandle.GetNextWave(m_BattleZoneWaveHandle.CurrentWaveIdx);
//             //     if (newWaveInstance != null)
//             //     {
//             //         m_WaveInstancesToAdd.Enqueue(newWaveInstance);
//             //     }
//             // }
//         }
//
//         // 延迟添加
//         while (m_WaveInstancesToAdd.Count > 0)
//         {
//             m_WaveInstancesToUpdate.Add(m_WaveInstancesToAdd.Dequeue());
//         }
//     }
//
//     /// <summary>
//     /// 当一小波结束时触发
//     /// </summary>
//     /// <param name="waveInstance"></param>
//     private void OnWaveFinished(WaveInstance waveInstance)
//     {
//         // 移除wave
//         m_WaveInstancesToUpdate.Remove(waveInstance);
//
//         // 检查是否还有wave
//         if (m_BattleZoneWaveHandle.finished)
//         {
//             StartCoroutine(WaitingCoroutine(waveInstance));    
//         }
//         else
//         {
//             // 如果没有波次了，说明全部波次完毕
//             m_IsRunning = false;
//             m_BattleZoneWaveHandle = null;
//             
//             // TODO:解锁关卡空气墙
//         }
//     }
//
//     /// <summary>
//     /// 添加新的小波次
//     /// </summary>
//     /// <param name="waveInstance"></param>
//     private void AddWaveInstance(WaveInstance waveInstance)
//     {
//         if(waveInstance == null) return;
//         waveInstance.Init();
//         m_WaveInstancesToAdd.Enqueue(waveInstance);
//     }
//     
//
//     /// <summary>
//     /// 每一波结束后都会调用
//     /// </summary>
//     /// <param name="waveInstance"></param>
//     /// <returns></returns>
//     private IEnumerator WaitingCoroutine(WaveInstance waveInstance)
//     {
//         float counter = 0;
//
//         // 如果不是正常结束的波次都不会启动下一波
//         WaveInstance newWaveInstance = m_BattleZoneWaveHandle.GetNextWave(waveInstance.waveIdx);
//         if (newWaveInstance == null) yield return null;
//         else
//         {
//             while (counter < waveInstance.waveListItem.waveDelay)
//             {
//                 counter += Time.deltaTime;
//                 yield return null;
//             }
//
//             AddWaveInstance(newWaveInstance);
//         }
//     }
// }