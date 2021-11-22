using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace GameLogic
{
    /// <summary>
    /// 音频管理器
    /// </summary>

    public class AudioMgr : SingleToneManager<AudioMgr>
    {
        //声音对象
        AudioSource sourceObj;
        Transform transformObj;

        bool playMusic = true;
        float volumeMusicMax = 1f;
        float volumeMusicMan = 0f;
        float volumeMusic = 0.5f;

        bool playSound = true;
        float volumeSoundMax = 0.8f;
        float volumeSoundMan = 0.8f;
        float volumeSound = 0.5f;

        // 游戏配置表
        Dictionary<int, GameConfig> configs = new Dictionary<int, GameConfig>();

        private void Start()
        {
            List<GameConfig> tempConfigs = DataMgr.Instance.GetGameConfig();

            foreach(GameConfig tempData in tempConfigs)
            {
                configs.Add(tempData.Index,tempData);
            }

            Init();
        }

        ///// <summary>
        ///// 初始化
        ///// </summary>
        public void Init()
        {
            //============初始化声音管理系统=============

            //if (!PlayerPrefs.HasKey(musicSwitchKey))//不存在键值则创建
            //{
            //    PlayerPrefs.SetInt(musicSwitchKey, 100);
            //}
            //if (!PlayerPrefs.HasKey(audioSwitchKey))//不存在键值则创建
            //{
            //    PlayerPrefs.SetInt(audioSwitchKey, 100);
            //}

            playMusic = configs[100001].Value_3;
            volumeMusic = configs[100002].Value_2;

            playSound = configs[100101].Value_3;
            volumeSound = configs[100102].Value_2;

            transformObj = transform;
            sourceObj = transformObj.GetComponent<AudioSource>();
            if (sourceObj == null)
            {
                sourceObj = gameObject.AddComponent<AudioSource>();
            }
            //----------------声音管理系统初始化完成----------------

            SetPlayMusic(false);
            SetVolumeMusic(0.3f);
            SetPlaySound(false);
            SetVolumeSound(0.3f);
        }

        public void ReadConfig()
        {

        }

        public bool PlayMusic()
        {
            return playMusic;
        }

        public bool PlaySound()
        {
            return playSound;
        }

        public float VolumeMusic()
        {
            return volumeMusic;
        }

        public float VolumeSound()
        {
            return volumeSound;
        }

        public void SetPlayMusic(bool value)
        {
            playMusic = value;

            if (playMusic == false)
            {
                sourceObj.Pause();
            }
            else
            {
                sourceObj.Play();
            }
            configs[100001].Value_3 = playMusic;
            DataMgr.Instance.SetGameConfig(configs[100001]);
        }
        public void SetVolumeMusic(float value)
        {

            volumeMusic = value;
            configs[100002].Value_2 = volumeMusic;
            DataMgr.Instance.SetGameConfig(configs[100002]);
            sourceObj.volume = volumeMusic;
        }

        public void SetPlaySound(bool value)
        {
            playSound = value;
            configs[100101].Value_3 = playSound;
            DataMgr.Instance.SetGameConfig(configs[100101]);
        }

        public void SetVolumeSound(float value)
        {
            volumeSound = value;
            configs[100102].Value_2 = volumeSound;
            DataMgr.Instance.SetGameConfig(configs[100102]);
        }


        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="p_fileName"></param>
        public void PlayBG(string index)
        {
            if (sourceObj.clip == null || index != sourceObj.clip.name || !sourceObj.isPlaying)
            {
                //BGMSetting bgm = BGMSettings.Get(index);
                //if (bgm != null)
                //{
                //    var audioLoader = AudioLoader.Load(bgm.Path, (isOk, loadAudio) =>
                //    {
                //        if (isOk)
                //        {
                //            sourceObj.clip = loadAudio;
                //            sourceObj.volume = volumeMusic;
                //            sourceObj.loop = true;
                //            if (playMusic)
                //                sourceObj.Play();
                //        }
                //    });
                //}
            }
        }

        /// <summary>
        /// 结束背景音乐
        /// </summary>
        public void StopBG()
        {
            if (sourceObj != null)
                sourceObj.Stop();
        }

        //音效 - 播放与停止
        public void PlaySound(string index)
        {
            PlaySound(index, null, /*0.8f,*/ false);
        }

        public void PlaySound(string index, Transform pos)
        {
            PlaySound(index, pos,/* 0.8f,*/ false);
        }

        public void PlaySound(string index, Transform emitter, /*float volume,*/ bool loop)
        {
            if (playSound == true && volumeSound > 0)
            {
                //AudioSetting audio = AppSettings.AudioSettings.Get(index);
                //if (audio != null)
                //{
                //    var audioLoader = AudioLoader.Load(audio.Path, (isOk, loadAudio) =>
                //    {
                //        if (isOk)
                //        {
                //            AudioClip clip = loadAudio;

                //            if (clip != null)
                //            {
                //                StartCoroutine(ObjectProcessing(clip, emitter, volumeSound, loop));
                //            }
                //        }
                //    });
                //}
            }
        }

        //WaitForSeconds delay0 = new WaitForSeconds(3.0f);
        //IEnumerator ObjectProcessing(AudioClip clip, Transform emitter, float volume, bool loop)
        //{
        //    GameObject go = SimplePool.Instance.GiveObj(0);
        //    go.transform.parent = emitter != null ? emitter : transformObj;
        //    go.transform.localPosition = Vector3.zero;
        //    go.SetActive(true);

        //    AudioSource t_source = go.GetComponent<AudioSource>();
        //    t_source.clip = clip;
        //    t_source.volume = volume;//AudioSize / 100f;
        //    t_source.loop = loop;
        //    t_source.Play();

        //    yield return new WaitForSeconds(clip.length);

        //    SimplePool.Instance.Takeobj(go);
        //}
        //#endregion

    }
}

