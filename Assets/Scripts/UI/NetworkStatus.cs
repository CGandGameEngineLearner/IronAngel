using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkStatus : MonoBehaviour
{
    private GameObject m_NetworkStatusPanel;
    private NetworkManager m_Manager;
    [SerializeField]
    private TextMeshProUGUI m_Detail;
    private void Awake()
    {
        m_NetworkStatusPanel = gameObject;
        m_Manager = GameObject.FindAnyObjectByType<NetworkManager>();
    }

    public void SetDetail(string detail)
    {
        m_Detail.text = detail;
    }

    public void OnCancelConnecting()
    {
        m_Manager.StopClient();
        m_NetworkStatusPanel.SetActive(false);
    }
}
