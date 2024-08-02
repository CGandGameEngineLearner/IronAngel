
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using AI.TokenPool;
using Mirror;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;


public class AIController : NetworkBehaviour
{
    private AIMovement m_AIMovement;
    private IAISensor m_AISensor;
    private GameObject m_ChaseGO;
    private BaseProperties m_BaseProperties;

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

    [ServerCallback]
    public List<GameObject> GetGameObjectsInAttackRange()
    {
        var result = GetPerceiveGameObjects();
        
        // 移除超出攻击范围的
        result.RemoveAll(o =>
            Vector3.Distance(o.transform.position, transform.position) > 
            m_BaseProperties.m_Properties.m_AttackRange
            );

        return result;
    }

    [ServerCallback]
    public bool Attack()
    {
        var enemy = GetGameObjectsInAttackRange();
        if (enemy.Count <= 0)
        {
            return false;
        }

        if (TokenPool.ApplyToken(m_BaseProperties.m_Properties.m_TokenWeight) == false)
        {
            return false;
        }
    }


    [ClientRpc]
    public bool RpcAttack(GameObject enemy)
    {
        
    }
}
