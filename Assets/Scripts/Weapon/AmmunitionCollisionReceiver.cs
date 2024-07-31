using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmmunitionCollisionReceiver : NetworkBehaviour
{
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
        
        Debug.Log("客户端的子弹打中了角色");
        Hit();
    }
    
    [ServerCallback]
    private void Hit()
    {
        Debug.Log("服务端子弹打中了角色");
    }
}
