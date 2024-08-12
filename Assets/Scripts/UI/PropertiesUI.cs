using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    [Tooltip("��������ʣ���ӵ�")]
    [SerializeField]
    private TextMeshProUGUI m_LeftMag;
    
    [Tooltip("��������Ѫ��")]
    [SerializeField]
    private Image m_RightHP;
    
    [Tooltip("��������ʣ���ӵ�")]
    [SerializeField]
    private TextMeshProUGUI m_RightMag;
    
    [Tooltip("��̴���")]
    private GameObject m_DashCount;
    
    [Tooltip("����")]
    [SerializeField]
    private Image m_Energy;
    
    [Tooltip("�������")]
    [SerializeField]
    private int m_MaxEnergy = 250;
    
    [Tooltip("����������")]
    [SerializeField]
    private Image m_DashPanel;
    
    [Tooltip("�������UI,��˳���Ͻ���")]
    [SerializeField]
    private List<Image> m_DashCountImages = new List<Image>();
    
    [Tooltip("���UIƫ��")]
    [SerializeField]
    private Vector2 m_DashUIOffset;
    
    [Tooltip("����������")]
    [SerializeField]
    private TextMeshProUGUI m_LeftWeaponName;
    
    [Tooltip("����������")]
    [SerializeField]
    private TextMeshProUGUI m_RightWeaponName;

    [Tooltip("����ͼ")]
    [SerializeField]
    private List<Image> m_Powers = new List<Image>();
    [Tooltip("�Ҷ�ͼ")]
    [SerializeField]
    private Color m_GrayColor;
    

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
            if (leftWeapon)
            {
                m_LeftMag.text = leftWeapon.GetComponent<WeaponInstance>().GetCurrentMag() >= 0 ? leftWeapon.GetComponent<WeaponInstance>().GetCurrentMag().ToString() : "0";
                m_LeftWeaponName.text = leftWeapon.GetComponent<WeaponInstance>().GetWeaponName();
            }
            else
            {
                m_LeftMag.text = "0";
                m_LeftWeaponName.text = "";
            }
            if (rightWeapon)
            {
                m_RightMag.text = rightWeapon.GetComponent<WeaponInstance>().GetCurrentMag() >= 0 ? rightWeapon.GetComponent<WeaponInstance>().GetCurrentMag().ToString() : "0";
                m_RightWeaponName.text = rightWeapon.GetComponent<WeaponInstance>().GetWeaponName();
            }
            else
            {
                m_RightMag.text = "0";
                m_RightWeaponName.text = "";
            }

            var playerPosOnScreen = Camera.main.WorldToScreenPoint(controller.Player.GetPlayerPosition());
            Vector2 dashCountUI = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), playerPosOnScreen, null, out dashCountUI);
            m_DashPanel.rectTransform.localPosition = dashCountUI + m_DashUIOffset;
            for (int i = 0; i < m_DashCountImages.Count; i++)
            {
                if (controller.Player.GetDashCount() >= i + 1)
                {
                    m_DashCountImages[i].gameObject.SetActive(true);
                }
                else
                {
                    m_DashCountImages[i].gameObject.SetActive(false);
                }

            }
            var power = controller.m_Power;
            for (int i = 0; i < power.Count; i++)
            {
                if(propertity.m_Properties.m_Energy >= power[i])
                {
                    m_Powers[i].color = Color.white;
                }
                else
                {
                    m_Powers[i].color = m_GrayColor;
                }
            }
        }
    }
}
