using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
public class VisionSensor:MonoBehaviour,IAISensor
{
    public Collider Sight;
    
    /// <summary>
    /// 视线左边缘到右边缘的夹角
    /// </summary>
    public float VisualAngle = 45;

    private float m_HalfVisualAngle
    {
        get { return VisualAngle / 2; }
    }
    public float MaximumVisualDistance = 5;
    public Vector3 RelativeSightDirection = Vector3.right;//(1,0,0)
    public bool DrawSightLine = false;
    
    /// <summary>
    /// 只注意察觉的GameObject的Tag
    /// </summary>
    public List<String> AttentionalGameObjectsTags;
    private Vector3 m_AbsoluteSightDirection;
    private HashSet<GameObject> m_GameObjectsInSight = new HashSet<GameObject>();
    private HashSet<GameObject> m_GameObjectsInTrigger = new HashSet<GameObject>();
    private LogicStateManager m_LogicStateManager;
    
    public List<GameObject> GetPerceiveGameObjects()
    {
        List<GameObject> result = m_GameObjectsInSight.ToList();
        return result;
    }

    private void Start()
    {
        RelativeSightDirection.Normalize();
        m_AbsoluteSightDirection = transform.TransformDirection(RelativeSightDirection);
        m_LogicStateManager = GetComponent<LogicStateManager>();
    }

    private void Update()
    {
        m_AbsoluteSightDirection = transform.TransformDirection(RelativeSightDirection);
        
        UpdateObjectsInSight();
        DrawSight();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AttentionalGameObjectsTags.Contains(other.gameObject.tag))
        {
            m_GameObjectsInTrigger.Add(other.gameObject);
        }
        
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        m_GameObjectsInTrigger.Remove(other.gameObject);
        UpdateObjectsInSight();
        if (m_GameObjectsInSight.Count <= 0)
        {
            m_LogicStateManager.RemoveState(ELogicState.AIVisionPerceived);
        }
    }

    private void DrawSight()
    {
        if (DrawSightLine)
        {
            Quaternion leftRotation = Quaternion.Euler(0, 0, m_HalfVisualAngle);
            Vector3 leftSightEdge = leftRotation * RelativeSightDirection;
            leftSightEdge = transform.TransformDirection(leftSightEdge);
            
            
            Quaternion rightRotation = Quaternion.Euler(0, 0, -m_HalfVisualAngle);
            Vector3 rightSightEdge = rightRotation * RelativeSightDirection;
            rightSightEdge = transform.TransformDirection(rightSightEdge);
            
            
            Debug.DrawLine(transform.position,transform.position+(leftSightEdge*MaximumVisualDistance),Color.red);
            Debug.DrawLine(transform.position,transform.position+(rightSightEdge*MaximumVisualDistance),Color.red);
        }
    }

    private bool CheckInSight(GameObject go)
    {
        var toVisitedGameObject = go.transform.position - transform.position;
            
        var distance = toVisitedGameObject.magnitude;
        var angle = Vector3.Angle(m_AbsoluteSightDirection, toVisitedGameObject);
        
        // 如果超出视线范围，或超过夹角，则移除
        if ( distance > MaximumVisualDistance || angle > m_HalfVisualAngle)
        {
            return false;
        }

        return true;
    }

    private void UpdateObjectsInSight()
    {
        m_GameObjectsInSight.Clear();
        foreach (var go in m_GameObjectsInTrigger)
        {
            if (CheckInSight(go))
            {
                Debug.Log("AI察觉到："+ go.name);
                m_LogicStateManager.AddState(ELogicState.AIVisionPerceived);
                m_GameObjectsInSight.Add(go);
            }
        }
    }
}
