
using UnityEngine;
using System.Collections.Generic;


public interface IAISensor
{
    /// <summary>
    /// 获取察觉到的游戏对象
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetPerceiveGameObjects();

    public void PutPerceiveGameObject(GameObject go);

    public delegate void NotifyPerceivedDelegate();

    public void SetNotifyPerceivedDelegate(NotifyPerceivedDelegate notifyPerceived);

}
