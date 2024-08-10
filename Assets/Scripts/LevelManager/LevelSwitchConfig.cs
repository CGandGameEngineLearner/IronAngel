using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SectionStruct
{
    public string levelName;
    public List<string> sectionName;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level/LevelSwitchConfig", order = 1)]
public class LevelSwitchConfig : ScriptableObject
{
    public List<SectionStruct> levelName;
    
}