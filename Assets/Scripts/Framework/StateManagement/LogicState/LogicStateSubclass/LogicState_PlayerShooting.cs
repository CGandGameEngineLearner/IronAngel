using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicState
{
    public class LogicState_PlayerShooting : LogicState
    {
        public LogicState_PlayerShooting(ELogicState stateEnum) : base(stateEnum) { }
        public override void OnStateIn()
        {
#if UNITY_EDITOR
            Debug.Log("enter" + GetType());
#endif
        
        }

        public override void Update(float deltaTime)
        {

        }

        public override void FixedUpdate()
        {
        
        }

        public override void OnStateOut()
        {
#if UNITY_EDITOR
            Debug.Log("exit" + GetType());
#endif
        }
    }
}

