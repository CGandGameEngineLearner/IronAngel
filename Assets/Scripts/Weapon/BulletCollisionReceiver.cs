using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletCollisionReceiver : NetworkBehaviour
{
    /// <summary>
    /// 应该只有服务端上的物体会接收碰撞
    /// </summary>
    /// <param name="collision"></param>
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
