using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [Tooltip("左手武器剩余子弹")]
    [SerializeField]
    private TextMeshProUGUI m_LeftMag;
    [Tooltip("右手武器血量")]
    [SerializeField]
    private Image m_RightHP;
    [Tooltip("右手武器剩余子弹")]
    [SerializeField]
    private TextMeshProUGUI m_RightMag;
    [Tooltip("冲刺次数")]
    private GameObject m_DashCount;
    [Tooltip("能量")]
    [SerializeField]
    private Image m_Energy;
    [Tooltip("最大能量")]
    private int m_MaxEnergy = 100;
    [Tooltip("冲刺数量面板")]
    [SerializeField]
    private Image m_DashPanel;
    [Tooltip("冲刺数量UI,按顺序拖进来")]
    [SerializeField]
    private List<Image> m_DashCountImages = new List<Image>();

    private void Update()
    {
        if (NetworkClient.localPlayer != null)
        {
            var player = NetworkClient.localPlayer;
            var controller = player.GetComponent<PlayerController>();
            var propertity = player.GetComponent<BaseProperties>();
            m_HP.material.SetFloat(m_RateName, 1.0f * propertity.m_Properties.m_CurrentHP / propertity.m_Properties.m_BaseHP);
            m_LeftHP.material.SetFloat(m_RateName, propertity.m_Properties.m_LeftHandWeaponHP > 0 ? 1.0f * propertity.m_Properties.m_LeftHandWeaponCurrentHP / propertity.m_Properties.m_LeftHandWeaponHP : 0);
            m_RightHP.material.SetFloat(m_RateName, propertity.m_Properties.m_RightHandWeaponHP > 0 ? 1.0f * propertity.m_Properties.m_RightHandWeaponCurrentHP / propertity.m_Properties.m_RightHandWeaponHP : 0);
            m_Energy.material.SetFloat(m_RateName, propertity.m_Properties.m_Energy * 1.0f / m_MaxEnergy);
            var leftWeapon = controller.Player.GetPlayerLeftHandWeapon();
            var rightWeapon = controller.Player.GetPlayerRightHandWeapon();
            if(leftWeapon)
            {
                m_LeftMag.text = leftWeapon.GetComponent<WeaponInstance>().GetCurrentMag().ToString();
            }
            else
            {
                m_LeftMag.text = "0";
            }
            if(rightWeapon)
            {
                m_RightMag.text = rightWeapon.GetComponent<WeaponInstance>().GetCurrentMag().ToString();
            }
            else
            {
                m_RightMag.text = "0";
            }

            var playerPosOnScreen = Camera.main.WorldToScreenPoint(controller.Player.GetPlayerPosition());
            Vector2 output = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), playerPosOnScreen, transform.parent.GetComponent<Canvas>().worldCamera,out output);
            m_DashPanel.rectTransform.localPosition = output;
        }
    }
}
