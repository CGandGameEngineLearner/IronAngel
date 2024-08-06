using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    [Header("伤害计算")]
    [Tooltip("护甲减伤系数")]
    [Range(0f, 1f)]
    public float _DamageReductionCoefficient;
    
    [Header("AI")]
    [Tooltip("同时间发动攻击的AI数量"),Range(0,100)]
    public int AI_Attack_Tokens = 3;


    private void Start()
    {
        EventCenter.AddListener<GameObject, float, bool>(EventType.Buff_Speed, OnBuffSpeedModifier);
        EventCenter.AddListener<GameObject, bool>(EventType.Buff_Stun, OnBuffStunModifier);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameObject, float, bool>(EventType.Buff_Speed, OnBuffSpeedModifier);
        EventCenter.RemoveListener<GameObject, bool>(EventType.Buff_Stun, OnBuffStunModifier);
    }


    /// <summary>
    /// 有关移速的Buff
    /// </summary>
    /// <param name="receiver"></param> Buff接受对象
    /// <param name="speed"></param> 更改的速度值，可以是正负数，Detla值
    /// <param name="isStart"></param> 是否是Buff开始，用于设置和重置速度
    private void OnBuffSpeedModifier(GameObject receiver, float speed, bool isStart)
    {
        if(isStart)
        {
            if (receiver.TryGetComponent<PlayerController>(out var controller))
            {
                controller.Player.SetSpeed(speed);
            }
            if (receiver.TryGetComponent<AIMovement>(out var movement))
            {
                movement.SetSpeed(speed);
            }
        }
        else
        {
            if (receiver.TryGetComponent<PlayerController>(out var controller))
            {
                controller.Player.ResetSpeed();
            }
            if (receiver.TryGetComponent<AIMovement>(out var movement))
            {
                movement.ResetSpeed();
            }
        }

        Debug.LogWarning(receiver.name + " 更改速度" + speed);
    }

    /// <summary>
    /// 暂时是先设置为不能用
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="isStart"></param> 是否是开始眩晕
    private void OnBuffStunModifier(GameObject receiver, bool isStart)
    {
        if (isStart)
        {
            if (receiver.TryGetComponent<PlayerController>(out var controller))
            {
                controller.Player.SetSpeed(-1000);
            }
            if (receiver.TryGetComponent<AIMovement>(out var movement))
            {
                movement.SetSpeed(-1000);
            }
        }
        else
        {
            if (receiver.TryGetComponent<PlayerController>(out var controller))
            {
                controller.Player.ResetSpeed();
            }
            if (receiver.TryGetComponent<AIMovement>(out var movement))
            {
                movement.ResetSpeed();
            }
        }
    }
}
