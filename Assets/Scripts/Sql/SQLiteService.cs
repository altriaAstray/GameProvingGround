using SqlCipher4Unity3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Sql
{
    public class SQLiteService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="con"></param>
        public SQLiteService(SQLiteConnection con)
        {
            this.Connection = con;
        }

        //db connect
        private SQLiteConnection Connection { get; set; }
    }

}
