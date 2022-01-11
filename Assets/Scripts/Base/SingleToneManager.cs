using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建者：长生
/// 时间：2021年11月20日10:32:26
/// 功能：单例模式
/// </summary>

namespace GameLogic
{
    public class SingleToneManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get { return instance; }
            set
            {
                if (instance == null)
                {
                    instance = value;
                    //DontDestroyOnLoad(instance.gameObject);
                }
                else if (instance != value)
                {
                    Destroy(value.gameObject);
                }
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            Instance = this as T;
        }
    }
}