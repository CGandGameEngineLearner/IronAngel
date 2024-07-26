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
        PlayerSpec playerSpec = new PlayerSpec();
        playerSpec.m_Player = setting._Player;
        playerSpec.m_NormalSpeed = setting._MoveSpeed;
        playerSpec.m_DashTime = setting._DashTime;
        playerSpec.m_DashCoolDownTime = setting._DashCoolDownTime;
        playerSpec.m_DashCount = setting._DashCount;
        playerSpec.m_MaxDashCount = setting._MaxDashCount;
        playerSpec.m_DashSpeed = setting._DashSpeed;
        m_Player.Init(playerSpec);

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
        m_Player.Update();
    }

    private void FixedUpdate()
    {
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
        Vector3 v3 = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
        m_Player.LookAt(new Vector2(v3.x, v3.y));
    }

    private void MovePlayer()
    {
        m_InputController.ExcuteActionWhilePlayerMoveInputPerformedAndStay(() =>
        {
            m_Player.Move(m_InputController.GetPlayerMoveInputVector2());
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
        // 玩家冲刺
        m_InputController.AddStartedActionToPlayerDash(() =>
        {
            m_Player.Dash(m_InputController.GetPlayerMoveInputVector2());
        });
    }
}
