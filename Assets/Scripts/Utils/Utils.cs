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
    
    public static Vector3 ApplyScatterZ(Vector3 originalDirection, float spreadAngle)
    {
        // 在 Z 轴上生成一个微小的随机扰动角度
        float randomAngle = Random.Range(-spreadAngle, spreadAngle);

        // 创建一个表示绕 Z 轴旋转的四元数
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);

        // 对原始方向向量应用该旋转
        Vector3 perturbedDirection = rotation * originalDirection;

        // 返回结果，保持归一化
        return perturbedDirection.normalized;
    }
}
