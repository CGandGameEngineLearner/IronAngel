using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
public struct WeaponList
{
    public List<WeaponSpawnSetting> weaponList;
}

public class BasementWeaponList : NetworkBehaviour
{
    public List<WeaponList> weaponList;

    private void Start()
    {
        StartCoroutine(DelayedCmdSpawnWeapon(2f));
    }

    IEnumerator DelayedCmdSpawnWeapon(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (NetworkClient.isConnected && NetworkClient.ready)
        {
            if (isClient)
            {
                SaveLoadManager.LoadGame();
                int level = SaveLoadManager.GlobalSaveFile.currentLevel;
                level = level >= weaponList.Count - 1 ? weaponList.Count - 1 : level;

                for (int i = 0; i <= level; i++)
                {
                    foreach (var weaponSpawnSetting in weaponList[i].weaponList)
                    {
                        PlayerController.PlayerControllers[0].CmdSpawnWeapon(weaponSpawnSetting.WeaponType, weaponSpawnSetting.Position);
                    }
                }
            
                gameObject.SetActive(false);
            }
        }
    }
}