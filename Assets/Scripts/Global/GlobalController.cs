using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    private CameraController m_CameraController;
    private Player m_Player;
    private InputController m_InputController;
    private PlayerMovement m_PlayerMovement;

    //  public-----------------------------------------
    private void Awake()
    {
        GlobalSetting setting = GetComponent<GlobalSetting>();

        m_CameraController = new CameraController();
        m_CameraController.Init(setting._Camera, setting._VirtualCamera, setting._VirtualCameraTarget, setting._CameraMinDistance, setting._CameraMaxDistance);

        m_Player = new Player();
        m_Player.Init(setting._Player);

        m_InputController = new InputController();
        m_InputController.Init();

        m_PlayerMovement = new PlayerMovement();
        m_PlayerMovement.Init(setting._Player, setting._MoveSpeed);

        Destroy(setting);
    }

    private void Start()
    {
        m_InputController.AddStartedActionToCameraViewTypeSwitch(() =>
        {
            m_CameraController.SetCameraDistanceToMax();
        });
        m_InputController.AddCanceledActionToCameraViewTypeSwitch(() =>
        {
            m_CameraController.SetCmaeraDistanceToMin();
        });

        m_InputController.SetCursorLockState(CursorLockMode.Confined);
    }

    private void Update()
    {
        Vector3 targetPos = m_InputController.GetMousePositionInWorldSpace(m_CameraController.GetCamera()) - m_Player.GetPlayerPosition();
        targetPos *= m_CameraController.GetCameraDistance();
        targetPos.z = 0;
        m_CameraController.SetVirtualCameraTargetPosition(targetPos + m_Player.GetPlayerPosition());
    }
}
