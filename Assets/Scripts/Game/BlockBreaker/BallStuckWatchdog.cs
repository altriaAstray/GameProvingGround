using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：球防止卡死监管
    /// 创建者：长生
    /// 日期：2022年1月9日18:46:11
    /// </summary>
    public class BallStuckWatchdog : MonoBehaviour
    {
        [SerializeField] private Dictionary<Ball, float> stuckBalls = new Dictionary<Ball, float>();
        private static float TIME_TILL_ASSUMED_STUCK = 7f;

        void Update()
        {
            List<Ball> toBeRemoved = new List<Ball>();
            foreach (KeyValuePair<Ball, float> entry in stuckBalls)
            {
                if (Time.realtimeSinceStartup - entry.Value > TIME_TILL_ASSUMED_STUCK)
                {
                    if (entry.Key != null)
                    {
                        entry.Key.UnstuckMe();
                    }
                    toBeRemoved.Add(entry.Key);
                }
            }

            foreach (Ball ball in toBeRemoved)
            {
                RemovePossibleStuckBall(ball);
            }
        }

        public void AddPossibleStuckBall(Ball ball)
        {
            if (!stuckBalls.ContainsKey(ball))
            {
                stuckBalls[ball] = Time.realtimeSinceStartup;
            }
        }

        public void RemovePossibleStuckBall(Ball ball)
        {
            if (stuckBalls.ContainsKey(ball))
            {
                stuckBalls.Remove(ball);
            }
        }
    }
}