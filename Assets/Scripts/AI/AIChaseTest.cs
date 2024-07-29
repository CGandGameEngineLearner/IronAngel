using UnityEngine;
using UnityEngine.AI;

public class AIChaseTest : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
