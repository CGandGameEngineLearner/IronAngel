using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Mirror.BouncyCastle.Security;


public class PlotPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_PlotText;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param> 目标文字
    /// <param name="time"></param> 用多久出现
    /// <param name="duration"></param> 持续多久
    /// <param name="isEnd"></param> 是否是最后一句，是的话这句结束后文字面板会隐藏
    public void SetText(string text, float time, float duration, bool isEnd = false)
    {
        CancelInvoke();
        m_PlotText.DOKill();
        m_PlotText.text = "";
        m_PlotText.alpha = 1;
        m_PlotText.DOText(text, time).OnComplete(() =>
        {
            if(isEnd)
            {
                Invoke("HideTextPanel", duration);
            }
            else
            {
                Invoke("SetTextEmpty", duration);
            }
        });
    }
    private void SetTextEmpty()
    {
        m_PlotText.DOFade(0, 0.5f);
    }

    private void HideTextPanel()
    {
        m_PlotText.DOFade(0, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
