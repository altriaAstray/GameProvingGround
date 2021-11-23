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

        // 游戏配置表
        Dictionary<int, GameConfig> configs = new Dictionary<int, GameConfig>();

        void Awake()
        {
            Instance = this;
            sqlService = new SQLiteLoder();

            List<GameConfig> tempConfigs = DataMgr.Instance.GetGameConfig();
            foreach (GameConfig tempData in tempConfigs)
            {
                configs.Add(tempData.Index, tempData);
            }
        }

        private void Start()
        {

        }

        //-------------------------------------
        public Dictionary<int, GameConfig> GetConfig()
        {
            return configs;
        }

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
        public List<Languages> GetLanguages()
        {
            IEnumerable<Languages> configs = sqlService.GetTable<Languages>();
            return configs.ToList();
        }

        //-------------------------------------


    }
}