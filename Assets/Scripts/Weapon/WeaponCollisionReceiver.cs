using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponCollisionReceiver : NetworkBehaviour
{
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
