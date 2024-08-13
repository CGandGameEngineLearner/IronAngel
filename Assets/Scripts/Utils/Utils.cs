using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronAngel
{
    public static class Utils
    {
        // public static int 
    
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

    
        /// <summary>
        /// 随机布尔
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static bool RandomBool(float probability) {
            return Random.value < probability;
        }
        
        public static IEnumerable<GameObject> GetAllChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                yield return child.gameObject;
                foreach (var grandChild in GetAllChildren(child))
                {
                    yield return grandChild;
                }
            }
        }

       /// <summary>
       /// 爆炸衰减伤害计算
       /// </summary>
       /// <param name="maxDamage"></param>
       /// <param name="minDamage"></param>
       /// <param name="sigma"></param>
       /// <param name="fallout"></param>
       /// <param name="explodePoint"></param>
       /// <param name="characterPoint"></param>
       /// <returns></returns>
        public static int CalculateQuadraticDecayDamage(int maxDamage, int minDamage, float sigma, float fallout, Vector2 explodePoint, Vector2 characterPoint)
        {
            // 计算爆炸点和角色点之间的距离
            float distance = Vector2.Distance(explodePoint, characterPoint);
        
            // 如果距离为零，返回最大伤害
            if (distance == 0)
            {
                return maxDamage;
            }
        
            // 计算衰减比例
            float inverseSquareDistance = 1 / Mathf.Pow(distance, 2);
        
            // 计算实际伤害
            int damage = Mathf.RoundToInt(maxDamage * inverseSquareDistance);
        
            // 将伤害值限制在最小和最大伤害值之间
            damage = Mathf.Clamp(damage, minDamage, maxDamage);
        
            return damage;
        }

        public static Vector3 RandomDirection2D()
        {
            Vector3 dir = new Vector3(UnityEngine.Random.Range(-1.0f,1.0f),UnityEngine.Random.Range(-1.0f,1.0f),0);
            return dir.normalized;
        }
    }
}

