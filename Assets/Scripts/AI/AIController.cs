using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private AIMovement m_AIMovement;
    // Start is called before the first frame update
    void Start()
    {
        m_AIMovement = new AIMovement(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
