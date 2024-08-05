using System;
using UnityEngine;
using System.Collections.Generic;
using LogicState;
using Unity.VisualScripting;

public class DamageSensor:MonoBehaviour,IAISensor
{
    private LogicStateManager m_LogicStateManager;
    
    private void Start()
    {
        m_LogicStateManager = GetComponent<LogicStateManager>();
    }
    
    /// <summary>
    ///  伤害了玩家的人
    /// </summary>
    private List<GameObject> m_WhoDamaged = new List<GameObject>();
    public List<GameObject> GetPerceiveGameObjects()
    {
        m_WhoDamaged.RemoveAll(go => go == null);
        return m_WhoDamaged;
    }

    public void PutPerceiveGameObject(GameObject go)
    {
        m_WhoDamaged.Add(go);
        m_LogicStateManager.AddState(ELogicState.AIVisionPerceived);
    }

    
}
