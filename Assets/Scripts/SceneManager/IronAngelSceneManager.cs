using System;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class IronAngelSceneManager : MonoBehaviour
{
    public static IronAngelSceneManager Instance { get; private set; }
    private NetworkManager m_Manager;

    private void Awake()
    {
        Instance = this;
        m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
    }

    private void Start()
    {
        m_Manager.StartHost();
    }

    public void LoadScene(string sceneName)
    {
        PlayerController.PlayerControllers[0].LoadScene(sceneName);
    }

    public void Update()
    {
        if (NetworkClient.isConnected && NetworkClient.ready && NetworkClient.localPlayer != null)
        {
            PlayerController.PlayerControllers[0].CmdStartGame();
            GameSaveFile gameSaveFile = SaveLoadManager.GlobalSaveFile;
            
            // 给玩家双手添加武器
            // GivePlayerWeapon(gameSaveFile.leftWeaponType, gameSaveFile.rightWeaponType);
            PlayerController.PlayerControllers[0].Player.GivePlayerWeapon(gameSaveFile.leftWeaponType, gameSaveFile.rightWeaponType);
            gameObject.SetActive(false);
            
            EventCenter.Broadcast(EventType.ChangeScene);
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