using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShieldCollisionReceiver : NetworkBehaviour
{
    [Tooltip("盾的类型")]
    public ShieldType m_ShieldType = ShieldType.Armor;
    [Tooltip("如果这个角色的护甲是分开计算的，这个属性是分开计算的单个护甲值")]
    public int m_SubArmor = 100;
    [Tooltip("这个是能量盾的特殊Tag，用于一击移除能量盾的,只有子弹和能量盾所有的Tag对上了才会一击移除")]
    public List<SpecialAtkType> m_specialAtkTypes = new List<SpecialAtkType>();

    private bool m_IsOverallArmor = true;
    private AmmunitionCollisionReceiver m_AmmunitionCollisionReceiver;
    //护甲减伤系数
    private float m_DamageReductionCoefficient;

    private void Awake()
    {
        var setting = GameObject.FindAnyObjectByType<GlobalSetting>().GetComponent<GlobalSetting>();
        m_DamageReductionCoefficient = setting._DamageReductionCoefficient;

        m_AmmunitionCollisionReceiver = transform.parent.GetComponent<AmmunitionCollisionReceiver>();
#if UNITY_EDITOR
        if (m_AmmunitionCollisionReceiver == null)
        {
            Debug.LogError("这个护甲组件物体需要是核心接收伤害碰撞脚本物体的子物体");
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
            Debug.Log("查询不到这个弹药的Handle,子弹对象为" + collision.gameObject);
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
        // 提交能量盾或者穿甲结算
        m_AmmunitionCollisionReceiver.CalculateDamage(config);
        // 类内计算分块的护甲
        if(m_IsOverallArmor == false && m_ShieldType == ShieldType.Armor)
        {
            int damage = config.m_Damage;
            RPCBroadcastDamage(damage);
        }
    }


    /// <summary>
    /// 这个是计算当前这块护甲的伤害
    /// </summary>
    /// <param name="damage"></param> 对当前护甲的伤害
    [ClientRpc]
    private void RPCBroadcastDamage(int damage)
    {
        m_SubArmor -= damage;
        if(m_SubArmor <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("护甲 ：" + gameObject.name + "被摧毁");
#endif
            gameObject.SetActive(false);
        }
    }
}

public enum ShieldType
{
    Armor,
    Energy,
}
