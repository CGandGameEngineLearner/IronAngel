using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletCollisionReceiver : NetworkBehaviour
{
    /// <summary>
    /// Ӧ��ֻ�з�����ϵ�����������ײ
    /// </summary>
    /// <param name="collision"></param>
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
