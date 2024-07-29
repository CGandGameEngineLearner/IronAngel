using System;
using System.Collections.Generic;
using System.Linq;
using Mirror.BouncyCastle.Math.Field;
using UnityEngine;
public class VisionSensor:MonoBehaviour,IAISensor
{
    public Collider Sight;
    
    /// <summary>
    /// 视线左边缘到右边缘的夹角
    /// </summary>
    public float VisualAngle = 45;
    public float MaximumVisualDistance = 5;
    public Vector3 RelativeSightDirection = Vector3.right;//(1,0,0)
    
    /// <summary>
    /// 只注意察觉的GameObject的Tag
    /// </summary>
    public List<String> AttentionalGameObjectsTags;
    private Vector3 m_AbsoluteSightDirection;
    private HashSet<GameObject> m_GameObjectsInSight;
    private LogicStateManager m_LogicStateManager;
    
    public List<GameObject> GetPerceiveGameObjects()
    {
        List<GameObject> result = m_GameObjectsInSight.ToList();
        for(int i=0;i<result.Count;)
        {
            var visitedGameObject = result[i];
            var toVisitedGameObject = visitedGameObject.transform.position - transform.position;
            
            var distance = toVisitedGameObject.magnitude;
            var angle = Vector3.Angle(m_AbsoluteSightDirection, toVisitedGameObject);
            
            // 如果超出视线范围，或超过夹角，则移除
            if ( distance > MaximumVisualDistance || angle > (VisualAngle/2))
            {
                result.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        return result;
    }

    private void Start()
    {
        m_AbsoluteSightDirection = transform.TransformDirection(RelativeSightDirection);
    }

    private void Update()
    {
        m_AbsoluteSightDirection = transform.TransformDirection(RelativeSightDirection);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AttentionalGameObjectsTags.Contains(other.gameObject.tag))
        {
            m_GameObjectsInSight.Add(other.gameObject);
            m_LogicStateManager.AddState(ELogicState.AIVisionPerceived);
        }
        
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        m_GameObjectsInSight.Remove(other.gameObject);
        if (m_GameObjectsInSight.Count <= 0)
        {
            m_LogicStateManager.RemoveState(ELogicState.AIVisionPerceived);
        }
    }
}
