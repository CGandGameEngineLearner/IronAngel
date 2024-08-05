using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using LogicState;


[RequireComponent(typeof(BaseProperties))]
[RequireComponent (typeof(LogicStateManager))]
[RequireComponent(typeof(BoxCollider2D))]
public class AmmunitionCollisionReceiver : NetworkBehaviour
{
    [Tooltip("所有的护甲是否是一起计算")]
    public bool m_IsOverallArmor = true;

    public List<ShieldCollisionReceiver> m_Shields = new List<ShieldCollisionReceiver>();
    //护甲减伤系数
    private float m_DamageReductionCoefficient;
    public BaseProperties m_Properties;
    // 能量盾的特殊Tag
    public List<SpecialAtkType> m_specialAtkTypes = new List<SpecialAtkType>();
    private BoxCollider2D m_Collider;
    private LogicStateManager m_LogicStateManager;

    public WeaponCollisionReceiver m_LeftWeapon;
    public WeaponCollisionReceiver m_RightWeapon;

    private void Start()
    {
        var setting = GameObject.FindAnyObjectByType<GlobalSetting>().GetComponent<GlobalSetting>();
        m_DamageReductionCoefficient = setting._DamageReductionCoefficient;

        m_Properties = GetComponent<BaseProperties>();
        m_LogicStateManager = GetComponent<LogicStateManager>();

        m_Properties.m_Properties.m_CurrentArmor = 0;

        // 计算总体护甲值
        // 数值为子物体的所有分块的护甲值总和
        if (m_IsOverallArmor)
        {
            int armor = 0;
            foreach (var shield in m_Shields)
            {
                armor += shield.m_SubArmor;
            }
            m_Properties.m_Properties.m_Armor = armor;
            m_Properties.m_Properties.m_CurrentArmor = armor;
        }

        m_Properties.m_Properties.m_RightHandWeaponHP = WeaponSystemCenter.GetWeaponConfig(m_Properties.m_Properties.m_RightHandWeapon).weaponHp;
        m_Properties.m_Properties.m_LeftHandWeaponHP = WeaponSystemCenter.GetWeaponConfig(m_Properties.m_Properties.m_LeftHandWeapon).weaponHp;

        m_Collider = GetComponent<BoxCollider2D>();
    }



    /// <summary>
    /// 应该只有服务端上的物体会接收碰撞
    /// Comment:两边都会接受碰撞
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ammunitionFactory = WeaponSystemCenter.GetAmmunitionFactory();
        var ammunitionHandle = ammunitionFactory.GetAmmunitionHandle(collision.gameObject);
        if (ammunitionHandle==null)
        {
#if UNITY_EDITOR
            //Debug.Log("查询不到这个弹药的Handle,子弹对象为"+collision.gameObject);
#endif
            return;
        }

        var launcherCharacterProperties = ammunitionHandle.launcherCharacter.GetComponent<BaseProperties>();
        if (launcherCharacterProperties == null)
        {
            return;
        }

        var launcherCamp = launcherCharacterProperties.m_Properties.m_Camp;
        
        if (IsBulletFromOwnCamp(ammunitionHandle, launcherCamp))
        {
            return;
        }
        

        if(TryGetComponent<BaseProperties>(out var prop) == false)
        {
#if UNITY_EDITOR
            //Debug.LogWarning("游戏物体 ：" + gameObject.name + "没有属性值");
#endif
            return;
        }
        
        NoticeDamage(ammunitionHandle.launcherCharacter);
        CalculateDamage(ammunitionHandle.ammunitionConfig, m_Properties.m_Properties.m_CurrentArmor, collision.ClosestPoint(new Vector2(transform.position.x, transform.position.y) + m_Collider.offset));
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
    /// armor:受击部位的护甲值
    /// Pos:受击位置
    [ServerCallback]
    public void CalculateDamage(AmmunitionConfig config, int armor, Vector2 Pos)
    {
        var m_Properties = GetComponent<BaseProperties>();

        // 读取子弹上的Buff并且加入LogicStateManager
        NoticeBuff(config.m_EffectBuff);

        int damage = config.m_Damage;

        // 有能量护盾
        // 直接结算
        if (m_Properties.m_Properties.m_EnergyShieldCount > 0)
        {
            // 特殊子弹对能量盾的效果
            foreach(var type in m_specialAtkTypes)
            {
                // 如果有对不上的type就认为是没有特殊效果的子弹
                // 只会有对能量盾减一的效果
                if(config.m_specialAtkTypes.Contains(type) == false)
                {
                    m_Properties.m_Properties.m_EnergyShieldCount--;
                    RPCBroadcastDamage(m_Properties.m_Properties);
                    return;
                }
            }
            // 如果包含了能量盾所有的type(哪怕是子弹的Type种类数量比护盾的多)
            // 都认为可以直接移除能量盾
            if(m_specialAtkTypes.Count > 0)
            {
                m_Properties.m_Properties.m_EnergyShieldCount = 0;
            }
            // 下面的结算能量盾方式是普通的减一
            m_Properties.m_Properties.m_EnergyShieldCount--;
            RPCBroadcastDamage(m_Properties.m_Properties);
            return;
        }

        

        // 护甲大于0才进行减伤计算
        // 下面两句的计算顺序不能对换
        m_Properties.m_Properties.m_CurrentArmor -= damage;
        armor -= damage;
        damage = armor + damage >= 0 ? (int)(damage * (1 - m_DamageReductionCoefficient)) : damage;

        // 两边的武器血条还没有考虑
        // 分别计算受击位置和两个手部碰撞体和核心碰撞体的距离
        // 取最短的距离作为受击部位
        float leftDis = Vector2.Distance(new Vector2(m_LeftWeapon.transform.position.x + m_LeftWeapon.m_Collider.offset.x, m_LeftWeapon.transform.position.y + m_LeftWeapon.m_Collider.offset.y), Pos);
        float rightDis = Vector2.Distance(new Vector2(m_RightWeapon.transform.position.x + m_RightWeapon.m_Collider.offset.x, m_RightWeapon.transform.position.y + m_RightWeapon.m_Collider.offset.y), Pos);
        float coreDis = Vector2.Distance(new Vector2(transform.position.x + m_Collider.offset.x, transform.position.y + m_Collider.offset.y), Pos);
        // 击中左手
        if(leftDis < rightDis && leftDis < coreDis && m_Properties.m_Properties.m_LeftHandWeaponHP > 0)
        {
            m_Properties.m_Properties.m_LeftHandWeaponHP -= damage;
        }
        // 击中右手
        else if(rightDis < coreDis && rightDis < leftDis && m_Properties.m_Properties.m_RightHandWeaponHP > 0)
        {
            m_Properties.m_Properties.m_RightHandWeaponHP -= damage;
        }
        // 击中核心
        else
        {
            m_Properties.m_Properties.m_CurrentHP -= damage;
        }
        

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

            if (m_Properties.m_Properties.m_DropWeapon_CharacterDied)
            {
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_LeftHandWeapon, transform.position);
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_RightHandWeapon, transform.position);
            }
        }
        // 玩家所有护甲损失
        if(m_IsOverallArmor && m_Properties.m_Properties.m_CurrentArmor <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("玩家 ：" + gameObject.name + "损失所有护甲");
#endif
            foreach (var shield in m_Shields)
            {
                if(shield.m_ShieldType == ShieldType.Armor)
                    shield.gameObject.SetActive(false);
            }
        }
        // 玩家损失能量护盾
        if (m_Properties.m_Properties.m_EnergyShieldCount <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("玩家 ：" + gameObject.name + "损失能量护盾");
#endif
            foreach (var shield in m_Shields)
            {
                if (shield.m_ShieldType == ShieldType.Energy)
                    shield.gameObject.SetActive(false);
            }
        }
        // 玩家左右手部位损失
        if(m_Properties.m_Properties.m_LeftHandWeaponHP <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("玩家 ：" + gameObject.name + "丢失左手");
#endif
            m_LeftWeapon.gameObject.SetActive(false);

            if (m_Properties.m_Properties.m_DropWeapon_WeaponDestroy)
            {
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_LeftHandWeapon, transform.position);
            }
        }
        if(m_Properties.m_Properties.m_RightHandWeaponHP <= 0)
        {
#if UNITY_EDITOR
            Debug.Log("玩家 ：" + gameObject.name + "丢失右手");
#endif
            m_RightWeapon.gameObject.SetActive(false);

            if (m_Properties.m_Properties.m_DropWeapon_WeaponDestroy)
            {
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_RightHandWeapon, transform.position);
            }
        }
    }

    public bool IsBulletFromOwnCamp(AmmunitionHandle ammunitionHandle, ECamp launcherCamp)
    {
        return ammunitionHandle.launcherCharacter == gameObject || launcherCamp == m_Properties.m_Properties.m_Camp;
    }

    /// <summary>
    /// 向DamageSensor通知受到攻击了
    /// </summary>
    /// <param name="attacker"></param>
    public void NoticeDamage(GameObject attacker)
    {
        var damageSensor = GetComponent<DamageSensor>();
        if (damageSensor != null)
        {
            damageSensor.PutPerceiveGameObject(attacker);
        }
    }

    public void NoticeBuff(List<BuffStruct> buffs)
    {
        foreach(BuffStruct buff in buffs)
        {
            switch(buff.m_EffectBuff)
            {
                case ELogicState.SpeedModifier:
                    {
                        /*if(m_LogicStateManager.IncludeState(ELogicState.SpeedModifier))
                        {
                            Debug.Log("减速还剩：" + m_LogicStateManager.GetStateDuration(ELogicState.SpeedModifier));
                        }*/
                        
                        // 如果本来没有减速并且能够挂上减速Buff
                        if (m_LogicStateManager.IncludeState(ELogicState.SpeedModifier) == false && m_LogicStateManager.AddState(ELogicState.SpeedModifier))
                        {
                            Debug.Log("减速Buff的时间" + buff.m_Duration);
                            Debug.Log(m_LogicStateManager.SetStateDuration(ELogicState.SpeedModifier, buff.m_Duration));
                            Debug.Log(gameObject.name + "减速还剩：" + m_LogicStateManager.GetStateDuration(ELogicState.SpeedModifier));
                            EventCenter.Broadcast<GameObject, float, bool>(EventType.Buff_Speed, gameObject, buff.m_Number, true);
                        }
                        break;
                    }
                case ELogicState.StunModifier:
                    {
                        m_LogicStateManager.AddState(ELogicState.StunModifier);
                        break;
                    }
            }
        }
    }
}
