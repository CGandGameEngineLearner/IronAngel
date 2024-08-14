using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level/LevelChooseConfig", order = 1)]
public class LevelChooseConfig : ScriptableObject
{
    [SerializeField]
    public List<LevelOptionSetting> LevelOptionSettings = new List<LevelOptionSetting>();

    private static LevelOptionSetting DefaultLogicStateSetting = new LevelOptionSetting();


    public LevelChooseConfig()
    {
        Init();
    }

    void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        foreach(var levelOptionSetting in LevelOptionSettings)
        {
            m_LevelOptionSettingDic[levelOptionSetting.LevelSceneName]=levelOptionSetting;
        }
    }

    private Dictionary<string, LevelOptionSetting> m_LevelOptionSettingDic  = new Dictionary<string, LevelOptionSetting>();
}
