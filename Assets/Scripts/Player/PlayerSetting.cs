using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float _CameraMinDistance;
    [Range(0f, 0.5f)]
    public float _CameraMaxDistance;

    [Header("Player Spec")]
    public PlayerSpec _PlayerSpec;

    [Header("Fire Distance")]
    public float _FireDistance;

    [Header("Power")]
    public List<int> _PowerLimit;

    [Header("玩家冲刺尾焰")]
    public List<GameObject> _Whiff;

    [Header("能量武器音效")]
    public List<AudioClip> _AudioClips;
    [Header("武器炸了的音效")]
    public AudioClip _WeaponBroken;
}