using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using LogicState;
using Mirror.BouncyCastle.Asn1.Mozilla;


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

    private GameObject m_Launcher;

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
        


        m_Collider = GetComponent<BoxCollider2D>();
        m_Properties.m_Properties.m_LeftHandWeaponCurrentHP = m_Properties.m_Properties.m_LeftHandWeaponHP;
        m_Properties.m_Properties.m_RightHandWeaponCurrentHP = m_Properties.m_Properties.m_RightHandWeaponHP;
        m_Properties.m_Properties.m_CurrentHP = m_Properties.m_Properties.m_BaseHP;


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
            return;
        }
        if (ammunitionHandle.launcherCharacter == null)
        {
            return;
        }
        m_Launcher = ammunitionHandle.launcherCharacter;
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
        if (m_LogicStateManager.IncludeState(ELogicState.PlayerDashing))
        {
            return;
        }
        DamageData data = new DamageData();
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
                    data.m_CurrentHP = m_Properties.m_Properties.m_CurrentHP;
                    data.m_CurrentArmor = m_Properties.m_Properties.m_CurrentArmor;
                    data.m_EnergyShieldCount = m_Properties.m_Properties.m_EnergyShieldCount;
                    data.m_LeftHandWeaponHP = m_Properties.m_Properties.m_LeftHandWeaponHP;
                    data.m_RightHandWeaponHP = m_Properties.m_Properties.m_RightHandWeaponHP;
                    RPCBroadcastDamage(data);
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
            data.m_CurrentHP = m_Properties.m_Properties.m_CurrentHP;
            data.m_CurrentArmor = m_Properties.m_Properties.m_CurrentArmor;
            data.m_EnergyShieldCount = m_Properties.m_Properties.m_EnergyShieldCount;
            data.m_LeftHandWeaponHP = m_Properties.m_Properties.m_LeftHandWeaponHP;
            data.m_RightHandWeaponHP = m_Properties.m_Properties.m_RightHandWeaponHP;
            RPCBroadcastDamage(data);
            return;
        }

        

        // 护甲大于0才进行减伤计算
        // 下面两句的计算顺序不能对换
        m_Properties.m_Properties.m_CurrentArmor -= damage;
        armor -= damage;
        damage = armor + damage >= 0 ? (int)(damage * (1 - m_DamageReductionCoefficient)) : damage;

        data.m_CurrentHP = m_Properties.m_Properties.m_CurrentHP;
        data.m_CurrentArmor = m_Properties.m_Properties.m_CurrentArmor;
        data.m_EnergyShieldCount = m_Properties.m_Properties.m_EnergyShieldCount;
        data.m_LeftHandWeaponHP = m_Properties.m_Properties.m_LeftHandWeaponCurrentHP;
        data.m_RightHandWeaponHP = m_Properties.m_Properties.m_RightHandWeaponCurrentHP;

        // 两边的武器血条还没有考虑
        // 分别计算受击位置和两个手部碰撞体和核心碰撞体的距离
        // 取最短的距离作为受击部位
        float leftDis = Vector2.Distance(new Vector2(m_LeftWeapon.transform.position.x + m_LeftWeapon.m_Collider.offset.x, m_LeftWeapon.transform.position.y + m_LeftWeapon.m_Collider.offset.y), Pos);
        float rightDis = Vector2.Distance(new Vector2(m_RightWeapon.transform.position.x + m_RightWeapon.m_Collider.offset.x, m_RightWeapon.transform.position.y + m_RightWeapon.m_Collider.offset.y), Pos);
        float coreDis = Vector2.Distance(new Vector2(transform.position.x + m_Collider.offset.x, transform.position.y + m_Collider.offset.y), Pos);
        // 击中左手
        if(leftDis < rightDis && leftDis < coreDis && m_Properties.m_Properties.m_LeftHandWeaponCurrentHP > 0)
        {
            data.m_LeftHandWeaponHP -= damage;
        }
        // 击中右手
        else if(rightDis < coreDis && rightDis < leftDis && m_Properties.m_Properties.m_RightHandWeaponCurrentHP > 0)
        {

            data.m_RightHandWeaponHP -= damage;
        }
        // 击中核心
        else
        {
            data.m_CurrentHP -= damage;
        }
        


        
        
        Debug.Log(data.m_LeftHandWeaponHP + " " + data.m_RightHandWeaponHP);
        RPCBroadcastDamage(data);
    }


    [ServerCallback]
    private void SetProp(DamageData data)
    {
        RPCBroadcastDamage(data);
    }

    /// <summary>
    /// RPC直接通知属性更改
    /// </summary>
    /// <param name="properties"></param> 受击者更新后的属性
    [ClientRpc]
    private void RPCBroadcastDamage(DamageData data)
    {
        m_Properties.m_Properties.m_CurrentHP = data.m_CurrentHP;
        m_Properties.m_Properties.m_CurrentArmor = data.m_CurrentArmor;
        m_Properties.m_Properties.m_EnergyShieldCount = data.m_EnergyShieldCount;
        m_Properties.m_Properties.m_LeftHandWeaponCurrentHP = data.m_LeftHandWeaponHP;
        m_Properties.m_Properties.m_RightHandWeaponCurrentHP = data.m_RightHandWeaponHP;
        // 被记录在WeaponInstance里的当前武器血量只有在玩家手上才会更改
        if(TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.Player.GetPlayerLeftHandWeapon()?.GetComponent<WeaponInstance>().SetWeaponCurrentHP(data.m_LeftHandWeaponHP);
            playerController.Player.GetPlayerRightHandWeapon()?.GetComponent<WeaponInstance>().SetWeaponCurrentHP(data.m_RightHandWeaponHP);
        }
        // 角色死亡
        if(m_Properties.m_Properties.m_CurrentHP <= 0)
        {
            if(gameObject.active)
            {
                m_Launcher.GetComponent<BaseProperties>().m_Properties.m_Energy += m_Properties.m_Properties.m_Energy;
            }
            
            gameObject.SetActive(false);
            EventCenter.Broadcast<GameObject>(EventType.CharacterDied,gameObject);
            if (m_Properties.m_Properties.m_DropWeapon_CharacterDied)
            {
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_LeftHandWeapon, transform.position);
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_RightHandWeapon, transform.position);
            }
            if(TryGetComponent<PlayerController>(out var controller))
            {
                EventCenter.Broadcast< GameObject>(EventType.PlayerDied,gameObject);
            }
        }
        // 角色所有护甲损失
        if(m_IsOverallArmor && m_Properties.m_Properties.m_CurrentArmor <= 0)
        {
            foreach (var shield in m_Shields)
            {
                if(shield.m_ShieldType == ShieldType.Armor)
                    shield.gameObject.SetActive(false);
            }
        }
        // 角色损失能量护盾
        if (m_Properties.m_Properties.m_EnergyShieldCount <= 0)
        {
            foreach (var shield in m_Shields)
            {
                if (shield.m_ShieldType == ShieldType.Energy)
                    shield.gameObject.SetActive(false);
            }
        }
        // 角色左右手部位损失
        if(m_Properties.m_Properties.m_LeftHandWeaponCurrentHP <= 0)
        {
            m_LeftWeapon.gameObject.SetActive(false);

            if (m_Properties.m_Properties.m_DropWeapon_WeaponDestroy)
            {
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_LeftHandWeapon, transform.position);
            }

            // 如果是角色的话就丢失武器
            if(TryGetComponent<PlayerController>(out var controller))
            {
                var weapon = controller.Player.DropPlayerLeftHandWeapon(transform.position);
                if(weapon)
                {
                    DestroyWeapon(weapon);
                }
            }
        }
        if(m_Properties.m_Properties.m_RightHandWeaponCurrentHP <= 0)
        {
            m_RightWeapon.gameObject.SetActive(false);

            if (m_Properties.m_Properties.m_DropWeapon_WeaponDestroy)
            {
                WeaponSystemCenter.Instance.SpawnWeapon(m_Properties.m_Properties.m_RightHandWeapon, transform.position);
            }

            // 如果是角色的话就丢失武器
            if (TryGetComponent<PlayerController>(out var controller))
            {
                var weapon = controller.Player.DropPlayerRightHandWeapon(transform.position);
                if (weapon)
                {
                    DestroyWeapon(weapon);
                }
            }
        }
    }

    public bool IsBulletFromOwnCamp(AmmunitionHandle ammunitionHandle, ECamp launcherCamp)
    {
        return ammunitionHandle.launcherCharacter == gameObject || launcherCamp == m_Properties.m_Properties.m_Camp;
    }

    /// <summary>
    /// 向AI的DamageSensor通知受到攻击了
    /// </summary>
    /// <param name="attacker"></param>
    [ServerCallback]
    public void NoticeDamage(GameObject attacker)
    {
        var damageSensor = GetComponent<DamageSensor>();
        if (damageSensor != null)
        {
            damageSensor.PutPerceiveGameObject(attacker);
        }
    }

    /// <summary>
    /// 根据子弹的Buff给受击对象
    /// </summary>
    /// <param name="buffs"></param>
    public void NoticeBuff(List<BuffStruct> buffs)
    {
        foreach(BuffStruct buff in buffs)
        {
            switch(buff.m_EffectBuff)
            {
                case ELogicState.SpeedModifier:
                    {
                        
                        // 如果本来没有减速并且能够挂上减速Buff
                        if (m_LogicStateManager.IncludeState(ELogicState.SpeedModifier) == false && m_LogicStateManager.AddState(ELogicState.SpeedModifier))
                        {
                            m_LogicStateManager.SetStateDuration(ELogicState.SpeedModifier, buff.m_Duration);
                            EventCenter.Broadcast<GameObject, float, bool>(EventType.Buff_Speed, gameObject, buff.m_Number, true);
                        }
                        break;
                    }
                case ELogicState.StunModifier:
                    {
                        // 如果本来没有眩晕并且能够挂上眩晕Buff
                        if(m_LogicStateManager.IncludeState(ELogicState.StunModifier) == false && m_LogicStateManager.AddState(ELogicState.StunModifier))
                        {
                            m_LogicStateManager.SetStateDuration(ELogicState.StunModifier, buff.m_Duration);
                            EventCenter.Broadcast<GameObject, bool>(EventType.Buff_Stun, gameObject, true);
                        }
                        break;
                    }
            }
        }
    }

    [ServerCallback]
    public void DestroyWeapon(GameObject weapon)
    {
        NetworkServer.Destroy(weapon);
    }
}


[Serializable]
public struct DamageData
{
    public int m_CurrentHP;
    public int m_CurrentArmor;
    public int m_EnergyShieldCount;
    public int m_LeftHandWeaponHP;
    public int m_RightHandWeaponHP;
}
