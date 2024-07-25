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

    public void DisablePlayerInput()
    {
        m_PlayerInputConfig.Disable();
    }

    public void EnablePlayerInput()
    {
        m_PlayerInputConfig.Enable();
    }

    public Vector3 GetMousePositionOnScreen()
    {
        return Input.mousePosition;
    }

    public Vector3 GetMousePositionInWorldSpace(Camera mainCamera)
    {
        var pos = Input.mousePosition;
        pos.z = mainCamera.WorldToScreenPoint(new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 5, mainCamera.transform.position.z)).z;
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

    public void AddPerformActionToCameraViewTypeSwitch(Action func)
    {
        m_PlayerInputConfig.Camera.ViewTypeSwitch.performed += ctx =>
        {
            func?.Invoke();
        };
    }

    public void RemovePerformActionFromCameraViewTypeSwitch(Action func)
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
}
