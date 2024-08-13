using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiePanel : MonoBehaviour
{
    [Tooltip("重开按钮")]
    [SerializeField]
    private GameObject m_Retry;

    public void OnBackToStartMenu()
    {
        if (NetworkClient.localPlayer != null||!NetworkClient.isConnected)
        {

            UICanvas.Instance.BackToStartMenu();
        }
    }

    public void OnRetryClicked()
    {
        SaveLoadManager.LoadGame();
        LevelSwitchConfig levelSwitchConfig = LevelManager.Instance.levelSwitchConfig;
        // 大关卡
        int level = SaveLoadManager.GlobalSaveFile.currentLevel;
        int section = SaveLoadManager.GlobalSaveFile.currentSection;

        string sectionName = section == -1
            ? levelSwitchConfig.basementName
            : levelSwitchConfig.levelStruct[level].sectionName[section];
        
        SceneManager.LoadScene(sectionName);
        
        if (NetworkServer.active || NetworkClient.isConnected)
        {
            NetworkManager networkManager = GameObject.FindAnyObjectByType<NetworkManager>();
            networkManager.StopClient();
            networkManager.StopHost();
            networkManager.StopServer();
        }
        
        UICanvas.Instance.DiePanel.gameObject.SetActive(false);
        UICanvas.Instance.PropertiesUI.gameObject.SetActive(true);
    }

    public void SetRetryButtonVisiable(bool visiable)
    {
        m_Retry.SetActive(visiable);
    }
}
