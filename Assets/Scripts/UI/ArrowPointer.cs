using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowPointer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Canvas uiCanvas; // 引用UI Canvas

    private ObjectPool<GameObject> arrowPoolManager;
    private Dictionary<GameObject, GameObject> enemyPointers = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        arrowPoolManager = new ObjectPool<GameObject>(() => 
        {
            // 创建新的箭头对象，并将其设置为UI Canvas的子对象
            var arrow = Instantiate(arrowPrefab, uiCanvas.transform);
            arrow.SetActive(false);
            return arrow;
        }, arrow => 
        {
            arrow.SetActive(true);
        }, arrow => 
        {
            arrow.SetActive(false);
        }, arrow => 
        {
            Destroy(arrow);
        });
        
        EventCenter.AddListener(EventType.ChangeScene, ClearEnemy);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemyPointers.ContainsKey(enemy))
        {
            var arrow = arrowPoolManager.Get();
            enemyPointers.Add(enemy, arrow);
        }
                
        EventCenter.AddListener<GameObject>(EventType.CharacterDied, OnEnemyDied);
    }

    public void OnEnemyDied(GameObject enemy)
    {
        if (enemyPointers.ContainsKey(enemy))
        {
            arrowPoolManager.Release(enemyPointers[enemy]);
            enemyPointers.Remove(enemy);
        }
    }

    private void ClearEnemy()
    {
        foreach (var enemyPair in enemyPointers)
        {
            arrowPoolManager.Release(enemyPair.Value);
        }
        
        enemyPointers.Clear();
    }

    private void Update()
    {
        foreach (var enemyPair in enemyPointers)
        {
            if (!NetworkClient.isConnected || NetworkClient.localPlayer == null) continue;
            if (!NetworkClient.localPlayer.TryGetComponent<PlayerController>(out var playerController)) return;

            Vector3 screenPos = playerController.CameraController.GetCamera().WorldToScreenPoint(enemyPair.Key.transform.position);
            RectTransform arrowUI = enemyPointers[enemyPair.Key].GetComponent<RectTransform>();

            bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;

            if (isOffScreen)
            {
                // 将屏幕坐标限制在屏幕范围内，并留出一些边距
                screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
                screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

                // 设置锚点位置使箭头在屏幕上正确显示
                Vector2 anchoredPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)uiCanvas.transform, screenPos, playerController.CameraController.GetCamera(), out anchoredPos);
                arrowUI.anchoredPosition = anchoredPos;

                // 计算箭头旋转
                Vector3 direction = enemyPair.Key.transform.position - playerController.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrowUI.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // 调整角度使箭头正确指向敌人

                arrowUI.gameObject.SetActive(true);
            }
            else
            {
                arrowUI.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // 移除事件监听器以防止内存泄漏
        EventCenter.RemoveListener<GameObject>(EventType.CharacterDied, OnEnemyDied);
    }
}