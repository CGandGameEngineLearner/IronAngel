using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesUI : MonoBehaviour
{
    [Tooltip("进度条Shader属性名字")]
    [SerializeField]
    private string m_RateName = "_Rate";
    [Tooltip("血量")]
    [SerializeField]
    private Image m_HP;
    [Tooltip("左手武器血量")]
    [SerializeField]
    private Image m_LeftHP;
    [Tooltip("右手武器血量")]
    [SerializeField]
    private Image m_RightHP;
    [Tooltip("冲刺次数")]
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
