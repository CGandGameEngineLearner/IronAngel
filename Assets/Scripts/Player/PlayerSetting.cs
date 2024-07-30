using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    [Header("Camera")]
    public Camera _Camera;
    public CinemachineVirtualCamera _VirtualCamera;
    public Transform _VirtualCameraTarget;
    [Range(0f, 0.5f)]
    public float _CameraMinDistance;
    [Range(0f, 0.5f)]
    public float _CameraMaxDistance;

    [Header("Player Spec")]
    public PlayerSpec _PlayerSpec;



    [Header("AudioUtils")]
    public AudioConfig _AudioConfig;

    [Header("WeaponConfig")]
    public List<WeaponCat> _WeaponCats = new List<WeaponCat>();
    public List<AmmunitionCat> _AmmunitionCats = new();
}