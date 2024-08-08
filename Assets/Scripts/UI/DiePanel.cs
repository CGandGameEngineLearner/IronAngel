using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePanel : MonoBehaviour
{
    public void OnBackToStartMenu()
    {
        if (NetworkClient.localPlayer != null)
        {
            NetworkClient.localPlayer.GetComponent<PlayerController>().EndGame();
        }
    }
}
