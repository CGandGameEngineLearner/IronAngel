using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SectionChangeTrigger : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        SaveLoadManager.LoadGame();
        LevelSwitchConfig levelSwitchConfig = LevelManager.Instance.levelSwitchConfig;
        // 大关卡
        int level = SaveLoadManager.GlobalSaveFile.currentLevel;
        int section = SaveLoadManager.GlobalSaveFile.currentSection;
        
        if (section >= levelSwitchConfig.levelStruct[level].sectionName.Count)
        {
            section = -1;
            level++;
        }
        else if (section == -1)
        {
            // load basement
        }
        else
        {
            section++;
        }

        string sectionName = section == -1 ? levelSwitchConfig.basementName : levelSwitchConfig.levelStruct[level].sectionName[section];

        SaveLoadManager.GlobalSaveFile.currentLevel = level;
        SaveLoadManager.GlobalSaveFile.currentSection = section;
        
        SaveLoadManager.SaveGame(SaveLoadManager.GlobalSaveFile);
        
        SceneManager.LoadScene(sectionName);
        
        if (NetworkServer.active || NetworkClient.isConnected)
        {    
            NetworkManager networkManager = GameObject.FindAnyObjectByType<NetworkManager>();
            networkManager.StopClient();
            networkManager.StopHost();
            networkManager.StopServer();
        }
    }
}