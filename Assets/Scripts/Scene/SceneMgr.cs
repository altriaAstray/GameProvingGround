using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 功能：场景管理
/// 创建者：长生
/// 日期：2021年11月23日11:22:09
/// </summary>

namespace GameLogic
{
    public class SceneMgr : SingleToneManager<SceneMgr>
    {

        private Action m_onSceneLoaded = null;   // 场景加载完成回调

        private string m_strTargetSceneName = null;  // 将要加载的场景名
        private string m_strCurSceneName = null;   // 当前场景名，如若没有场景，则默认返回 Login
        private string m_strPreSceneName = null;   // 上一个场景名
        private bool m_bLoading = false;     // 是否正在加载中
        private const string m_strLoadSceneName = "LoadingScene";  // 加载场景名字
        private GameObject m_objLoadProgress = null;               // 加载进度显示对象

        //获取当前场景名
        public string s_strLoadedSceneName => Instance.m_strCurSceneName;

        private void Start()
        {
            Instance.m_strCurSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// 返回上一场景
        /// </summary>
        public void LoadPreScene()
        {
            if (string.IsNullOrEmpty(Instance.m_strPreSceneName))
            {
                return;
            }
            LoadScene(Instance.m_strPreSceneName);
        }
        //-------------------------------------------------
        void Clear()
        {
            LanguagesMgr.Instance.GetLanguageSubject().Clear();
        }
        //-------------------------------------------------
        /// <summary>
        /// 加载场景 (不带回调)
        /// </summary>
        /// <param name="strLevelName"></param>
        public void LoadScene(string strLevelName)
        {
            Clear();
            Instance.LoadLevel(strLevelName, null);
        }

        /// <summary>
        /// 加载场景（带回调）
        /// </summary>
        /// <param name="strLevelName"></param>
        /// <param name="onSecenLoaded"></param>
        public void LoadScene(string strLevelName, Action onSecenLoaded)
        {
            Instance.LoadLevel(strLevelName, onSecenLoaded);
        }
         
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strLevelName"></param>
        /// <param name="onSecenLoaded"></param>
        /// <param name="isDestroyAuto"></param>
        private void LoadLevel(string strLevelName, Action onSecenLoaded, bool isDestroyAuto = true)
        {
            if (m_bLoading || m_strCurSceneName == strLevelName)
            {
                return;
            }

            m_bLoading = true;  // 锁屏
                                // *开始加载    
            m_onSceneLoaded = onSecenLoaded;
            m_strTargetSceneName = strLevelName;
            m_strPreSceneName = m_strCurSceneName;
            m_strCurSceneName = m_strLoadSceneName;

            //先异步加载 Loading 界面
            //StartCoroutine(StartLoadSceneOnEditor(m_strLoadSceneName, OnLoadingSceneLoaded, null, LoadSceneMode.Additive));
            StartCoroutine(OnLoadingScene(OnLoadingSceneLoaded, LoadSceneMode.Single));
        }
        
        /// <summary>
        /// 加载-加载场景
        /// </summary>
        /// <param name="OnSecenLoaded"></param>
        /// <param name="OnSceneProgress"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        private IEnumerator OnLoadingScene(Action OnSecenLoaded, LoadSceneMode loadSceneMode)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(m_strLoadSceneName, loadSceneMode);
            if (null == async)
            {
                yield break;
            }

            while (!async.isDone)
            {
                float fProgressValue;
                if (async.progress < 0.9f)
                {
                    fProgressValue = async.progress;
                }
                else
                {
                    fProgressValue = 1.0f;
                }
                yield return null;
            }

            OnProgress(0);
            OnLoadingSceneLoaded();
        }


        /// <summary>
        /// 过渡场景加载完成回调
        /// </summary>
        private void OnLoadingSceneLoaded()
        {
            // 过渡场景加载完成后加载下一个场景
            StartCoroutine(OnLoadTargetScene(m_strTargetSceneName, LoadSceneMode.Single));
        }

        private IEnumerator OnLoadTargetScene(string strLevelName, LoadSceneMode loadSceneMode)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(strLevelName, loadSceneMode);
            async.allowSceneActivation = false;
            if (null == async)
            {
                yield break;
            }

            OnProgress(0.15f);
            yield return new WaitForSeconds(1.5f);

            //*加载进度
            while (!async.isDone)
            {
                float fProgressValue;

                if (async.progress < 0.9f)
                    fProgressValue = async.progress;
                else
                    fProgressValue = 1.0f;

                OnProgress(fProgressValue);

                if (fProgressValue >= 0.9)
                {
                    yield return new WaitForSeconds(1.5f);
                    async.allowSceneActivation = true;
                }
            }
            
            OnTargetSceneLoaded();
        }

        /// <summary>
        /// 加载下一场景完成回调
        /// </summary>
        private void OnTargetSceneLoaded()
        {
            m_bLoading = false;
            m_strCurSceneName = m_strTargetSceneName;
            m_strTargetSceneName = null;
            m_onSceneLoaded?.Invoke();
        }

        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="fProgress"></param>
        private void OnProgress(float fProgress)
        {
            if (m_objLoadProgress == null)
            {
                m_objLoadProgress = GameObject.Find("LoadingBar");

                if (m_objLoadProgress == null)
                {
                    Debug.Log(s_strLoadedSceneName);
                    return;
                }
            }

            Text textLoadProgress = m_objLoadProgress.transform.Find("TextLoadProgress").GetComponent<Text>();
            Slider slider = m_objLoadProgress.GetComponent<Slider>();

            if (textLoadProgress == null)
            {
                return;
            }

            if (slider == null)
            {
                return;
            }

            slider.value = fProgress;
            textLoadProgress.text = (fProgress * 100).ToString() + "%";
        }
    }
}