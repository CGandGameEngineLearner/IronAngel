using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(BaseProperties))]
public class AmmunitionCollisionReceiver : NetworkBehaviour
{
    [Tooltip("所有的护甲是否是一起计算")]
    public bool m_IsOverallArmor = true;

    public List<ShieldCollisionReceiver> m_Shields = new List<ShieldCollisionReceiver>();
    //护甲减伤系数
    private float m_DamageReductionCoefficient;
    private BaseProperties m_Properties;



    private void Start()
    {
        var setting = GameObject.FindAnyObjectByType<GlobalSetting>().GetComponent<GlobalSetting>();
        m_DamageReductionCoefficient = setting._DamageReductionCoefficient;

        m_Properties = GetComponent<BaseProperties>();

        // 计算总体护甲值
        // 数值为子物体的所有分块的护甲值总和
        int armor = 0;
        foreach (var shield in m_Shields)
        {
            armor += shield.m_SubArmor;
        }
        m_Properties.m_Properties.m_Armor = armor;
        m_Properties.m_Properties.m_CurrentArmor = armor;
    }



    /// <summary>
    /// 应该只有服务端上的物体会接收碰撞
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ammunitionFactory = WeaponSystemCenter.GetAmmunitionFactory();
        var ammunitionHandle = ammunitionFactory.GetAmmunitionHandle(collision.gameObject);
        if (ammunitionHandle==null)
        {
#if UNITY_EDITOR
            Debug.Log("查询不到这个弹药的Handle,子弹对象为"+collision.gameObject);
#endif
            return;
        }

        if (ammunitionHandle.launcherCharacter==gameObject)
        {
            return;
        }

        if(TryGetComponent<BaseProperties>(out var prop) == false)
        {
#if UNITY_EDITOR
            Debug.LogWarning("游戏物体 ：" + gameObject.name + "没有属性值");
#endif
            return;
        }
        CalculateDamage(ammunitionHandle.ammunitionConfig);
        ammunitionFactory.UnRegisterAmmunition(collision.gameObject);
    }

    /// <summary>
    /// 获取受击者身上的属性进行伤害计算
    /// 计算优先级：
    ///     先计算能量盾数量
    ///     再计算具体护甲
    ///     
    ///     护甲减伤计算公式：
    ///     {
    ///         护甲值减去实际伤害数值
    ///         内部受到伤害为（1 - 护甲减伤系数）* 实际伤害
    ///     }
    /// </summary>
    ///  config: 子弹的配置表，用于计算伤害 
    ///
    [ServerCallback]
    public void CalculateDamage(AmmunitionConfig config)
    {
        var m_Properties = GetComponent<BaseProperties>();
        int damage = config.m_Damage;

        // 护甲大于0才进行减伤计算
        // 下面两句的计算顺序不能对换
        m_Properties.m_Properties.m_CurrentArmor -= damage;
        damage = m_Properties.m_Properties.m_CurrentArmor + damage >= 0 ? (int)(damage * (1 - m_DamageReductionCoefficient)) : damage;

        // 两边的武器血条还没有考虑
        m_Properties.m_Properties.m_CurrentHP -= damage;

        RPCBroadcastDamage(m_Properties.m_Properties);
    }


    /// <summary>
    /// RPC直接通知属性更改
    /// </summary>
    /// <param name="properties"></param> 受击者更新后的属性
    [ClientRpc]
    private void RPCBroadcastDamage(Properties properties)
    {
        m_Properties.m_Properties = properties;
        // 玩家死亡
        if(m_Properties.m_Properties.m_CurrentHP <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("玩家 ：" + gameObject.name + "死亡");
#endif
            gameObject.SetActive(false);
        }
        // 玩家所有护甲损失
        if(m_IsOverallArmor && m_Properties.m_Properties.m_CurrentArmor <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("玩家 ：" + gameObject.name + "损失所有护甲");
#endif
            foreach (var shield in m_Shields)
            {
                shield.gameObject.SetActive(false);
            }
        }
    }
}