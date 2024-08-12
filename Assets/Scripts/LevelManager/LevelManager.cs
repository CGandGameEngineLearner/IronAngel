using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class WaveInstance
{
    public Action<WaveInstance> onWaveFinished;

    public int waveIdx = -1;
    public float currentTime = 0;
    public int CurrentEnemyCount => enemySet.Count;

    private HashSet<GameObject> enemySet = new HashSet<GameObject>();
    public WaveListItem waveListItem;
    private Queue<EnemyListItemConfig> m_EnemyToSpawn = new Queue<EnemyListItemConfig>();
    private bool isServer;

    public WaveInstance(Action<WaveInstance> onWaveFinished, int waveIdx, WaveListItem waveListItem, bool isServer)
    {
        this.onWaveFinished = onWaveFinished;
        this.waveIdx = waveIdx;
        this.waveListItem = waveListItem;

        this.isServer = isServer;

        foreach (var enemy in waveListItem.enemyListConfigs)
        {
            m_EnemyToSpawn.Enqueue(enemy);
        }
    }

    public void Init()
    {
        EventCenter.AddListener<GameObject>(EventType.CharacterDied, OnEnemyDeath);
    }

    public void GenerateEnemies()
    {
        while (m_EnemyToSpawn.Count > 0 && CurrentEnemyCount < waveListItem.onFieldEnemyCount)
        {
            EnemyListItemConfig enemyConfig = m_EnemyToSpawn.Dequeue();

            GameObject enemy =
                GameObject.Instantiate(enemyConfig.enemyPrefab, enemyConfig.spawnPosition, Quaternion.identity);
            // 只有服务端调用
            if (isServer)
            {
                NetworkServer.Spawn(enemy);
            }

            // 设置仇恨
            enemy.GetComponent<DamageSensor>().PutPerceiveGameObject(PlayerController.PlayerControllers[0].gameObject);
            WeaponSystemCenter.Instance.GiveAIWeapon(enemy);
            enemySet.Add(enemy);
        }
    }

    private void OnEnemyDeath(GameObject gameObject)
    {
        if (!enemySet.Contains(gameObject)) return;

        enemySet.Remove(gameObject);
#if UNITY_EDITOR
        Debug.LogWarning($"Enemy Death,Remain{enemySet.Count}, ${m_EnemyToSpawn.Count}");
#endif
        if (CurrentEnemyCount < waveListItem.onFieldEnemyCount && m_EnemyToSpawn.Count > 0)
        {
            GenerateEnemies();
        }
        else if (CurrentEnemyCount == 0 && m_EnemyToSpawn.Count == 0)
        {
            Clear();
            onWaveFinished?.Invoke(this);
        }
    }

    private void Clear()
    {
        EventCenter.RemoveListener<GameObject>(EventType.CharacterDied, OnEnemyDeath);
    }
}

public class BattleZoneWaveHandle
{
    public int CurrentWaveIdx => m_CurrentWaveIdx;
    public bool Finished => waveQueue.Count == 0;

    private Queue<WaveInstance> waveQueue;
    private int m_CurrentWaveIdx = -1;

    public BattleZoneWaveHandle(WaveConfig waveConfig, Action<WaveInstance> onWaveFinished, bool isServer)
    {
        waveQueue = new Queue<WaveInstance>();

        for (int i = 0; i < waveConfig.waveConfigs.Count; i++)
        {
            waveQueue.Enqueue(new WaveInstance(onWaveFinished, i, waveConfig.waveConfigs[i], isServer));
        }
    }

    public WaveInstance GetNextWave()
    {
        if (waveQueue.Count == 0) return null;

        m_CurrentWaveIdx++;
        return waveQueue.Dequeue();
    }
}

public class LevelManager : NetworkBehaviour
{
    public LevelSwitchConfig levelSwitchConfig;
    public static LevelManager Instance { get; private set; }

    private bool m_IsRunning = false;
    private BattleZoneWaveHandle m_BattleZoneWaveHandle;
    private HashSet<WaveInstance> m_WaveInstancesToUpdate = new HashSet<WaveInstance>();
    private Queue<WaveInstance> m_WaveInstancesToAdd = new Queue<WaveInstance>();
    private List<GameObject> m_WaveInvisibleWall;

    public void Awake()
    {
        Instance = this;
    }

    public void StartBattleZoneWave(WaveConfig enemyWaveConfig, List<GameObject> invisibleWall)
    {
        // UICanvas.Instance.SetPlotText("Someone is coming, be careful.", 0, 2f);
        m_BattleZoneWaveHandle = new BattleZoneWaveHandle(enemyWaveConfig, OnWaveFinished, isServer);

        m_IsRunning = true;
        m_WaveInstancesToAdd.Clear();
        m_WaveInstancesToUpdate.Clear();

        AddWaveInstance(m_BattleZoneWaveHandle.GetNextWave());

        m_WaveInvisibleWall = invisibleWall;
    }

    private void Update()
    {
        if (!m_IsRunning) return;

        foreach (var waveInstance in m_WaveInstancesToUpdate)
        {
            waveInstance.currentTime += Time.deltaTime;
        }

        while (m_WaveInstancesToAdd.Count > 0)
        {
            var instance = m_WaveInstancesToAdd.Dequeue();
            if (instance != null)
            {
                instance.Init();
                instance.GenerateEnemies();
                m_WaveInstancesToUpdate.Add(instance);
            }
        }
    }

    private void AddWaveInstance(WaveInstance waveInstance)
    {
        if (waveInstance == null) return;
        m_WaveInstancesToAdd.Enqueue(waveInstance);
        StartCoroutine(ShowDialogText(waveInstance.waveListItem));
    }

    private void OnWaveFinished(WaveInstance waveInstance)
    {
        m_WaveInstancesToUpdate.Remove(waveInstance);

        if (m_BattleZoneWaveHandle.Finished)
        {
            m_IsRunning = false;

            // UICanvas.Instance.SetPlotText("They all gone, well done.", 0, 2f);
            
            // TODO: 解锁关卡空气墙
            foreach (var invisibleWall in m_WaveInvisibleWall)
            {
                invisibleWall.SetActive(false);
            }
        }
        else
        {
            StartCoroutine(WaitAndAddNextWave(waveInstance));
        }
    }

    private IEnumerator WaitAndAddNextWave(WaveInstance waveInstance)
    {
        yield return new WaitForSeconds(waveInstance.waveListItem.waveDelay);
        AddWaveInstance(m_BattleZoneWaveHandle.GetNextWave());

        // UICanvas.Instance.SetPlotText("New enemy is coming, finish them.", 0, 2f);
    }

    private IEnumerator ShowDialogText(WaveListItem waveListItem)
    {
        List<DialogStruct> dialogStructs = waveListItem.dialogStructs;
        for (int i = 0; i < dialogStructs.Count; i++)
        {
            if (i < dialogStructs.Count - 1)
            {
                UICanvas.Instance.SetPlotText(dialogStructs[i].stringToSays, dialogStructs[i].delayTime, dialogStructs[i].time);
                yield return new WaitForSeconds(dialogStructs[i].time + dialogStructs[i].delayTime);
            }
            else
            {
                UICanvas.Instance.SetPlotText(dialogStructs[i].stringToSays, dialogStructs[i].delayTime, dialogStructs[i].time, true);
            }
        }
    }
}