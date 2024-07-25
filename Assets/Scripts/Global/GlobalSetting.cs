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
}
