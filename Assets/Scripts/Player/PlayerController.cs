
using Mirror;

using System.Collections.Generic;

using UnityEngine;
using Cinemachine;

public class PlayerController : NetworkBehaviour
{
    public readonly static List<PlayerController> PlayerControllers = new List<PlayerController>();
    private CameraController m_CameraController = new CameraController();
    private Player m_Player = new Player();
    private InputController m_InputController = new InputController();
    private WeaponSystemCenter m_WeaponSystemCenter;
    private LogicStateManager m_LogicStateManager;

    bool m_AfterStartLocalPlayer = false;
    //  public------------------------------------------
    public WeaponSystemCenter WeaponSystemCenter
    {
        get { return m_WeaponSystemCenter; }
    }
    public Player Player
    {
        get { return m_Player; }
    }
    public InputController InputController
    {
        get { return m_InputController; }
    }
    public CameraController CameraController
    {
        get { return CameraController; }
    }

    private void Start()
    {
        m_LogicStateManager = GetComponent<LogicStateManager>();
        PlayerSetting setting = GetComponent<PlayerSetting>();
        PlayerSpec playerSpec = new PlayerSpec();
        playerSpec.m_Player = this.gameObject;
        playerSpec.m_NormalSpeed = setting._MoveSpeed;
        playerSpec.m_DashCoolDownTime = setting._DashCoolDownTime;
        playerSpec.m_DashCount = setting._DashCount;
        playerSpec.m_MaxDashCount = setting._MaxDashCount;
        playerSpec.m_DashSpeed = setting._DashSpeed;
        playerSpec.m_Energy = setting._Energy;
        playerSpec.m_EnergyThreshold = setting._EnergyThreshold;
        playerSpec.m_EnergyLimition = setting._EnergyLimition;
        playerSpec.m_BaseHP = setting._BaseHP;
        playerSpec.m_Armor = setting._Armor;
        playerSpec.m_DetectRange = setting._DetectRange;
        playerSpec.m_WeaponLayer = setting._WeaponLayer;
        m_Player.Init(playerSpec);
        
    }


#if !UNITY_SERVER
    public override void OnStartLocalPlayer()
    {
        PlayerSetting setting = GetComponent<PlayerSetting>();
        
        m_CameraController.Init(Camera.main, GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineVirtualCamera>(), GameObject.FindWithTag("CameraTarget").transform, setting._CameraMinDistance, setting._CameraMaxDistance);
        m_InputController.Init();
        
        RegisterInputActionFunc();
        RegisterGameEvent();

        m_AfterStartLocalPlayer = true;
    }
#endif


    public void OnDestroy()
    {
        PlayerControllers.Remove(this);
    }
    // private------------------------------------------

    

    [ClientCallback]
    private void Update()
    {
        if (!m_AfterStartLocalPlayer)
            return;
        
        UpdatePlayerMovement();
        m_Player.Update();
        m_InputController.UpdateInputDevice();
        //m_WeaponSystemCenter.Update();
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!m_AfterStartLocalPlayer)
            return;
        m_Player.FixedUpdate();
        UpdatePlayerRotation();
        m_InputController.ExcuteActionWhilePlayerMoveInputPerformedAndStay();
        m_InputController.ExcuteActionWhilePlayerShootLeftInputPerformedAndStay();
        m_InputController.ExcuteActionWhilePlayerShootRightInputPerformedAndStay();
    }

    [ClientCallback]
    private void LateUpdate()
    {
        if (!m_AfterStartLocalPlayer)
            return;
        UpdateCameraPosition();
    }

#if !UNITY_SERVER
    [ClientCallback]
    private void UpdateCameraPosition()
    {
        Vector3 targetPos = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
        targetPos *= m_CameraController.GetCameraDistance();
        targetPos.z = 0;
        m_CameraController.SetVirtualCameraTargetPosition(targetPos + m_Player.GetPlayerPosition());
    }
#endif

    
    private void UpdatePlayerRotation()
    {
        // if (isLocalPlayer)
        // {
            if(m_InputController.IsGamePadInput())
            {
                if(m_InputController.GetGamePadViewInput() != Vector2.zero)
                {
                    m_Player.LookAt(m_InputController.GetGamePadViewInput());
                }
            }
            else
            {
                Vector3 v3 = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
                m_Player.LookAt(new Vector2(v3.x, v3.y));
            }
        // }
        
    }

    [ClientCallback]
    private void UpdatePlayerMovement()
    {
        if(m_InputController.IsPlayerMoveInput())
        {
            m_LogicStateManager.AddState(ELogicState.PlayerWalking);
        }
        else
        {
            m_LogicStateManager.RemoveState(ELogicState.PlayerWalking);
        }
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
        // 玩家拾取武器
        m_InputController.AddPerformedActionToPlayerThrowAndPickLeft(() =>
        {
            // 这里的交互顺序不能换
            // 必须先获取最近武器再使得玩家丢下武器
            // 否则会有碰撞体冲突的问题
            var nearestWeapon = m_Player.GetNearestWeapon();
            var handWeapon = m_Player.DropPlayerLeftHandWeapon(m_Player.GetPlayerPosition());
            
            m_Player.SetPlayerLeftHandWeapon(nearestWeapon);
        });
        m_InputController.AddPerformedActionToPlayerThrowAndPickRight(() =>
        {
            // 这里的交互顺序不能换
            // 必须先获取最近武器再使得玩家丢下武器
            // 否则会有碰撞体冲突的问题
            var nearestWeapon = m_Player.GetNearestWeapon();
            var handWeapon = m_Player.DropPlayerRightHandWeapon(m_Player.GetPlayerPosition());

            m_Player.SetPlayerRightHandWeapon(nearestWeapon);
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
        // 玩家移动
        EventCenter.AddListener<bool>(EventType.StateToGlobal_PlayerWalkState, (walking) =>
        {
            m_Player.Move(m_InputController.GetPlayerMoveInputVector2());
        });
    }
}

[System.Serializable]
public struct WeaponCat
{
    public WeaponType weaponType;
    public WeaponConfig weaponConfig;
}

[System.Serializable]
public struct AmmunitionCat
{
    public AmmunitionType ammunitionType;
    public AmmunitionConfig ammunitionConfig;
}
