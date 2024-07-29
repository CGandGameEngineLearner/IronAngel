
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;


public class AIController : MonoBehaviour
{
    private AIMovement m_AIMovement;
    private IAISensor m_AISensor;

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

    public virtual void Chase(GameObject target)
    {
        m_AIMovement.Chase(target);
    }

    public virtual bool SetDestination(Vector3 target)
    {
        return m_AIMovement.SetDestination(target);
    }

    public List<GameObject> GetPerceiveGameObjects()
    {
        return m_AISensor.GetPerceiveGameObjects();
    }
}
