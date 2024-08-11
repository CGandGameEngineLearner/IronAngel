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

        // 如果是关底，那就加载basement，并解锁下一关
        if (section >= levelSwitchConfig.levelStruct[level].sectionName.Count - 1)
        {
            section = -1;
            level++;
            // 如果打完了就返回主页，并重置存档
            if (level >= levelSwitchConfig.levelStruct.Count)
            {
                SaveLoadManager.SaveGame(new GameSaveFile() { currentSection = -1 });
                if (NetworkClient.localPlayer != null)
                {
                    UICanvas.Instance.BackToStartMenu();
                }

                return;
            }
        }
        // 非关底正常加载section
        else
        {
            section++;
        }

        // 如果在基地，那么下一关就加载场景
        if (SceneManager.GetActiveScene().name == levelSwitchConfig.basementName)
        {
            section = 0;
        }

        // 是basement就加载，否则加载section
        string sectionName = section == -1
            ? levelSwitchConfig.basementName
            : levelSwitchConfig.levelStruct[level].sectionName[section];

        SaveLoadManager.GlobalSaveFile.currentLevel = level;
        SaveLoadManager.GlobalSaveFile.currentSection = section;

        // 存储玩家的武器
        GameObject leftWeapon = PlayerController.PlayerControllers[0].Player
            .GetPlayerLeftHandWeapon();
        if (leftWeapon)
        {
            SaveLoadManager.GlobalSaveFile.leftWeaponType = WeaponSystemCenter.GetWeaponType(leftWeapon);
        }
        else
        {
            SaveLoadManager.GlobalSaveFile.leftWeaponType = WeaponType.None;
        }
        
        GameObject rightWeapon = PlayerController.PlayerControllers[0].Player
            .GetPlayerRightHandWeapon();
        if (rightWeapon)
        {
            SaveLoadManager.GlobalSaveFile.rightWeaponType = WeaponSystemCenter.GetWeaponType(rightWeapon);
        }
        else
        {
            SaveLoadManager.GlobalSaveFile.rightWeaponType = WeaponType.None;
        }

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