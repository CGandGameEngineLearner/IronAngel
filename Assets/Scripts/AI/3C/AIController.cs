
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using Mirror;


public class AIController : NetworkBehaviour
{
    private AIMovement m_AIMovement;
    private IAISensor m_AISensor;
    private GameObject m_ChaseGO;

    /// <summary>
    /// 训练路线
    /// </summary>
    public SplineContainer PatrolRoute;
    // Start is called before the first frame update
    void Start()
    {
        m_AIMovement = GetComponent<AIMovement>();
        m_AISensor = GetComponent<IAISensor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [ServerCallback]
    public virtual void Patrol()
    {
        if (PatrolRoute != null)
        {
            m_AIMovement.PatrolWithFixedRoute(PatrolRoute);
        }
        else
        {
            Debug.Log("没有设置巡逻路线");
        }
    }

    [ServerCallback]
    public virtual void Chase()
    {
        var chaseGameObjects = m_AISensor.GetPerceiveGameObjects();
        if (chaseGameObjects.Count > 0)
        {
            m_ChaseGO = chaseGameObjects[0];
            Debug.Log("AI正在追逐"+chaseGameObjects[0]);
            m_AIMovement.Chase(chaseGameObjects[0]);
        }
        else
        {
            if (m_ChaseGO!=null)
            {
                m_AIMovement.Chase(m_ChaseGO);
            }

        }
    }
    [ServerCallback]
    public virtual bool SetDestination(Vector3 target)
    {
        return m_AIMovement.SetDestination(target);
    }
    
    [ServerCallback]
    public List<GameObject> GetPerceiveGameObjects()
    {
        return m_AISensor.GetPerceiveGameObjects();
    }
}
