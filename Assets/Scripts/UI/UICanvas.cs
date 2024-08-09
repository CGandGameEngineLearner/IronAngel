using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private StartMenu m_StartMenu;
    public StartMenu StartMenu {  get { return m_StartMenu; } }

    public bool isSingle = true;
    bool isPause = false;


    private void Start()
    {
        if(m_Instance == null)
        {
            Instance = this;
        }
        else if(m_Instance != this)
        {
            Destroy(gameObject);
        }
        EventCenter.AddListener<GameObject>(EventType.PlayerDied, ShowDirPanel);
        DontDestroyOnLoad(gameObject);
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

    public void BackToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");

        m_StartMenu.OnBackToStartMenu();
        m_StartMenu.MultiplayerPanel.SetActive(false);
        m_DiePanel.gameObject.SetActive(false);
        m_PropertiesUI.gameObject.SetActive(false);
        m_PauseMenu.gameObject.SetActive(false);
        m_StartMenu.gameObject.SetActive(true);
    }

    public void ShowPauseMenu(bool val)
    {
        if(val)
        {
            //m_PropertiesUI.gameObject.SetActive(false);
            m_PauseMenu.gameObject.SetActive(true);
        }
        else
        {
            m_PropertiesUI.gameObject.SetActive(true);
            m_PauseMenu.gameObject.SetActive(false);
        }
    }

    public void OnPauseMenuEvent()
    {
        ShowPauseMenu(IsPauseMenuActive());
    }

    public bool IsPauseMenuActive()
    {
        return m_PauseMenu.gameObject.active;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu(!isPause);
            isPause = !isPause;
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener< GameObject>(EventType.PlayerDied, ShowDirPanel);
    }
}
