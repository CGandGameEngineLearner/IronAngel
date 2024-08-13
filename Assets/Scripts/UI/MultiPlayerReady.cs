
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerReady : MonoBehaviour
{
    private GameObject m_MultiPlayerReadyPanel;
    

    private void Awake()
    {
        m_MultiPlayerReadyPanel = gameObject;
    }
    public void OnMultiPlayerGameStart()
    {
        PlayerController.PlayerControllers[0].CmdStartGame();
        m_MultiPlayerReadyPanel.SetActive(false);
        EventCenter.Broadcast(EventType.MultiPlayerGameStart);

        GameObject battleZoneTriggers = GameObject.Find("BattleZoneTrigger");
        battleZoneTriggers.SetActive(true);
    }
}
