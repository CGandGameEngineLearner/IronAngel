using System;
using System.Linq.Expressions;
using UnityEngine.InputSystem.Controls;
using static System.DateTime;


namespace AI.TokenPool
{
    /// <summary>
    /// 用于限制同时攻击玩家的AI数量
    /// 每个AI只能以权重概率抢到Token后才能攻击
    /// </summary>
    public static class TokenPool
    {
        private static int m_TokensNum = 3;
        private static int m_MaxTokensNum = 3;
        private static int m_ReleaseTokens = 0;
        private static Random rand = new Random( DateTime.Now.Millisecond);
        
        /// <summary>
        /// 当前全游戏的Token数量，即发出去的Token+池子中的Token
        /// </summary>
        public static int AllTokensNum
        {
            get { return m_TokensNum + m_ReleaseTokens; }
        }
        
        /// <summary>
        /// 设置所有AI同时持有的最大Token数量
        /// </summary>
        /// <param name="tokensNum"></param>
        public static void SetMaxTokensNum(int tokensNum)
        {
            m_MaxTokensNum = tokensNum;
            
            // 运行时 Token数量增大 则自动补足
            if (AllTokensNum < tokensNum)
            {
                m_TokensNum += m_MaxTokensNum - AllTokensNum;
            }
        }
        
        /// <summary>
        /// 申请Token 申请完后记得归还
        /// </summary>
        /// <param name="weight">抢到token的权重概率</param>
        /// <returns></returns>
        public static bool ApplyToken(float weight)
        {
            if (weight > 1 || weight < 0)
            {
                throw new Exception("概率权重的取值范围应为[0,1],但是接受到的是:"+weight);
            }
            if (m_TokensNum <= 0)
            {
                return false;
            }

            float randFloat = (float)rand.NextDouble();
            
            if (randFloat > weight)
            {
                return false;
            }

            m_TokensNum -= 1;
            m_ReleaseTokens += 1;

            return true;
        }
        
        /// <summary>
        /// 归还Token
        /// </summary>
        public static void ReturnToken()
        {
            m_ReleaseTokens -= 1;
            
            // 防止运行时，动态调整MaxTokensNum，导致Token超发
            if (AllTokensNum > m_MaxTokensNum)
            {
                m_TokensNum -= AllTokensNum - m_MaxTokensNum;
                return;
            }
            
            m_TokensNum += 1;
        }
    }
}