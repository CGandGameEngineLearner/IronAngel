using System;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "gamesave.json");

    public static GameSaveFile GlobalSaveFile;

// 保存游戏
    public static void SaveGame(GameSaveFile saveFile)
    {
        try
        {
            string json = JsonUtility.ToJson(saveFile, true);
            File.WriteAllText(saveFilePath, json);

#if UNITY_EDITOR
            Debug.Log("Game saved successfully.");
#endif
            GlobalSaveFile = saveFile;
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError("Failed to save game: " + ex.Message);
#endif
        }
    }

    // 加载游戏
    public static void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                GameSaveFile saveFile = JsonUtility.FromJson<GameSaveFile>(json);

#if UNITY_EDITOR
                Debug.Log("Game loaded successfully." + saveFilePath);
#endif

                GlobalSaveFile = saveFile;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError("Failed to load game: " + ex.Message);
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("Save file not found.");
#endif
            // 创建新文件
            GameSaveFile gameSaveFile = new GameSaveFile();
            gameSaveFile.currentLevel = 0;
            gameSaveFile.currentSection = 0;
            gameSaveFile.leftWeaponType = WeaponType.None;
            gameSaveFile.rightWeaponType = WeaponType.None;
            
            SaveGame(gameSaveFile);
        }
    }

    // 检查是否存在存档文件
    public static bool SaveFileExists()
    {
        return File.Exists(saveFilePath);
    }

    // 删除存档文件
    public static void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);

#if UNITY_EDITOR
            Debug.Log("Save file deleted.");
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("No save file to delete.");
#endif
        }
    }
}