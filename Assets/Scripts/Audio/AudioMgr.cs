using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

/// <summary>
/// 功能：音频管理
/// 创建者：长生
/// 日期：2021年11月23日11:22:09
/// </summary>

namespace GameLogic
{

    public class AudioMgr : SingleToneManager<AudioMgr>
    {
        //声音对象
        AudioSource sourceObj;
        Transform transformObj;

        //背景音乐
        bool playMusic = true;
        float volumeMusic = 0.5f;
        Dictionary<int, AudioConfig> musics = new Dictionary<int, AudioConfig>();   //全部bgm
        List<int> randomBgm = new List<int>();                                      //bgm随机播放
        //特效音
        bool playSound = true;
        float volumeSound = 0.5f;
        Dictionary<int, AudioConfig> sounds = new Dictionary<int, AudioConfig>();   //全部特效音

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            //加载特效音
            List<AudioConfig> tempAudios = DataMgr.Instance.GetAudio();

            foreach (AudioConfig tempData in tempAudios)
            {
                if(tempData.Type == 1 || tempData.Type == 2)
                {
                    musics.Add(tempData.Index, tempData);
                }
                else if (tempData.Type == 3)
                {
                    sounds.Add(tempData.Index, tempData);
                }
            }

            Init();
        }

        ///// <summary>
        ///// 初始化
        ///// </summary>
        public void Init()
        {
            //============初始化声音管理系统=============

            transformObj = transform;
            sourceObj = transformObj.GetComponent<AudioSource>();
            if (sourceObj == null)
            {
                sourceObj = gameObject.AddComponent<AudioSource>();
            }

            ReadConfig();
            //----------------声音管理系统初始化完成----------------
        }

        /// <summary>
        /// 获取全部音乐键
        /// </summary>
        /// <returns>全部音乐键</returns>
        public List<int> GetMusicKey()
        {
            return musics.Keys.ToList();
        }

        /// <summary>
        /// 根据类型获取全部音乐键
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>全部音乐键</returns>
        public List<int> GetMusicKeyByType(int type)
        {
            var query = from r in musics where r.Value.Type == type select r;
            List<int> list = new List<int>();

            foreach (KeyValuePair<int ,AudioConfig> kv in query)
            {
                list.Add(kv.Key); 
            }
            return list;
        }

        /// <summary>
        /// 获取全部音效键
        /// </summary>
        /// <returns>全部音效键</returns>
        public List<int> GetSoundKey()
        {
            return sounds.Keys.ToList();
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        public void ReadConfig()
        {
            playMusic = DataMgr.Instance.GetConfig()[100001].Value_3;
            volumeMusic = DataMgr.Instance.GetConfig()[100002].Value_2;

            playSound = DataMgr.Instance.GetConfig()[100101].Value_3;
            volumeSound = DataMgr.Instance.GetConfig()[100102].Value_2;
        }

        /// <summary>
        /// 是否播放BGM
        /// </summary>
        /// <returns>是否</returns>
        public bool IsPlayingMusic()
        {
            return playMusic;
        }
        /// <summary>
        /// 是否播放特效音
        /// </summary>
        /// <returns>是否</returns>
        public bool IsPlayingSound()
        {
            return playSound;
        }
        /// <summary>
        /// 播放BGM声音大小
        /// </summary>
        /// <returns>值</returns>
        public float VolumeMusic()
        {
            return volumeMusic;
        }
        /// <summary>
        /// 播放特效音声音大小
        /// </summary>
        /// <returns>值</returns>
        public float VolumeSound()
        {
            return volumeSound;
        }
        /// <summary>
        /// 设置背景音开关
        /// </summary>
        /// <param name="value"></param>
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
            DataMgr.Instance.GetConfig()[100001].Value_3 = playMusic;
            DataMgr.Instance.SetGameConfig(DataMgr.Instance.GetConfig()[100001]);
        }
        /// <summary>
        /// 设置背景音大小
        /// </summary>
        /// <param name="value"></param>
        public void SetVolumeMusic(float value)
        {
            volumeMusic = Mathf.Clamp(value,0,1);
            DataMgr.Instance.GetConfig()[100002].Value_2 = (float)Math.Round(volumeMusic, 2);
            DataMgr.Instance.SetGameConfig(DataMgr.Instance.GetConfig()[100002]);
            sourceObj.volume = volumeMusic;
        }
        /// <summary>
        /// 设置特效音开关
        /// </summary>
        /// <param name="value"></param>
        public void SetPlaySound(bool value)
        {
            playSound = value;
            DataMgr.Instance.GetConfig()[100101].Value_3 = playSound;
            DataMgr.Instance.SetGameConfig(DataMgr.Instance.GetConfig()[100101]);
        }
        /// <summary>
        /// 设置特效音大小
        /// </summary>
        /// <param name="value"></param>
        public void SetVolumeSound(float value)
        {
            volumeSound = Mathf.Clamp(value, 0, 1);
            DataMgr.Instance.GetConfig()[100102].Value_2 = (float)Math.Round(volumeSound,2);
            DataMgr.Instance.SetGameConfig(DataMgr.Instance.GetConfig()[100102]);
        }


        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="p_fileName"></param>
        public void PlayBGM(int index)
        {
            if (sourceObj.clip == null || !sourceObj.isPlaying)
            {
                if (musics != null && musics.ContainsKey(index))
                {
                    AudioClip audioClip = ResourcesMgr.Instance.LoadAsset<AudioClip>(musics[index].Path + musics[index].Name);
                    sourceObj.clip = audioClip;
                    sourceObj.volume = volumeMusic;
                    sourceObj.loop = true;
                    if (playMusic)
                        sourceObj.Play();
                }
            }
        }

        /// <summary>
        /// 随机播放
        /// </summary>
        /// <param name="p_fileName"></param>
        public void RandomPlayBGM(List<int> Ids)
        {
            randomBgm = Ids;

            if (sourceObj.clip == null || !sourceObj.isPlaying)
            {
                int index = UnityEngine.Random.Range(0, 5);
                if (musics != null && musics.ContainsKey(randomBgm[index]))
                {
                    AudioClip audioClip = ResourcesMgr.Instance.LoadAsset<AudioClip>(musics[randomBgm[index]].Path + musics[randomBgm[index]].Name);
                    sourceObj.clip = audioClip;
                    sourceObj.volume = volumeMusic;
                    sourceObj.loop = false;
                    if (playMusic)
                        sourceObj.Play();
                }
            }
        }

        public void Update()
        {
            if(randomBgm != null && randomBgm.Count > 0 && playMusic)
            {
                if(!sourceObj.isPlaying)
                {
                    int index = UnityEngine.Random.Range(0, randomBgm.Count);
                    if (musics != null && musics.ContainsKey(randomBgm[index]))
                    {
                        AudioClip audioClip = ResourcesMgr.Instance.LoadAsset<AudioClip>(musics[randomBgm[index]].Path + musics[randomBgm[index]].Name);
                        sourceObj.clip = audioClip;
                        sourceObj.volume = volumeMusic;
                        sourceObj.loop = false;
                        sourceObj.Play();
                    }
                }
            }
        }


        /// <summary>
        /// 结束背景音乐
        /// </summary>
        public void StopBGM()
        {
            if(randomBgm != null)
            {
                randomBgm.Clear();
            }

            if (sourceObj != null)
            {
                sourceObj.Stop();
            }
        }

        //特效音 - 播放与停止
        public void PlaySound(int index)
        {
            PlaySound(index, null, /*0.8f,*/ false);
        }
        //特效音 - 播放与停止
        public void PlaySound(int index, Transform pos)
        {
            PlaySound(index, pos,/* 0.8f,*/ false);
        }
        //特效音 - 播放与停止
        public void PlaySound(int index, Transform emitter, bool loop)
        {
            if (playSound == true && volumeSound > 0)
            {
                if (sounds != null && sounds.ContainsKey(index))
                {
                    AudioClip clip = ResourcesMgr.Instance.LoadAsset<AudioClip>(sounds[index].Path + sounds[index].Name);
                    if (clip != null)
                    {
                        StartCoroutine(ObjectProcessing(clip, emitter, volumeSound, loop));
                    }
                }
            }
        }

        /// <summary>
        /// 对象处理（生成声音对象）
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="emitter"></param>
        /// <param name="volume"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        IEnumerator ObjectProcessing(AudioClip clip, Transform emitter, float volume, bool loop)
        {
            GameObject go = SimplePool.Instance.GiveObj(0);
            if(go != null)
            {
                if (emitter != null)
                {
                    go.transform.parent = emitter;
                }
                else
                {
                    go.transform.parent = transformObj;
                }

                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);

                AudioSource t_source = go.GetComponent<AudioSource>();
                t_source.clip = clip;
                t_source.volume = volume;//AudioSize / 100f;
                t_source.loop = loop;
                t_source.Play();

                yield return new WaitForSeconds(clip.length);

                SimplePool.Instance.Takeobj(go);
            }
        }
    }
}

