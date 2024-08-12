using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiePanel : MonoBehaviour
{
    [Tooltip("重开按钮")]
    [SerializeField]
    private GameObject m_Retry;

    public void OnBackToStartMenu()
    {
        if (NetworkClient.localPlayer != null)
        {

            UICanvas.Instance.BackToStartMenu();
        }
    }

    public void OnRetryClicked()
    {

    }

    public void SetRetryButtonVisiable(bool visiable)
    {
        m_Retry.SetActive(visiable);
    }
}
