using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private StartMenu m_StartMenu;


    public void OnContinue()
    {
        UICanvas.Instance.ShowPauseMenu(false);
    }

    public void OnBackToStartMenu()
    {
        if(NetworkClient.localPlayer != null||(NetworkClient.localPlayer==null&&!NetworkClient.isConnected))
        {
            UICanvas.Instance.BackToStartMenu();
        }
    }
}
