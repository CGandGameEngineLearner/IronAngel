using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    [Header("Camera")]
    public Camera _Camera;
    public CinemachineVirtualCamera _VirtualCamera;
    public Transform _VirtualCameraTarget;
    [Range(0f, 0.5f)]
    public float _CameraMinDistance;
    [Range(0f, 0.5f)]
    public float _CameraMaxDistance;

    [Header("Player")]
    public GameObject _Player;
    public float _MoveSpeed;
    public float _DashCoolDownTime;
    public int _DashCount;
    public float _DashSpeed;
    public int _MaxDashCount;

    [Header("PlayerHand")]
    public GameObject _PlayerLeftHand;
    public GameObject _PlayerRightHand;

    [Header("PlayerProperties")]
    public int _Energy = 0;
    public int _EnergyThreshold;
    public int _EnergyLimition;
    public int _Armor;
    public int _BaseHP;
}
