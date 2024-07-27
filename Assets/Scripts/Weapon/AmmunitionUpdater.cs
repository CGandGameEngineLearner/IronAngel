using System.Collections.Generic;
using UnityEngine;

public class AmmunitionHandle
{
    public GameObject ammunition;
    public bool active;
}




/// <summary>
/// 更新子弹和路径
/// </summary>
public class AmmunitionUpdater
{
    private HashSet<AmmunitionHandle> ammunitionsToUpdate;

    // TODO:Add Route Data
    public void RegisterAmmunition(GameObject ammunition)
    {
        
    }

    public void Update()
    {
        foreach (var ammunition in ammunitionsToUpdate)
        {
            
        }
    }
}