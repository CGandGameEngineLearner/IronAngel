using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/AudioConfig", order = 1)]
[Serializable]
public class AudioConfig : ScriptableObject
{
    [SerializeField]
    public AudioType _AudioType;
    [SerializeField]
    public AudioClip _AudioSource;
}

[Serializable]
public enum AudioType
{

}
