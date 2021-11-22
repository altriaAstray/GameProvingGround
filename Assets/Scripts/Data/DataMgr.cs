using GameLogic.SQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 创建者：长生
/// 时间：2021年11月20日10:32:26
/// 功能：数据管理器
/// </summary>

namespace GameLogic
{
    public class DataMgr : SingleToneManager<DataMgr>
    {

        private SQLiteLoder sqlService;

        void Awake()
        {
            Instance = this;
            sqlService = new SQLiteLoder();
        }

        private void Start()
        {
            
        }

        //-------------------------------------
        //-------------------------------------
        //-------------------------------------
        //-------------------------------------
        //-------------------------------------
        //-------------------------------------
        public List<AudioConfig> GetAudio()
        {
            IEnumerable<AudioConfig> audios = sqlService.GetTable<AudioConfig>();
            return audios.ToList();
        }
        //-------------------------------------
        public List<GameConfig> GetGameConfig()
        {
            IEnumerable<GameConfig> configs = sqlService.GetTable<GameConfig>();
            return configs.ToList();            
        }

        public void SetGameConfig(GameConfig config)
        {
            sqlService.Update<GameConfig>(config);
        }
        //-------------------------------------
    }
}