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
        gameObject.SetActive(false);
    }

    public void OnBackToStartMenu()
    {
        if(NetworkClient.localPlayer != null)
        {
            UICanvas.Instance.BackToStartMenu();
        }
    }
}
