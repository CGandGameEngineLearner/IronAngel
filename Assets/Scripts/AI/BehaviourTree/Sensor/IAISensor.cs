
using UnityEngine;
using System.Collections.Generic;


public interface IAISensor
{
    /// <summary>
    /// 获取察觉到的游戏对象
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetPerceiveGameObjects();
}
