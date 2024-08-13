
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using LogicState;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Collections;


[RequireComponent(typeof(BaseProperties))]
public class PlayerController : NetworkBehaviour
{
    public readonly static List<PlayerController> PlayerControllers = new List<PlayerController>();
    private CameraController m_CameraController = new CameraController();
    private Player m_Player = new Player();
    private InputController m_InputController = new InputController();
    private LogicStateManager m_LogicStateManager;
    private BaseProperties m_BaseProperties;
    private PlayerSetting setting;

    bool m_AfterStartLocalPlayer = false;
    float m_FireDistance;
    public List<int> m_Power;
    public List<AudioClip> m_PowerAudios;
    public AudioClip m_WeaponBrokenAudio;
    public AudioClip m_EmptyWeapon;
    //  public------------------------------------------
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
        get { return m_CameraController; }
    }

    private void Start()
    {
        m_LogicStateManager = GetComponent<LogicStateManager>();
        m_BaseProperties = GetComponent<BaseProperties>();
        setting = GetComponent<PlayerSetting>();
        PlayerSpec playerSpec = new PlayerSpec();
        playerSpec = setting._PlayerSpec;
        playerSpec.m_Player = this.gameObject;
            
        m_Player.Init(playerSpec);
    }


#if !UNITY_SERVER
    public override void OnStartLocalPlayer()
    {
        PlayerSetting setting = GetComponent<PlayerSetting>();

        m_Power = setting._PowerLimit;
        m_FireDistance = setting._FireDistance;
        m_PowerAudios = setting._PowerAudioClips;
        m_WeaponBrokenAudio = setting._WeaponBroken;
        m_EmptyWeapon = setting.m_EmptyWeapon;


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
        UnRegisterGameEvent();
        PlayerControllers.Remove(this);
        m_InputController.DisposeAllInput();
    }


    // 玩家眩晕
    public void SetPlayerStun()
    {
        m_InputController.DisablePlayerInput();
    }

    // 恢复玩家眩晕
    public void ResetPlayerStun()
    {
        m_InputController.EnablePlayerInput();
    }

    // private------------------------------------------
    [ClientCallback]
    private void Update()
    {
        if (!m_AfterStartLocalPlayer)
            return;
        
        
        m_Player.Update();
        m_InputController.UpdateInputDevice();
    }
    
    

    /// <summary>
    /// 公用切换场景的方法
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (NetworkServer.active && isServer)
        {
            GameObject.FindAnyObjectByType<NetworkManager>().StopHost();
        }
        else
        {
            GameObject.FindAnyObjectByType<NetworkManager>().StopClient();
        }

        SceneManager.LoadScene(sceneName);
    }

    [Command]
    public void CmdStartGame()
    {
        WeaponSystemCenter.Instance.CmdStartGame();
    }
    
    /// <summary>
    /// 用来给训练场生成枪械
    /// </summary>
    /// <param name="weaponType"></param>
    /// <param name="pos"></param>
    [Command]
    public void CmdSpawnWeapon(WeaponType weaponType, Vector3 pos)
    {
        WeaponSystemCenter.Instance.SpawnWeapon(weaponType, pos);
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!m_AfterStartLocalPlayer)
            return;


        Vector3 dir_left = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerLeftHandPosition();
        Vector3 dir_right = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerRightHandPosition();
        if (Vector2.Distance(m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()), m_Player.GetPlayerLeftHandPosition()) <= m_FireDistance)
        {
            var v3 = (m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition()).normalized;
            v3.z = 0;
            v3 = v3.normalized;
            dir_left = v3 * m_FireDistance + m_Player.GetPlayerPosition() - m_Player.GetPlayerLeftHandPosition();
        }
        if (Vector2.Distance(m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()), m_Player.GetPlayerRightHandPosition()) <= m_FireDistance)
        {
            var v3 = (m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition()).normalized;
            v3.z = 0;
            v3 = v3.normalized;
            dir_right = v3 * m_FireDistance + m_Player.GetPlayerPosition() - m_Player.GetPlayerRightHandPosition();
        }
        m_Player.FixedUpdate(dir_left, dir_right);
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
        if(m_LogicStateManager.IncludeState(ELogicState.StunModifier))
        {
            return;
        }
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
            m_CameraController.SwitchCameraDis();
        });
        // 鼠标状态
        m_InputController.SetCursorLockState(CursorLockMode.Confined);
        // 玩家冲刺
        m_InputController.AddStartedActionToPlayerDash(() =>
        {
            if(m_Player.StartDash())
            {
                m_LogicStateManager.AddState(ELogicState.PlayerDashing);
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
                    return;
                }
                pos = weapon.GetComponent<WeaponInstance>().firePoint.position;
                if(weapon.GetComponent<WeaponInstance>().GetCurrentMag() <= 0)
                {
                    UICanvas.Instance.SetTips("Ammo ran out!!(Left)", 1.0f);
                }
                Vector3 dir = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerLeftHandPosition();
                if (Vector2.Distance(m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()), m_Player.GetPlayerLeftHandPosition()) <= m_FireDistance)
                {
                    var v3 = (m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition()).normalized;
                    v3.z = 0;
                    v3 = v3.normalized;
                    dir = v3 * m_FireDistance + m_Player.GetPlayerPosition() - m_Player.GetPlayerLeftHandPosition();
                }
                CmdFire(weapon,pos, dir);
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
                    return;
                }
                pos = weapon.GetComponent<WeaponInstance>().firePoint.position;
                if (weapon.GetComponent<WeaponInstance>().GetCurrentMag() <= 0)
                {
                    UICanvas.Instance.SetTips("Ammo ran out!!(Right)", 1.0f);
                }
                Vector3 dir = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerRightHandPosition();
                if (Vector2.Distance(m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()), m_Player.GetPlayerRightHandPosition()) <= m_FireDistance)
                {
                    var v3 = (m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition()).normalized;
                    v3.z = 0;
                    v3 = v3.normalized;
                    dir = v3 * m_FireDistance + m_Player.GetPlayerPosition() - m_Player.GetPlayerRightHandPosition();
                }
               
                CmdFire(weapon, pos, dir);
            }
        });
        // 左右手结束攻击
        m_InputController.AddCanceledActionToPlayerShootLeft(() =>
        {
            if(isLocalPlayer)
            {
                var weapon = m_Player.GetPlayerLeftHandWeapon();
                if(weapon == null) { return; }
                UnFire(weapon);
            }
        });
        m_InputController.AddCanceledActionToPlayerShootRight(() =>
        {
            if(isLocalPlayer)
            {
                var weapon = m_Player.GetPlayerRightHandWeapon();
                if (weapon == null) { return; }
                UnFire(weapon);
            }
        });
        // 玩家能量奖励
        m_InputController.AddPerformedActionToPower_1(() =>
        {
            if(m_Power.Count > 0 && m_BaseProperties.m_Properties.m_Energy >= m_Power[0])
            {
                m_BaseProperties.m_Properties.m_Energy -= m_Power[0];
                CmdSpecFire(m_Player.GetPlayer(), WeaponType.SPExplosiveLuncher, m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()), Vector3.zero);
            }
        });
        m_InputController.AddPerformedActionToPower_2(() =>
        {
            if (m_Power.Count > 1 && m_BaseProperties.m_Properties.m_Energy >= m_Power[1])
            {
                m_BaseProperties.m_Properties.m_Energy -= m_Power[1];
                CmdSpecFire(m_Player.GetPlayer(), WeaponType.SPKnightPilumLuncher, m_Player.GetPlayerPosition(), (m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition()).normalized);
            }
        });
        m_InputController.AddPerformedActionToPower_3(() =>
        {
            if (m_Power.Count > 2 && m_BaseProperties.m_Properties.m_Energy >= m_Power[2])
            {
                m_BaseProperties.m_Properties.m_Energy -= m_Power[2];
                CmdSpecFire(m_Player.GetPlayer(), WeaponType.SPEMPLuncher, m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()), Vector3.zero);
            }
        });
        m_InputController.AddPerformedActionToPower_4(() =>
        {
            if (m_Power.Count > 3 && m_BaseProperties.m_Properties.m_Energy >= m_Power[3])
            {
                m_BaseProperties.m_Properties.m_Energy -= m_Power[3];
            }
            StartCoroutine(SpecFire());
        });
    }

 

    IEnumerator SpecFire()
    {
        for(int i = 0; i < 10; i++)
        {
            CmdSpecFire(m_Player.GetPlayer(), WeaponType.SPRocketPodLuncher, m_Player.GetPlayerPosition(), (m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition()).normalized);
            yield return new WaitForSeconds(0.5f); ;
        }
        yield return null;
    }

    [Command]
    private void CmdSpecFire(GameObject character, WeaponType type, Vector3 start, Vector3 dir)
    {
        WeaponSystemCenter.Instance.CmdSPFire(character, type, start, dir);
    }

    [Command]
    private void UnFire(GameObject weapon)
    {
        WeaponSystemCenter.Instance.RpcUnFire(weapon);
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
        EventCenter.AddListener<bool>(EventType.StateToGlobal_PlayerDashState, OnDashEvent);
        // 玩家移动
        EventCenter.AddListener(EventType.StateToGlobal_PlayerWalkState, OnWalkEvent);
        
    }


    private void UnRegisterGameEvent()
    {
        EventCenter.RemoveListener<bool>(EventType.StateToGlobal_PlayerDashState, OnDashEvent);
        EventCenter.RemoveListener(EventType.StateToGlobal_PlayerWalkState, OnWalkEvent);
        
    }


    private void OnDashEvent(bool startDash)
    {
        if(startDash)
        {
            m_Player.ChangeDashCount(-1);
            var v3 = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
            var v2 = m_InputController.GetPlayerMoveInputVector2() == Vector2.zero ? new Vector2(v3.x, v3.y) : m_InputController.GetPlayerMoveInputVector2();
            m_Player.SetDashDirection(v2);
            foreach (var whiff in setting._Whiff)
            {
                whiff.SetActive(true);
            }
        }
        else
        {
            m_Player.Dash();
        }
    }

    private void OnWalkEvent()
    {
        m_Player.Move(m_InputController.GetPlayerMoveInputVector2());
        foreach (var whiff in setting._Whiff)
        {
            whiff.SetActive(false);
        }
    }
}


