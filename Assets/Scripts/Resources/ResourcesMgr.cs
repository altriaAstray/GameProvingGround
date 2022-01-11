using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 功能：资源管理
/// 创建者：长生
/// 日期：2021年11月22日18:55:51
/// </summary>

namespace GameLogic
{
    public class ResourcesMgr : SingleToneManager<ResourcesMgr>
    {

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Init();
        }

        private string _streamPath;
        private Hashtable _resourceTable; //缓存从Resource中加载的资源

        private void Init()
        {
            _resourceTable = new Hashtable();
            _streamPath = Application.streamingAssetsPath;
        }

        /// <summary>
        /// 加载Resources下资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string path, bool isCache = true) where T : Object
        {
            if (_resourceTable.Contains(path))
                return _resourceTable[path] as T;
            var assets = Resources.Load<T>(path);
            if (assets == null)
            {
                Debug.LogFormat(" assets is not found at Path:{0}", path);
                return null;
            }
            if (isCache)
                _resourceTable.Add(path, assets);

            return assets;
        }

        ///// <summary>
        ///// 加载Stream下资源
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="path"></param>
        ///// <param name="isCache"></param>
        ///// <returns></returns>
        //public WWW LoadAssetAsStream(string name, bool isCache = true)
        //{
        //    if (_resourceTable.Contains(name))
        //        return _resourceTable[name] as WWW;
        //    var www = new WWW("file:///" + Path.Combine(_streamPath, name));
        //    Debug.Log(Path.Combine(_streamPath, name));
        //    while (!www.isDone) { }
        //    return www;
        //}

        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T InstantiateAsset<T>(string path) where T : Object
        {
            var obj = LoadAsset<T>(path);
            var go = GameObject.Instantiate<T>(obj);
            if (go == null)
                Debug.LogError("Instantiate {0} failed!", obj);
            return go;
        }

        /// <summary>
        /// 清除资源缓存
        /// </summary>
        public void ClearAssetsCache()
        {
            foreach (Object asset in _resourceTable)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(asset, true);
#else
                GameObject.DestroyObject(asset);
#endif
            }
            _resourceTable.Clear();
        }
    }
}