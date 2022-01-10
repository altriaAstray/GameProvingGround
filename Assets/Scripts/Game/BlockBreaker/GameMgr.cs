using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：游戏管理器
    /// 创建人：长生
    /// 时间：2022年1月10日17:04:22
    /// </summary>

    public class GameMgr : SingleToneManager<GameMgr>
    {
        int totalScore;             //总分
        int stage;                  //阶段

        //获得总分
        public int GetTotalScore()
        {
            return totalScore;
        }

        //设置总分
        public void SetTotalScore(int value)
        {
            totalScore = value;
        }

        private void Start()
        {
            
        }

        public void LoadPreScene()
        {
            SceneMgr.Instance.LoadPreScene();
        }
    }
}