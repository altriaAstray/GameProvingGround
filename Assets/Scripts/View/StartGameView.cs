using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    /// <summary>
    /// 功能：游戏开始界面
    /// 创建者：长生
    /// 日期：2021年11月23日11:22:09
    /// </summary>
    
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

        string sceneName;
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
                    PlaySound();
                    LanguagesMgr.Instance.SetMultilingual(value);
                });
            }

            bgmOn.onClick.RemoveAllListeners();
            bgmOff.onClick.RemoveAllListeners();
            soundOn.onClick.RemoveAllListeners();
            soundOff.onClick.RemoveAllListeners();

            bgmOn.onClick.AddListener(()=> 
            {
                PlaySound();
                bgmOn.gameObject.SetActive(false);
                bgmOff.gameObject.SetActive(true);
                AudioMgr.Instance.SetPlayMusic(false);
            });

            bgmOff.onClick.AddListener(() => 
            {
                PlaySound();
                bgmOn.gameObject.SetActive(true);
                bgmOff.gameObject.SetActive(false);
                AudioMgr.Instance.SetPlayMusic(true);
            });

            soundOn.onClick.AddListener(() => 
            {
                PlaySound();
                soundOn.gameObject.SetActive(false);
                soundOff.gameObject.SetActive(true);
                AudioMgr.Instance.SetPlaySound(false);
            });

            soundOff.onClick.AddListener(() => 
            {
                PlaySound();
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
            List<int> musicKey = AudioMgr.Instance.GetMusicKeyByType(1);
            AudioMgr.Instance.RandomPlayBGM(musicKey);
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void StartGame(string name)
        {
            AudioMgr.Instance.StopBGM();
            PlaySound();

            sceneName = name;

            Invoke("LoadScene", 0.5f);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        void LoadScene()
        {
            SceneMgr.Instance.LoadScene(sceneName);
        }

        /// <summary>
        /// 播放按钮音效
        /// </summary>
        public void PlaySound()
        {
            AudioMgr.Instance.PlaySound(100014);
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

