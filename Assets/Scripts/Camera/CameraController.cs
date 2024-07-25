using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController
{
    private Camera m_Camera;
    private CinemachineVirtualCamera m_VirtualCamera;
    private Transform m_VirtualCameraTarget;
    private float m_MaxDistance;
    private float m_MinDistance;

    private float m_CameraDistance;
//  public-----------------------------------------
    public void Init(Camera camera, CinemachineVirtualCamera virtualCamera, Transform virtualCameraTarget, float minDis, float maxDis)
    {
        m_Camera = camera;
        m_VirtualCamera = virtualCamera;
        m_VirtualCameraTarget = virtualCameraTarget;
        m_MinDistance = minDis;
        m_MaxDistance = maxDis;
        m_CameraDistance = minDis;
    }

    public Camera GetCamera()
    {
        return m_Camera; 
    }

    public void SetVirtualCameraTargetPosition(Vector3 targetPosition)
    {
        m_VirtualCameraTarget.position = targetPosition;
    }

    public Vector3 GetVirtualCameraTargetPosition()
    {
        return m_VirtualCameraTarget.position;
    }

    public void SetVirtualCameraTargetRotation(Quaternion targetRotation)
    {
        m_VirtualCameraTarget.rotation = targetRotation;
    }

    public Quaternion GetVirtualCameraTargetRotation()
    {
        return m_VirtualCameraTarget.rotation;
    }

    public float GetMaxDistance()
    {
        return m_MaxDistance;
    }

    public float GetMinDistance()
    {
        return m_MinDistance;
    }

    public void SetCameraDistanceToMax()
    {
        m_CameraDistance = m_MaxDistance;
    }

    public void SetCmaeraDistanceToMin()
    {
        m_CameraDistance = m_MinDistance;
    }

    public float GetCameraDistance()
    {
        return m_CameraDistance;
    }
}
