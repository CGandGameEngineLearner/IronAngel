
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using LogicState;
using UnityEngine.SceneManagement;

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
        get { return WeaponSystemCenter.Instance; }
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
        playerSpec = setting._PlayerSpec;
        playerSpec.m_Player = this.gameObject;
            
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

        PlayerControllers.Add(this);
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
        
        
        m_Player.Update();
        m_InputController.UpdateInputDevice();
        if (Input.GetKeyDown(KeyCode.P) && isServer)
        {
            CmdStartGame();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (NetworkServer.active&&isServer)
            {
                GameObject.FindAnyObjectByType<NetworkManager>().StopHost();
            }
            else
            {  
                GameObject.FindAnyObjectByType<NetworkManager>().StopClient();
            }
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerSpec playerSpec = new PlayerSpec();
        }
    }

    [Command]
    void CmdStartGame()
    {
        WeaponSystemCenter.Instance.CmdStartGame();
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!m_AfterStartLocalPlayer)
            return;
        m_Player.FixedUpdate();
        UpdatePlayerRotation();
        UpdatePlayerMovement();
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

    [ClientCallback]
    private void UpdatePlayerRotation()
    {
            if(m_InputController.IsGamePadInput())
            {
                m_Player.LookAt(m_InputController.GetGamePadViewInput());
            }
            else
            {
                Vector3 v3 = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
                m_Player.LookAt(new Vector2(v3.x, v3.y));
            }
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
            CmdLeftHandPickWeapon();
        });
        m_InputController.AddPerformedActionToPlayerThrowAndPickRight(() =>
        {
            CmdRightHandPickWeapon();
        });
        // 玩家攻击
        // 左手
        m_InputController.AddActionWhilePlayerShootLeftInputPerformedAndStay(() =>
        {
            if (isLocalPlayer)
            {
                var weapon = m_Player.GetPlayerLeftHandWeapon();
                var pos = m_Player.GetPlayerLeftHandPosition();
                if (weapon == null)
                {
                    Debug.Log("左手上没武器");
                    return;
                }
                CmdFire(weapon,pos,m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition());
            }
            
        });
        // 右手
        m_InputController.AddActionWhilePlayerShootRightInputPerformedAndStay(() =>
        {
            if(isLocalPlayer)
            {
                var weapon = m_Player.GetPlayerRightHandWeapon();
                var pos = m_Player.GetPlayerRightHandPosition();
                if (weapon == null)
                {
                    Debug.Log("右手上没武器");
                    return;
                }
                CmdFire(weapon, pos, m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition());
            }
        });
    }
    
    [Command]
    private void CmdFire(GameObject weapon, Vector3 startPoint, Vector3 dir)
    {   
        WeaponSystemCenter.Instance.CmdFire(gameObject,weapon,startPoint,dir);
    }

    [Command]
    private void CmdLeftHandPickWeapon()
    {
        // 这里的交互顺序不能换
        // 必须先获取最近武器再使得玩家丢下武器
        // 否则会有碰撞体冲突的问题
        var nearestWeapon = m_Player.GetNearestWeapon();

        RPCLeftHandPickWeapon(nearestWeapon);
    }

    [ClientRpc]
    private void RPCLeftHandPickWeapon(GameObject weapon)
    {
        var handWeapon = m_Player.DropPlayerLeftHandWeapon(m_Player.GetPlayerPosition());
        m_Player.SetPlayerLeftHandWeapon(weapon);
    }

    [Command]
    private void CmdRightHandPickWeapon()
    {
        // 这里的交互顺序不能换
        // 必须先获取最近武器再使得玩家丢下武器
        // 否则会有碰撞体冲突的问题
        var nearestWeapon = m_Player.GetNearestWeapon();

        RPCRightHandPickWeapon(nearestWeapon);
    }

    [ClientRpc]
    private void RPCRightHandPickWeapon(GameObject weapon)
    {
        var handWeapon = m_Player.DropPlayerRightHandWeapon(m_Player.GetPlayerPosition());

        m_Player.SetPlayerRightHandWeapon(weapon);
    }
    
    
    
    [ClientCallback]
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
        EventCenter.AddListener(EventType.StateToGlobal_PlayerWalkState, () =>
        {
            m_Player.Move(m_InputController.GetPlayerMoveInputVector2());
        });
    }
}


