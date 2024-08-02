using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponCollisionReceiver : NetworkBehaviour
{
    [Tooltip("这个脚本的物体是左手的武器还是右手的武器")]
    public WeaponCollisionType m_Type = WeaponCollisionType.Left;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    [ServerCallback]
    public void CalculateDamage(AmmunitionConfig config)
    {

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