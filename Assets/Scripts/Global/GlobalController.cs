using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    private CameraController m_CameraController;
    private Player m_Player;
    private InputController m_InputController;
    

    //  public-----------------------------------------

    // private------------------------------------------
    private void Awake()
    {
        GlobalSetting setting = GetComponent<GlobalSetting>();

        m_CameraController = new CameraController();
        m_CameraController.Init(setting._Camera, setting._VirtualCamera, setting._VirtualCameraTarget, setting._CameraMinDistance, setting._CameraMaxDistance);

        m_Player = new Player();
        m_Player.Init(setting._Player, setting._MoveSpeed);

        m_InputController = new InputController();
        m_InputController.Init();

        Destroy(setting);
    }

    private void Start()
    {
        RegisterInputActionFunc();
    }

    private void Update()
    {
        UpdateCameraPosition();
        UpdatePlayerRotation();
        MovePlayer();
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPos = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
        targetPos *= m_CameraController.GetCameraDistance();
        targetPos.z = 0;
        m_CameraController.SetVirtualCameraTargetPosition(targetPos + m_Player.GetPlayerPosition());
    }

    private void UpdatePlayerRotation()
    {
        Vector3 v3 = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera());
        m_Player.LookAt(new Vector2(v3.x, v3.y));
    }

    private void MovePlayer()
    {
        m_InputController.ExcuteActionWhilePlayerMoveInputPerformedAndStay(() =>
        {
            var v2 = m_InputController.GetPlayerMoveInputVector2();
            m_Player.Move(new Vector3(v2.x, v2.y, 0));
        });
    }

    private void RegisterInputActionFunc()
    {
        // 镜头控制
        m_InputController.AddStartedActionToCameraViewTypeSwitch(() =>
        {
            m_CameraController.SetCameraDistanceToMax();
        });
        m_InputController.AddCanceledActionToCameraViewTypeSwitch(() =>
        {
            m_CameraController.SetCmaeraDistanceToMin();
        });
        // 鼠标限制
        m_InputController.SetCursorLockState(CursorLockMode.Confined);
    }
}
