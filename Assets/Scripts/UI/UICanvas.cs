using System;
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

    [SerializeField]
    private PlotPanel m_PlotPanel;

    public PlotPanel PlotPanel { get { return m_PlotPanel; } }

    [SerializeField]
    private TipsPanel m_TipsPanel;


    [SerializeField]
    private Texture2D m_Aim_1;
    [SerializeField]
    private Texture2D m_Aim_2;

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
            m_DiePanel.SetRetryButtonVisiable(isSingle);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param> 目标文字
    /// <param name="time"></param> 用多久出现
    /// <param name="duration"></param> 持续多久
    /// <param name="isEnd"></param> 是否是最后一句，是的话这句结束后文字面板会隐藏
    public void SetPlotText(string text, float time, float duration, bool isEnd = false)
    {
        m_PlotPanel.gameObject.SetActive(true);
        m_PlotPanel.SetText(text, time, duration, isEnd);
    }

    public bool IsPauseMenuActive()
    {
        return m_PauseMenu.gameObject.active;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param> 文字
    /// <param name="duration"></param> 持续时间
    public void SetTips(string text, float duration)
    {
        m_TipsPanel.gameObject.SetActive(true);
        m_TipsPanel.SetTips(text, duration);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu(!isPause);
            isPause = !isPause;
        }
        if(Input.GetMouseButton(0))
        {
            Cursor.SetCursor(m_Aim_2, new Vector2(0, 30), CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(m_Aim_1, new Vector2(0, 30), CursorMode.ForceSoftware);
        }
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<GameObject>(EventType.PlayerDied, ShowDirPanel);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameObject>(EventType.PlayerDied, ShowDirPanel);
    }
}
