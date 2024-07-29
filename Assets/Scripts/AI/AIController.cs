
using UnityEngine;
using UnityEngine.Splines;



public class AIController : MonoBehaviour
{
    private AIMovement m_AIMovement;

    /// <summary>
    /// 训练路线
    /// </summary>
    public SplineContainer PatrolRoute;
    // Start is called before the first frame update
    void Start()
    {
        m_AIMovement = GetComponent<AIMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Patrol()
    {
        //m_AIMovement.Patrol(PatrolRoute);
    }

    public virtual bool SetDestination(Vector3 target)
    {
        return m_AIMovement.SetDestination(target);
    }

}
