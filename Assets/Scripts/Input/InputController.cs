using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController
{
    private PlayerInputConfig m_PlayerInputConfig;
    private Gamepad m_GamePad;

    private Action m_PlayerMoveInputExcuting;
    private Action m_PlayerLeftHandShooting;
    private Action m_PlayerRightHandShooting;

//  public---------------------------------------------------
    public void Init()
    {
        m_PlayerInputConfig = new PlayerInputConfig();
        m_PlayerInputConfig.Enable();
    }
    public void UpdateInputDevice()
    {
        m_GamePad = Gamepad.current;
    }

    public bool IsGamePadInput()
    {
        return m_GamePad != null;
    }

    public void DisableAllInput()
    {
        m_PlayerInputConfig.Disable();
    }

    public void EnableAllInput()
    {
        m_PlayerInputConfig.Enable();
    }

    public void DisablePlayerInput()
    {
        m_PlayerInputConfig.Player.Disable();
    }

    public void EnablePlayerInput()
    {
        m_PlayerInputConfig.Player.Enable();
    }

    public Vector2 GetGamePadViewInput()
    {
        return m_PlayerInputConfig.Camera.View.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerMoveInputVector2()
    {
        return m_PlayerInputConfig.Player.Move.ReadValue<Vector2>();
    }

    public bool IsPlayerMoveInput()
    {
        return m_PlayerInputConfig.Player.Move.ReadValue<Vector2>() != Vector2.zero;
    }

    public Vector3 GetMousePositionOnScreen()
    {
        return Input.mousePosition;
    }

    public Vector3 GetMousePositionInWorldSpace(Camera mainCamera)
    {
        var pos = Input.mousePosition;
        pos.z = mainCamera.WorldToScreenPoint(new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0)).z;
        return mainCamera.ScreenToWorldPoint(pos);
    }

    public void AddStartedActionToCameraViewTypeSwitch(Action func)
    {
        m_PlayerInputConfig.Camera.ViewTypeSwitch.started += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveStartedActionToCameraViewTypeSwitch(Action func)
    {
        m_PlayerInputConfig.Camera.ViewTypeSwitch.started -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void AddPerformedActionToCameraViewTypeSwitch(Action func)
    {
        m_PlayerInputConfig.Camera.ViewTypeSwitch.performed += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemovePerformedActionFromCameraViewTypeSwitch(Action func)
    {
        m_PlayerInputConfig.Camera.ViewTypeSwitch.performed -= ctx =>
        {
            func?.Invoke();
        };
    }
    public void AddCanceledActionToCameraViewTypeSwitch(Action func)
    {
        m_PlayerInputConfig.Camera.ViewTypeSwitch.canceled += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveCanceledActionToCameraViewTypeSwitch(Action func)
    {
        m_PlayerInputConfig.Camera.ViewTypeSwitch.canceled -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void SetCursorLockState(CursorLockMode mode)
    {
        Cursor.lockState = mode;
    }

    public void AddStartedActionToPlayerMove(Action func)
    {
        m_PlayerInputConfig.Player.Move.started += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveStartedActionToPlayerMove(Action func)
    {
        m_PlayerInputConfig.Player.Move.started -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void AddPerformedActionToPlayerMove(Action func)
    {
        m_PlayerInputConfig.Player.Move.performed += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemovePerformedActionToPlayerMove(Action func)
    {
        m_PlayerInputConfig.Player.Move.performed -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void AddCanceledActionToPlayerMove(Action func)
    {
        m_PlayerInputConfig.Player.Move.canceled += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveCanceledActionToPlayerMove(Action func)
    {
        m_PlayerInputConfig.Player.Move.canceled -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void ExcuteActionWhilePlayerMoveInputPerformedAndStay()
    {
        if(m_PlayerInputConfig.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            m_PlayerMoveInputExcuting?.Invoke();
        }
    }

    public void AddActionWhilePlayerMoveInputPerformedAndStay(Action func)
    {
        m_PlayerMoveInputExcuting += func;
    }

    public void RemoveActionWhilePlayerMoveInputPerformedAndStay(Action func)
    {
        m_PlayerMoveInputExcuting -= func;
    }

    public void AddStartedActionToPlayerDash(Action func)
    {
        m_PlayerInputConfig.Player.Dash.started += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveStartedActionToPlayerDash(Action func)
    {
        m_PlayerInputConfig.Player.Dash.started -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void AddCanceledActionToPlayerDash(Action func)
    {
        m_PlayerInputConfig.Player.Dash.canceled += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveCanceledActionToPlayerDash(Action func)
    {
        m_PlayerInputConfig.Player.Dash.canceled -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void AddStartedActionToPlayerShootLeft(Action func)
    {
        m_PlayerInputConfig.Player.Shoot_Left.started += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveStartedActionToPlayerShootLeft(Action func)
    {
        m_PlayerInputConfig.Player.Shoot_Left.started -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void ExcuteActionWhilePlayerShootLeftInputPerformedAndStay()
    {
        if(m_PlayerInputConfig.Player.Shoot_Left.ReadValue<float>() != 0f)
        {
            m_PlayerLeftHandShooting?.Invoke();
        }
    }

    public void AddActionWhilePlayerShootLeftInputPerformedAndStay(Action func)
    {
        m_PlayerLeftHandShooting += func;
    }

    public void RemoveActionWhilePlayerShootLeftInputPerformedAndStay(Action func)
    {
        m_PlayerLeftHandShooting -= func;
    }

    public void AddStartedActionToPlayerShootRight(Action func)
    {
        m_PlayerInputConfig.Player.Shoot_Right.started += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemoveStartedActionToPlayerShootRight(Action func)
    {
        m_PlayerInputConfig.Player.Shoot_Right.started -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void ExcuteActionWhilePlayerShootRightInputPerformedAndStay()
    {
        if(m_PlayerInputConfig.Player.Shoot_Right.ReadValue<float>() != 0f)
        {
            m_PlayerRightHandShooting?.Invoke();
        }
    }

    public void AddActionWhilePlayerShootRightInputPerformedAndStay(Action func)
    {
        m_PlayerRightHandShooting += func;
    }

    public void RemoveActionWhilePlayerShootRightInputPerformedAndStay(Action func)
    {
        m_PlayerRightHandShooting -= func;
    }

    public void AddPerformedActionToPlayerThrowAndPickLeft(Action func)
    {
        m_PlayerInputConfig.Player.ThrowAndPick_Left.performed += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemovePerformedActionToPlayerThrowAndPickLeft(Action func)
    {
        m_PlayerInputConfig.Player.ThrowAndPick_Left.performed -= ctx =>
        {
            func?.Invoke();
        };
    }

    public void AddPerformedActionToPlayerThrowAndPickRight(Action func)
    {
        m_PlayerInputConfig.Player.ThrowAndPick_Right.performed += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemovePerformedActionToPlayerThrowAndPickRight(Action func)
    {
        m_PlayerInputConfig.Player.ThrowAndPick_Right.performed -= ctx =>
        {
            func?.Invoke();
        };
    }
    public void AddPerformedActionToPlayerInteract(Action func)
    {
        m_PlayerInputConfig.Player.Interact.performed += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemovePerformedActionToPlayerInteract(Action func)
    {
        m_PlayerInputConfig.Player.Interact.performed -= ctx =>
        {
            func?.Invoke();
        };
    }
}
