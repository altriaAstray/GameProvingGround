﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class StartGameView : MonoBehaviour
    {

        public Dropdown languages;    //语言选择
        public Button bgmOn;          //背景音开
        public Button bgmOff;         //背景音关
        public Slider bgmVolume;      //背景音大小
        public Text bgmVolumeStr;     //背景音大小数值


        public Button soundOn;        //效果音开
        public Button soundOff;       //效果音关
        public Slider soundVolume;    //效果音大小
        public Text soundVolumeStr;   //效果音大小数值

        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if(languages != null)
            {
                languages.value = (int)LanguagesMgr.Instance.GetMultilingual();
                languages.onValueChanged.RemoveAllListeners();
                languages.onValueChanged.AddListener((value)=> 
                {
                    LanguagesMgr.Instance.SetMultilingual(value);
                });
            }

            bgmOn.onClick.RemoveAllListeners();
            bgmOff.onClick.RemoveAllListeners();
            soundOn.onClick.RemoveAllListeners();
            soundOff.onClick.RemoveAllListeners();

            bgmOn.onClick.AddListener(()=> 
            {
                bgmOn.gameObject.SetActive(false);
                bgmOff.gameObject.SetActive(true);
                AudioMgr.Instance.SetPlayMusic(false);
            });

            bgmOff.onClick.AddListener(() => 
            {
                bgmOn.gameObject.SetActive(true);
                bgmOff.gameObject.SetActive(false);
                AudioMgr.Instance.SetPlayMusic(true);
            });

            soundOn.onClick.AddListener(() => 
            {
                soundOn.gameObject.SetActive(false);
                soundOff.gameObject.SetActive(true);
                AudioMgr.Instance.SetPlaySound(false);
            });

            soundOff.onClick.AddListener(() => 
            {
                soundOn.gameObject.SetActive(true);
                soundOff.gameObject.SetActive(false);
                AudioMgr.Instance.SetPlaySound(true);
            });


            if (DataMgr.Instance.GetConfig()[100001].Value_3 == true)
            {
                bgmOn.gameObject.SetActive(true);
                bgmOff.gameObject.SetActive(false);
            }
            else
            {
                bgmOn.gameObject.SetActive(false);
                bgmOff.gameObject.SetActive(true);
            }

            if (DataMgr.Instance.GetConfig()[100101].Value_3 == true)
            {
                soundOn.gameObject.SetActive(true);
                soundOff.gameObject.SetActive(false);
            }
            else
            {
                soundOn.gameObject.SetActive(false);
                soundOff.gameObject.SetActive(true);
            }

            bgmVolume.value = DataMgr.Instance.GetConfig()[100002].Value_2;
            bgmVolumeStr.text = bgmVolume.value.ToString("f1");
            
            bgmVolume.onValueChanged.AddListener((value) => 
            {
                bgmVolumeStr.text = value.ToString("f1");
                AudioMgr.Instance.SetVolumeMusic(value);
            });

            soundVolume.value = DataMgr.Instance.GetConfig()[100102].Value_2;
            soundVolumeStr.text = bgmVolume.value.ToString("f1");

            soundVolume.onValueChanged.AddListener((value) =>
            {
                soundVolumeStr.text = value.ToString("f1");
                AudioMgr.Instance.SetVolumeSound(value);
            });
            Invoke("Play", 1f);
        }

        /// <summary>
        /// 播放背景音
        /// </summary>
        public void Play()
        {
            List<int> musicKey = AudioMgr.Instance.GetMusicKey();
            List<int> musicIds = new List<int>();
            for(int i = 0;i < 5;i++)
            {
                musicIds.Add(musicKey[i]);
            }

            AudioMgr.Instance.RandomPlayBGM(musicIds);
        }
        /// <summary>
        /// 开始游戏
        /// </summary>
        public void StartGame()
        {
            AudioMgr.Instance.StopBGM();
            AudioMgr.Instance.PlaySound(100030);
            Invoke("LoadScene", 0.5f);
        }

        void LoadScene()
        {
            SceneMgr.Instance.LoadScene("MainScene");
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void GameQuit()
        {
            Application.Quit();
        }
    }
}

