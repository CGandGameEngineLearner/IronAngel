using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesUI : MonoBehaviour
{
    [Tooltip("������Shader��������")]
    [SerializeField]
    private string m_RateName = "_Rate";
    [Tooltip("Ѫ��")]
    [SerializeField]
    private Image m_HP;
    [Tooltip("��������Ѫ��")]
    [SerializeField]
    private Image m_LeftHP;
    [Tooltip("��������Ѫ��")]
    [SerializeField]
    private Image m_RightHP;
    [Tooltip("��̴���")]
    private GameObject m_DashCount;

    private void Update()
    {
        if (NetworkClient.localPlayer != null)
        {
            var player = NetworkClient.localPlayer;
            var controller = player.GetComponent<PlayerController>();
            var propertity = player.GetComponent<BaseProperties>();
            m_HP.material.SetFloat(m_RateName, 1.0f * propertity.m_Properties.m_CurrentHP / propertity.m_Properties.m_BaseHP);
            m_LeftHP.material.SetFloat(m_RateName, 1.0f * propertity.m_Properties.m_LeftHandWeaponCurrentHP / propertity.m_Properties.m_LeftHandWeaponHP);
            m_RightHP.material.SetFloat(m_RateName, 1.0f * propertity.m_Properties.m_RightHandWeaponCurrentHP / propertity.m_Properties.m_RightHandWeaponHP);
        }
    }
}
