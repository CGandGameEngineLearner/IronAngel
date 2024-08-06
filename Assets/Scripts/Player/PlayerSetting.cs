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
}