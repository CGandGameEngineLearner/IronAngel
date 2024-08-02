using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(BoxCollider2D))]
public class WeaponCollisionReceiver : NetworkBehaviour
{
    [Tooltip("这个脚本的物体是左手的武器还是右手的武器")]
    public WeaponCollisionType m_Type = WeaponCollisionType.Left;

    public BoxCollider2D m_Collider;

    private AmmunitionCollisionReceiver m_AmmunitionCollisionReceiver;

    private void Start()
    {
        m_AmmunitionCollisionReceiver = transform.parent.GetComponent<AmmunitionCollisionReceiver>();

        m_Collider = GetComponent<BoxCollider2D>();

        if(m_Type == WeaponCollisionType.Left)
        {
            if(m_AmmunitionCollisionReceiver.m_LeftWeapon)
            {
#if UNITY_EDITOR
                Debug.LogError("在相同的手部位置已经有武器了");
#endif
            }
            else
            {
                m_AmmunitionCollisionReceiver.m_LeftWeapon = this;
            }
        }
        else if(m_Type == WeaponCollisionType.Right)
        {
            if (m_AmmunitionCollisionReceiver.m_RightWeapon)
            {
#if UNITY_EDITOR
                Debug.LogError("在相同的手部位置已经有武器了");
#endif
            }
            else
            {
                m_AmmunitionCollisionReceiver.m_RightWeapon = this;
            }
        }

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
    public void CalculateDamage(AmmunitionConfig config)
    {
        m_AmmunitionCollisionReceiver.CalculateDamage(config, 0);
    }

    // <summary>
    /// RPC直接通知属性更改
    /// </summary>
    /// <param name="properties"></param> 受击者更新后的属性
    [ClientRpc]
    private void RPCBroadcastDamage(Properties properties)
    {

    }
}


public enum WeaponCollisionType
{
    Left,
    Right,
}