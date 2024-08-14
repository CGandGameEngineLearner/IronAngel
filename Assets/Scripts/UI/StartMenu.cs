using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StartMenu : MonoBehaviour
{
    private NetworkManager m_Manager;
    private GameObject m_StartMenu;
    [SerializeField] private GameObject m_PropertiesUI;
    [SerializeField] private PauseMenu m_PauseMenu;
    [SerializeField] private NetworkStatus m_NetworkStatus;
    [SerializeField] private GameObject m_MultiPlayerReadyPanel;
    [SerializeField] private GameObject m_MultiplayerPanel;
    [SerializeField] private TMP_InputField m_IpInput;
    [SerializeField] private TMP_InputField m_PortInput;
    [SerializeField] private GameObject m_MultiplayerLevelChoosePanel;
    [SerializeField] private GameObject m_MultiplayerLevelChoosePanel_Exit;

    // 打包有bug，必须得在这拉个shi
    [SerializeField] private LevelSwitchConfig m_LevelSwitchConfig;

    public GameObject MultiplayerPanel
    {
        get { return m_MultiplayerPanel; }
    }

    bool isSingle = false;
    bool isServer = false;

    private void Awake()
    {
        m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
        m_StartMenu = gameObject;
        EventCenter.AddListener<string>(EventType.RequireChangeMultiScene,ChangeMultiScene);
    }
    
    private void ChangeMultiScene(string sceneName)
    {
        if (sceneName == null)
        {
            return;
        }

        SceneManager.LoadScene(sceneName);
        m_MultiplayerPanel.SetActive(true);
        m_MultiplayerLevelChoosePanel.SetActive(false);
        m_MultiplayerLevelChoosePanel_Exit.SetActive(false);
    }

    private void Start()
    {
        m_PauseMenu.gameObject.SetActive(false);
    }

    public void OnSinglePlayerStart()
    {
        UICanvas.Instance.isSingle = true;
        SaveLoadManager.SaveGame(new GameSaveFile() { currentSection = -1 });

        SceneManager.LoadScene(m_LevelSwitchConfig.basementName);
    }

    public void OnSinglePlayerContinue()
    {
        // Load Player Save file,Then Get Config to find load which level
        SaveLoadManager.LoadGame();
        LevelSwitchConfig levelSwitchConfig = m_LevelSwitchConfig;
        // 大关卡
        int level = SaveLoadManager.GlobalSaveFile.currentLevel;
        int section = SaveLoadManager.GlobalSaveFile.currentSection;

        string sectionName = section == -1
            ? levelSwitchConfig.basementName
            : levelSwitchConfig.levelStruct[level].sectionName[section];

        SceneManager.LoadScene(sectionName);
        
        isSingle = true;
        UICanvas.Instance.isSingle = true;
    }

    public void OnMultiPlayerPanelEnter()
    {
        m_MultiplayerLevelChoosePanel.SetActive(true);
        m_MultiplayerLevelChoosePanel_Exit.SetActive(true);
        // SceneManager.LoadScene("PvELevel");
        // m_MultiplayerPanel.SetActive(true);
    }

    public void OnMultiPlayerPanelExit()
    {
        m_MultiplayerLevelChoosePanel.SetActive(false);
        m_MultiplayerLevelChoosePanel_Exit.SetActive(false);
    }

    public void OnMultiPlayerTestRoomEnter()
    {
        SceneManager.LoadScene("MultiPlayerTestRoom");
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
        
        // 发事件来切换音乐
        EnvironmentAudioManager.Instance.WaveChangeSceneMusic(EnvironmentAudioManager.Instance.m_SceneEvironmentAudioSettingDic["StartMenu"].m_AudioClip);
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
        UICanvas.Instance.isSingle = false;
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
        UICanvas.Instance.isSingle = false;
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
        if (isSingle && NetworkClient.isConnected && NetworkClient.ready && NetworkClient.localPlayer != null)
        {
            //PlayerController.PlayerControllers[0].CmdStartGame();
            // isSingle = false;
            m_MultiplayerPanel.SetActive(false);
            m_StartMenu.SetActive(false);
            m_PropertiesUI.SetActive(true);
        }

        if (isSingle == false && !NetworkClient.isConnected && !NetworkServer.active)
        {
            m_NetworkStatus.SetDetail($"Connecting to {m_Manager.networkAddress}..");
        }

        if (isSingle == false && (NetworkClient.isConnected || NetworkServer.active))
        {
            if (isServer)
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