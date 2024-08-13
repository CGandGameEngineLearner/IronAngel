using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipsPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Tip;

    public void SetTips(string tip, float duration)
    {
        m_Tip.DOKill();
        m_Tip.text = tip;
        m_Tip.DOFade(1, duration / 4).OnComplete(() =>
        {
            m_Tip.DOFade(0, duration / 4).OnComplete(() =>
            {
                m_Tip.DOFade(1, duration / 4).OnComplete(() =>
                {
                    m_Tip.DOFade(0, duration / 4).OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    });
                });
            });
        });
    }
}
