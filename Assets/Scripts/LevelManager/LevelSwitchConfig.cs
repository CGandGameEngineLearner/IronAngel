using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct SectionStruct
{
    public string levelName;
    public List<string> sectionName;
}

[System.Serializable]
public struct LevelWeaponProcessList
{
    public List<WeaponType> weaponTypes;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level/LevelSwitchConfig", order = 1)]
public class LevelSwitchConfig : ScriptableObject
{
    public string basementName;
    [Tooltip("配置关卡")]
    public List<SectionStruct> levelStruct;
}