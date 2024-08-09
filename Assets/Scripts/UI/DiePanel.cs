using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiePanel : MonoBehaviour
{
    public void OnBackToStartMenu()
    {
        if (NetworkClient.localPlayer != null)
        {

            UICanvas.Instance.BackToStartMenu();
        }
    }
}
