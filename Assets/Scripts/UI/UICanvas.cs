using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    private static UICanvas m_Instance;
    public static UICanvas Instance
    {
        get { return m_Instance; }
        private set { m_Instance = value; }
    }
    [SerializeField]
    private PauseMenu m_PauseMenu;
    public PauseMenu PauseMenu { get { return m_PauseMenu; } }

    [SerializeField]
    private DiePanel m_DiePanel;

    public DiePanel DiePanel { get { return m_DiePanel; } }

    [SerializeField]
    private PropertiesUI m_PropertiesUI;
    public PropertiesUI PropertiesUI { get { return m_PropertiesUI; } }

    public bool isSingle = true;


    private void Start()
    {
        m_Instance = this;
        EventCenter.AddListener<GameObject>(EventType.PlayerDied, ShowDirPanel);
    }
    private void ShowDirPanel(GameObject player)
    {
        if(NetworkClient.localPlayer.gameObject == player)
        {
            m_DiePanel.gameObject.SetActive(true);
            m_PropertiesUI.gameObject.SetActive(false);
            m_PauseMenu.gameObject.SetActive(false);
        }
        
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener< GameObject>(EventType.PlayerDied, ShowDirPanel);
    }
}
