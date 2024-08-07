using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private NetworkManager m_Manager;
    private GameObject m_StartMenu;
    [SerializeField]
    private PauseMenu m_PauseMenu;
    [SerializeField]
    private NetworkStatus m_NetworkStatus;
    [SerializeField]
    private GameObject m_MultiPlayerReadyPanel;
    [SerializeField]
    private GameObject m_MultiplayerPanel;
    [SerializeField]
    private TMP_InputField m_IpInput;
    [SerializeField]
    private TMP_InputField m_PortInput;

    bool isSingle = false;
    bool isServer = false;
    private void Awake()
    {
        m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
        m_StartMenu = gameObject;
    }

    private void Start()
    {
        m_PauseMenu.gameObject.SetActive(false);
    }

    public void OnSinglePlayerStart()
    {
        if(NetworkClient.active == false)
        {
            m_Manager.StartHost();
        }
        isSingle = true;
        
    }

    public void OnMultiPlayerPanelEnter()
    {
        m_MultiplayerPanel.SetActive(true);
    }

    public void OnBackToStartMenu()
    {
        m_MultiplayerPanel.SetActive(false);
        if (NetworkServer.active || NetworkClient.isConnected)
        {    
            m_Manager.StopClient();
            m_Manager.StopHost();
            m_Manager.StopServer();
        }
    }

    public void OnMultiPlayerStart()
    {
        if (NetworkClient.active == false)
        {
            m_Manager.StartHost();
        }
        isSingle = false;
        isServer = true;
        m_NetworkStatus.gameObject.SetActive(true);
    }

    public void OnMultiPlayerJoin()
    {
        if (NetworkClient.active == false)
        {
            m_Manager.StartClient();
        }
        m_Manager.networkAddress = m_IpInput.text;
        if (Transport.active is PortTransport portTransport)
        {
            if (ushort.TryParse(m_PortInput.text, out ushort port))
            {
                portTransport.Port = port;
            }
        }
        isSingle = false;
        m_NetworkStatus.gameObject.SetActive(true);
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Update()
    {
        if(isSingle && NetworkClient.isConnected && NetworkClient.ready && NetworkClient.localPlayer != null)
        {
            PlayerController.PlayerControllers[0].CmdStartGame();
            isSingle = false;
            m_MultiplayerPanel.SetActive(false);
            m_StartMenu.SetActive(false);
        }
        if(isSingle == false && !NetworkClient.isConnected && !NetworkServer.active)
        {
            m_NetworkStatus.SetDetail($"Connecting to {m_Manager.networkAddress}..");
        }
        if(isSingle == false && (NetworkClient.isConnected || NetworkServer.active))
        {
            if(isServer)
            {
                m_MultiPlayerReadyPanel.SetActive(true);
            }
            m_NetworkStatus.gameObject.SetActive(false);
            m_MultiplayerPanel.SetActive(false);
            m_StartMenu.SetActive(false);
        }
    }
}
