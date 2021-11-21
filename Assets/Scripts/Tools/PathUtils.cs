using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class PathUtils
    {
        public static readonly string[] PathHeadDefine = { "jar://", "jar:file:///", "file:///", "http://", "https://" };

        /// <summary>
        /// 验证路径（是否为真路径）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsSureDir(string path)
        {
            int i = path.LastIndexOf("/");
            if (i >= 0)
            {
                return true;
            }

            i = path.LastIndexOf("\\");
            if (i >= 0)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 验证路径（是否为全路径）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFullPath(string path)
        {
            int i = path.IndexOf(":/");
            if (i >= 0)
            {
                return true;
            }

            i = path.IndexOf(":\\");
            if (i >= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            string parent = "", child = "";
            SplitPath(path, ref parent, ref child, true);
            return child;
        }

        /// <summary>
        /// 获取父路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetParentDir(string path)
        {
            string parent = "", child = "";
            SplitPath(path, ref parent, ref child, true);
            return parent;
        }

        /// <summary>
        /// 路径拆分
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="bSplitExt"></param>
        /// <returns></returns>
        public static string SplitPath(string path, ref string parent, ref string child, bool bSplitExt = false)
        {
            string ext = "";
            string head = SplitPath(path, ref parent, ref child, ref ext);
            if (bSplitExt)
            {
                return head;
            }

            if (!string.IsNullOrEmpty(ext))
            {
                child = child + "." + ext;
            }

            return head;
        }

        /// <summary>
        /// 路径拆分
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string SplitPath(string path, ref string parent, ref string child, ref string ext)
        {
            string head = GetPathHead(path);

            int index = path.LastIndexOf("/");
            int index2 = path.LastIndexOf("\\");
            index = System.Math.Max(index, index2);

            if (index == head.Length - 1)
            {
                parent = "";
                child = path;
            }
            else
            {
                parent = path.Substring(0, index);
                child = path.Substring(index + 1);
            }

            index = child.LastIndexOf(".");
            if (index >= 0)
            {
                ext = child.Substring(index + 1);
                child = child.Substring(0, index);
            }

            return head;
        }

        /// <summary>
        /// 获取路径头
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathHead(string path)
        {
            for (int i = 0; i < PathHeadDefine.Length; i++)
            {
                if (path.StartsWith(PathHeadDefine[i]))
                {
                    return PathHeadDefine[i];
                }
            }

            return "";
        }
    }
}