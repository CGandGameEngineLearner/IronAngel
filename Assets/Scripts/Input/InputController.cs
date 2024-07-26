using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    private PlayerInputConfig m_PlayerInputConfig;

//  public---------------------------------------------------
    public void Init()
    {
        m_PlayerInputConfig = new PlayerInputConfig();
        m_PlayerInputConfig.Enable();
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

    public Vector2 GetPlayerMoveInputVector2()
    {
        return m_PlayerInputConfig.Player.Move.ReadValue<Vector2>();
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

    public void ExcuteActionWhilePlayerMoveInputPerformedAndStay(Action func)
    {
        if(m_PlayerInputConfig.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            func?.Invoke();
        }
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
}
