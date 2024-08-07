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

    private void Start()
    {
        m_Instance = this;
    }

    public bool IsPauseMenuActive()
    {
        return m_PauseMenu.gameObject.active;
    }

    public void SetPauseMenuActive(bool val)
    {
        m_PauseMenu.gameObject.SetActive(val);
    }
}
