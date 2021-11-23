using SqlCipher4Unity3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
/// <summary>
/// 创建者：长生
/// 时间：2021年11月20日11:24:48
/// 功能：SQLite加载器
/// </summary>

namespace GameLogic.SQL
{
    public class SQLiteLoder
    {
        public readonly static string DB_PATH = "DataBase.db";

        private readonly SQLiteConnection _connection;

        public SQLiteLoder()
        {
#if UNITY_EDITOR
            string dbPath = string.Format(@"Assets/StreamingAssets/{0}", DB_PATH);
#else
            // check if file exists in Application.persistentDataPath
            string filepath = string.Format("{0}/{1}", Application.persistentDataPath, DB_PATH);

            if (!File.Exists(filepath))
            {
                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
                WWW loadDb =
     new WWW ("jar:file://" + Application.dataPath + "!/assets/" + DB_PATH); // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { } // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes (filepath, loadDb.bytes);
#elif UNITY_IOS
                string loadDb =
     Application.dataPath + "/Raw/" + DB_PATH; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
#elif UNITY_WP8
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DB_PATH; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
    
#elif UNITY_WINRT
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DB_PATH; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy (loadDb, filepath);
#elif UNITY_STANDALONE_OSX
                string loadDb =
     Application.dataPath + "/Resources/Data/StreamingAssets/" + DB_PATH; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#else
                string loadDb =
     Application.dataPath + "/StreamingAssets/" + DB_PATH; // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#endif

                Debug.Log("Database written");
            }

            var dbPath = filepath;
#endif
            //_connection = new SQLiteConnection(dbPath, "password");
            _connection = new SQLiteConnection(dbPath, "");

            if(Tools.IsDebug())
            {
                Debug.Log("Final PATH: " + dbPath);
            }
            
        }

        /// <summary>
        /// 是否关闭
        /// </summary>
        public bool IsClose
        {
            get
            {
                return _connection == null || !_connection.IsOpen;
            }
        }

        /// <summary>
        /// DB路径
        /// </summary>
        public string DBPath
        {
            get
            {
                return this._connection.DatabasePath;
            }
        }

        /// <summary>
        /// 创建db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CreateTable<T>()
        {
            _connection.DropTable<T>();
            _connection.CreateTable<T>();
        }
        /// <summary>
        /// 创建db
        /// </summary>
        public void CreateTable(Type t)
        {
            _connection.DropTableByType(t);
            _connection.CreateTableByType(t);
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="objects"></param>
        public void InsertTable(System.Collections.IEnumerable objects)
        {
            _connection.InsertAll(objects);
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="objects"></param>
        public void Insert(object @object)
        {
            _connection.Insert(@object);
        }


        /// <summary>
        /// 插入所有
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objTypes"></param>
        public void InsertAll<T>(List<T> obj)
        {
            _connection.Insert(@obj, typeof(T));
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public TableQuery<T> GetTable<T>() where T : new()
        {
            return new TableQuery<T>(_connection);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void Update<T>(T obj)
        {
            _connection.Update(obj);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void UpdateAll<T>(List<T> obj)
        {
            _connection.UpdateAll(obj);
        }


        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            _connection.Close();
        }
    }
}
