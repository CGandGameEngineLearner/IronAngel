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
    /// RPCֱ��֪ͨ���Ը���
    /// </summary>
    /// <param name="properties"></param> �ܻ��߸��º������
    [ClientRpc]
    private void RPCBroadcastDamage(Properties properties)
    {

    }
}
