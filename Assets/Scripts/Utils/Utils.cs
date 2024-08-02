using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 ApplyScatterY(Vector3 originalDirection, float spreadAngle)
    {
        // return originalDirection.normalized;
        // 在 Y 轴上生成一个微小的随机扰动分量
        float randomOffsetY = Random.Range(-spreadAngle, spreadAngle);

        // 添加扰动到原始方向向量
        Vector3 perturbedDirection = originalDirection + new Vector3(0, randomOffsetY, 0);

        // 确保返回的方向向量保持归一化
        return perturbedDirection.normalized;
    }
}
