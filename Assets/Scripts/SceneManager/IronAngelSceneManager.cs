using System;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class IronAngelSceneManager : NetworkBehaviour
{
    public static IronAngelSceneManager Instance { get; private set; }
    private NetworkManager m_Manager;
    
    private void Awake()
    {
        Instance = this;
        
        m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
        
        
    }

    public void LoadScene(string sceneName)
    {
        PlayerController.PlayerControllers[0].LoadScene(sceneName);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
            m_Manager.StartHost();
        if ( NetworkClient.isConnected && NetworkClient.ready && NetworkClient.localPlayer != null)
        {
            PlayerController.PlayerControllers[0].CmdStartGame();
        }
    }
    
    public void OnDestroy()
    {
        if (NetworkServer.active || NetworkClient.isConnected)
        {    
            m_Manager.StopClient();
            m_Manager.StopHost();
            m_Manager.StopServer();
        }
    }
}