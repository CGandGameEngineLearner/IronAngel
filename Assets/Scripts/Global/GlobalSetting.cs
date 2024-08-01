using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    [Header("伤害计算")]
    [Tooltip("护甲减伤系数")]
    [Range(0f, 1f)]
    public float _DamageReductionCoefficient;
}
