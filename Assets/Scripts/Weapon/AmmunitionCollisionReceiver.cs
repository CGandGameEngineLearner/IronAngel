using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmmunitionCollisionReceiver : NetworkBehaviour
{
    //护甲减伤系数
    private float m_DamageReductionCoefficient;

    private void Start()
    {
        var setting = GameObject.FindAnyObjectByType<GlobalSetting>().GetComponent<GlobalSetting>();
        m_DamageReductionCoefficient = setting._DamageReductionCoefficient;
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
            Debug.Log("查询不到这个弹药的Handle,子弹对象为"+collision.gameObject);
            return;
        }

        if (ammunitionHandle.launcherCharacter==gameObject)
        {
            return;
        }

        if(TryGetComponent<BaseProperties>(out var prop) == false)
        {
            Debug.LogWarning("游戏物体 ：" + gameObject.name + "没有属性值");
            return;
        }
        Hit(ammunitionHandle.ammunitionConfig);
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
    private void Hit(AmmunitionConfig config)
    {
        var prop = GetComponent<BaseProperties>();
        int damage = config.m_Damage;

        // 护甲大于0才进行减伤计算
        // 下面两句的计算顺序不能对换
        prop.m_Properties.m_CurrentArmor -= damage;
        damage = prop.m_Properties.m_CurrentArmor + damage >= 0 ? (int)(damage * (1 - m_DamageReductionCoefficient)) : damage;
        
        // 两边的武器血条还没有考虑
        prop.m_Properties.m_CurrentHP -= damage;

        RPCBroadcastDamage(prop.m_Properties);
    }


    /// <summary>
    /// RPC直接通知属性更改
    /// </summary>
    /// <param name="properties"></param> 受击者更新后的属性
    [ClientRpc]
    public void RPCBroadcastDamage(Properties properties)
    {
        if(gameObject.TryGetComponent<BaseProperties>(out var prop))
        {
            prop.m_Properties = properties;
            if(prop.m_Properties.m_CurrentHP <= 0)
            {
                Debug.Log("玩家 ：" + gameObject.name + "死亡");
            }
        }
    }
}
