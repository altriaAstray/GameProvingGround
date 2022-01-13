using GameLogic.SQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 功能：数据管理器
/// 创建者：长生
/// 时间：2021年11月20日10:32:26
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
            DontDestroyOnLoad(gameObject);
            sqlService = new SQLiteLoder();

            List<GameConfig> tempConfigs = GetGameConfig();
            foreach (GameConfig tempData in tempConfigs)
            {
                configs.Add(tempData.Index, tempData);
            }
        }

        private void Start()
        {

        }

        //-------------------------------------
        /// <summary>
        /// 获取游戏配置表数据（已从数据库读取）
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, GameConfig> GetConfig()
        {
            return configs;
        }

        //-------------------------------------
        /// <summary>
        /// 获取音频配置表数据（从数据库读取）
        /// </summary>
        /// <returns></returns>
        public List<AudioConfig> GetAudio()
        {
            IEnumerable<AudioConfig> audios = sqlService.GetTable<AudioConfig>();
            return audios.ToList();
        }
        //-------------------------------------
        /// <summary>
        /// 获取游戏配置表数据（从数据库读取）
        /// </summary>
        /// <returns></returns>
        public List<GameConfig> GetGameConfig()
        {
            IEnumerable<GameConfig> configs = sqlService.GetTable<GameConfig>();
            return configs.ToList();            
        }
        /// <summary>
        /// 设置游戏配置表数据（从数据库读取）
        /// </summary>
        /// <returns></returns>
        public void SetGameConfig(GameConfig config)
        {
            sqlService.Update<GameConfig>(config);
        }
        //-------------------------------------
        /// <summary>
        /// 获取语言配置表数据（从数据库读取）
        /// </summary>
        /// <returns></returns>
        public List<Languages> GetLanguages()
        {
            IEnumerable<Languages> configs = sqlService.GetTable<Languages>();
            return configs.ToList();
        }
        //-------------------------------------


    }
}