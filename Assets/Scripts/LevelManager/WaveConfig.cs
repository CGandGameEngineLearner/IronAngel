using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level/WaveConfig", order = 1)]
public class WaveConfig:ScriptableObject
{
    public List<WaveListItem> waveConfigs;
}