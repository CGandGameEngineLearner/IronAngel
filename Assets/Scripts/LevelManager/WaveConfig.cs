using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level/WaveConfig", order = 1)]
public class WaveConfig : ScriptableObject
{
    [Tooltip("波次配置")]
    public List<WaveListItem> waveConfigs;
}