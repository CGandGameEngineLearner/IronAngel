using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShieldCollisionReceiver : NetworkBehaviour
{
    [Tooltip("�ܵ�����")]
    public ShieldType m_ShieldType = ShieldType.Armor;
    [Tooltip("��������ɫ�Ļ����Ƿֿ�����ģ���������Ƿֿ�����ĵ�������ֵ")]
    public int m_SubArmor = 100;

    private bool m_IsOverallArmor = true;
    private AmmunitionCollisionReceiver m_AmmunitionCollisionReceiver;
    //���׼���ϵ��
    private float m_DamageReductionCoefficient;

    private void Awake()
    {
        var setting = GameObject.FindAnyObjectByType<GlobalSetting>().GetComponent<GlobalSetting>();
        m_DamageReductionCoefficient = setting._DamageReductionCoefficient;

        m_AmmunitionCollisionReceiver = transform.parent.GetComponent<AmmunitionCollisionReceiver>();
#if UNITY_EDITOR
        if (m_AmmunitionCollisionReceiver == null)
        {
            Debug.LogError("����������������Ҫ�Ǻ��Ľ����˺���ײ�ű������������");
        }
#endif
        m_IsOverallArmor = m_AmmunitionCollisionReceiver.m_IsOverallArmor;

        m_AmmunitionCollisionReceiver.m_Shields.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ammunitionFactory = WeaponSystemCenter.GetAmmunitionFactory();
        var ammunitionHandle = ammunitionFactory.GetAmmunitionHandle(collision.gameObject);
        if (ammunitionHandle == null)
        {
#if UNITY_EDITOR
            Debug.Log("��ѯ���������ҩ��Handle,�ӵ�����Ϊ" + collision.gameObject);
#endif
            return;
        }
        if (ammunitionHandle.launcherCharacter == transform.parent.gameObject)
        {
            return;
        }
        CalculateDamage(ammunitionHandle.ammunitionConfig);
        ammunitionFactory.UnRegisterAmmunition(collision.gameObject);
    }


    [ServerCallback]
    private void CalculateDamage(AmmunitionConfig config)
    {
        if(m_IsOverallArmor)
        {
            m_AmmunitionCollisionReceiver.CalculateDamage(config);
        }
        else
        {

        }
    }

    [ClientRpc]
    private void RPCBroadcastDamage(Properties properties)
    {

    }
}

public enum ShieldType
{
    Armor,
    Energy,
}
