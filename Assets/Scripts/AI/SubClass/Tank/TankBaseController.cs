using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBaseController : MonoBehaviour
{
    private Vector3 m_LastPos;
    private Vector3 m_MoveDirection;
    
    public Transform TankBase;

    private Quaternion m_BaseRotate = Quaternion.identity;
    
    public float AngularSpeed = 30.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_BaseRotate = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        m_MoveDirection = (transform.position - m_LastPos);
        m_MoveDirection.z = 0;
        m_LastPos = transform.position;
        
        if (m_MoveDirection.magnitude > 0)
        {
            m_MoveDirection = m_MoveDirection.normalized;
            var targetRotate = Quaternion.LookRotation(m_MoveDirection);
            targetRotate = Quaternion.Euler(0, 0, targetRotate.eulerAngles.x+90);
            Debug.Log(targetRotate.eulerAngles);
            var m_Rotate = Quaternion.RotateTowards(m_BaseRotate, targetRotate, AngularSpeed * Time.deltaTime);
            m_BaseRotate = m_Rotate;
            TankBase.rotation = m_BaseRotate;
        }
        else
        {
            TankBase.rotation = m_BaseRotate;
        }
       
    }
}
