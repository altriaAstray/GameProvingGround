using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 创建者：长生
/// 时间：2021年11月20日10:32:26
/// 功能：工具
/// </summary>

namespace GameLogic
{
    public class Tools
    {
        /// <summary>
        /// 判断是否是编辑模式
        /// </summary>
        /// <returns></returns>
        public static bool IsDebug()
        {
            if (Application.isEditor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// object转int
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        /// <summary>
        /// object转float
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        /// <summary>
        /// object转long
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        /// <summary>
        /// 随机数（int）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// 随机数（float）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// 随机数（float）
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Uid(string uid)
        {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

        /// <summary>
        /// 获得时间
        /// </summary>
        /// <returns></returns>
        public static long GetTime()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }
        //----------------------------------------
        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static T Get<T>(GameObject go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static T Get<T>(Transform go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static T Get<T>(Component go, string subnode) where T : Component
        {
            return go.transform.Find(subnode).GetComponent<T>();
        }
        //----------------------------------------
        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T Add<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null) GameObject.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T Add<T>(Transform go) where T : Component
        {
            return Add<T>(go.gameObject);
        }
        //----------------------------------------
        /// <summary>
        /// 查找子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Child(GameObject go, string subnode)
        {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Child(Transform go, string subnode)
        {
            Transform tran = go.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        //----------------------------------------

        /// <summary>
        /// 取平级对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Peer(GameObject go, string subnode)
        {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="subnode"></param>
        /// <returns></returns>
        public static GameObject Peer(Transform go, string subnode)
        {
            Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        //----------------------------------------

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        /// <param name="go"></param>
        public static void ClearChild(Transform go)
        {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.GetChild(i).gameObject);
            }
        }

        //----------------------------------------
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        //----------------------------------------
        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string md5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }
        //----------------------------------------
        /// <summary>
        /// 无视锁文件，直接读bytes  读取（加载）数据
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string resPath)
        {
            byte[] bytes;
            using (FileStream fs = File.Open(resPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
            }
            return bytes;
        }
        //----------------------------------------
        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }
        //----------------------------------------
        /// <summary>
        /// color 转换hex
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ColorToHex(Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255.0f);
            int g = Mathf.RoundToInt(color.g * 255.0f);
            int b = Mathf.RoundToInt(color.b * 255.0f);
            int a = Mathf.RoundToInt(color.a * 255.0f);
            string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
            return hex;
        }

        /// <summary>
        /// hex转换到color
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexToColor(string hex)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            float a = cc / 255f;
            return new Color(r, g, b, a);
        }
        //----------------------------------------

        /// <summary>
        /// 检查类名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckClassName(string str)
        {
            return Regex.IsMatch(str, @"^[A-Z][A-Za-z0-9_]*$");
        }
        /// <summary>
        /// 检查字段名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckFieldName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z_][A-Za-z0-9_]*$");
        }
        //----------------------------------------

        /// <summary>
        /// LOG
        /// </summary>
        /// <param name="Level"></param>
        /// <returns></returns>
        public static bool OpenLog(int Level)
        {
            switch (Level)
            {
                case 0:     //log
                    return true;
                case 1:     //LogWarning
                    return true;
                case 2:     //LogError
                    return true;
            }
            return true;
        }

        public static void Log(string str)
        {
            if (OpenLog(1) == true)
                Debug.Log(str);
        }

        public static void LogWarning(string str)
        {
            if (OpenLog(2) == true)
                Debug.LogWarning(str);
        }

        public static void LogError(string str)
        {
            if (OpenLog(3) == true)
                Debug.LogError(str);
        }
        //----------------------------------------
        //static string ResourcesBuildDir
        //{
        //    get
        //    {
        //        var dir = "Assets/" + Config.ResourcesBuildDir + "/";
        //        return dir;
        //    }
        //}
        //----------------------------------------


        /// <summary>
        /// 获取文件协议
        /// </summary>
        /// <returns></returns>
        public static string GetFileProtocol()
        {
            string fileProtocol = "file://";
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer
#if !UNITY_5_4_OR_NEWER
                || Application.platform == RuntimePlatform.WindowsWebPlayer
#endif
)
                fileProtocol = "file:///";

            return fileProtocol;
        }
    }
}