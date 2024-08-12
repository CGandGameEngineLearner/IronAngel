using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StartMenu : MonoBehaviour
{
    private NetworkManager m_Manager;
    private GameObject m_StartMenu;
    [SerializeField]
    private GameObject m_PropertiesUI;
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
    
    // 打包有bug，必须得在这拉个shi
    [SerializeField] private LevelSwitchConfig m_LevelSwitchConfig;

    public GameObject MultiplayerPanel
    {
        get { return m_MultiplayerPanel;  }
    }

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
        // Load Player Save file,Then Get Config to find load which level
        SaveLoadManager.LoadGame();
        LevelSwitchConfig levelSwitchConfig = m_LevelSwitchConfig;
        // 大关卡
        int level = SaveLoadManager.GlobalSaveFile.currentLevel;
        int section = SaveLoadManager.GlobalSaveFile.currentSection;

        string sectionName = section == -1 ? levelSwitchConfig.basementName : levelSwitchConfig.levelStruct[level].sectionName[section];
        
        SceneManager.LoadScene(sectionName);
        
        // if(NetworkClient.active == false)
        // {
        //     // m_Manager.StartHost();
        //     SceneManager.LoadScene("Level1_Area1_Highway");
        // }
         isSingle = true;
    }

    public void OnSinglePlayerContinue()
    {

    }

    public void OnMultiPlayerPanelEnter()
    {
        SceneManager.LoadScene("PVPLevel");
        m_MultiplayerPanel.SetActive(true);
    }

    public void OnBackToStartMenu()
    {
        m_MultiplayerPanel.SetActive(false);
        m_MultiPlayerReadyPanel.SetActive(false);
        if (NetworkServer.active || NetworkClient.isConnected)
        {    
            m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
            m_Manager.StopClient();
            m_Manager.StopHost();
            m_Manager.StopServer();
        }
    }

    public void OnMultiPlayerStart()
    {
        if (NetworkClient.active == false)
        {
            m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
            m_Manager.StartHost();
        }
        isSingle = false;
        isServer = true;
        m_NetworkStatus.gameObject.SetActive(true);
    }

    public void OnMultiPlayerJoin()
    {
        m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
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
            //PlayerController.PlayerControllers[0].CmdStartGame();
            // isSingle = false;
            m_MultiplayerPanel.SetActive(false);
            m_StartMenu.SetActive(false);
            m_PropertiesUI.SetActive(true);
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
            m_PropertiesUI.SetActive(true);
            isServer = false;
        }
    }
}
