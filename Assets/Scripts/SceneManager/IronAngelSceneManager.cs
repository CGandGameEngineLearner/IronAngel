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
            gameObject.SetActive(false);
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


    // [ServerCallback]
    // public void GivePlayerWeapon(WeaponType leftWeaponType, WeaponType rightWeaponType)
    // {
    //     GameObject leftWeapon = null;
    //     GameObject rightWeapon = null;
    //     if (leftWeaponType != WeaponType.None)
    //     {
    //         leftWeapon = WeaponSystemCenter.Instance.GetWeapon(leftWeaponType);
    //         RpcGivePlayerLeftWeapon(leftWeapon);
    //     }
    //
    //     if (rightWeaponType != WeaponType.None)
    //     {
    //         rightWeapon = WeaponSystemCenter.Instance.GetWeapon(rightWeaponType);
    //         RpcGivePlayerRightWeapon(rightWeapon);
    //     }
    //     
    // }
    //
    // [ClientRpc]
    // private void RpcGivePlayerLeftWeapon(GameObject leftWeapon)
    // {
    //     PlayerController.PlayerControllers[0].Player.SetPlayerLeftHandWeapon(leftWeapon);
    //     
    // }
    //
    // [ClientRpc]
    // private void RpcGivePlayerRightWeapon(GameObject rightWeapon)
    // {
    //     PlayerController.PlayerControllers[0].Player.SetPlayerRightHandWeapon(rightWeapon);
    // }
}