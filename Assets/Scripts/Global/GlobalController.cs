using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    private CameraController m_CameraController;
    private Player m_Player;
    private InputController m_InputController;

    //  public------------------------------------------
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
        playerSpec.m_DashCoolDownTime = setting._DashCoolDownTime;
        playerSpec.m_DashCount = setting._DashCount;
        playerSpec.m_MaxDashCount = setting._MaxDashCount;
        playerSpec.m_DashSpeed = setting._DashSpeed;
        playerSpec.m_PlayerLeftHand = setting._PlayerLeftHand;
        playerSpec.m_PlayerRightHand = setting._PlayerRightHand;
        playerSpec.m_Energy = setting._Energy;
        playerSpec.m_EnergyThreshold = setting._EnergyThreshold;
        playerSpec.m_EnergyLimition = setting._EnergyLimition;
        playerSpec.m_BaseHP = setting._BaseHP;
        playerSpec.m_Armor = setting._Armor;
        m_Player.Init(playerSpec);

        m_InputController = new InputController();
        m_InputController.Init();

        Destroy(setting);
    }

    private void Start()
    {
        RegisterInputActionFunc();
        RegisterGameEvent();
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
        m_Player.FixedUpdate();
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
            m_Player.GetPlayer().GetComponent<LogicStateManager>().AddState(ELogicState.PlayerWalking);
            m_Player.Move(m_InputController.GetPlayerMoveInputVector2());
        });
    }

    private void RegisterInputActionFunc()
    {
        // 视角拉远
        m_InputController.AddStartedActionToCameraViewTypeSwitch(() =>
        {
            m_CameraController.SetCameraDistanceToMax();
        });
        m_InputController.AddCanceledActionToCameraViewTypeSwitch(() =>
        {
            m_CameraController.SetCmaeraDistanceToMin();
        });
        // 鼠标状态
        m_InputController.SetCursorLockState(CursorLockMode.Confined);
        // 玩家冲刺
        m_InputController.AddStartedActionToPlayerDash(() =>
        {
            if(m_Player.StartDash())
            {
                m_Player.GetPlayer().GetComponent<LogicStateManager>().AddState(ELogicState.PlayerDashing);
            }
        });
    }
    
    private void RegisterGameEvent()
    {
        // 玩家冲刺
        EventCenter.AddListener<bool>(EventType.StateToGlobal_PlayerDashState, (startDash) =>
        {
            if(startDash)
            {
                m_Player.ChangeDashCount(-1);
                var v3 = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
                var v2 = m_InputController.GetPlayerMoveInputVector2() == Vector2.zero ? new Vector2(v3.x, v3.y) : m_InputController.GetPlayerMoveInputVector2();
                m_Player.SetDashDirection(v2);
            }
            else
            {
                m_Player.Dash();
            }
        });
    }
}
