using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 创建者：长生
/// 时间：2021年11月20日10:32:26
/// 功能：数据管理器
/// </summary>

namespace GameLogic
{
    public class DataManager : SingleToneManager<DataManager>
    {
        private void Start()
        {
            StartSync();
        }

        private void StartSync()
        {
            DataService ds = new DataService("DataBase.db");
            //ds.CreateDB();
            //DataService ds = new DataService("tempDatabase.db");
            //ds.CreateDB();

            //IEnumerable<Person> people = ds.GetPersons();
            //ToConsole(people);
            //people = ds.GetPersonsNamedRoberto();
            //ToConsole("Searching for Roberto ...");
            //ToConsole(people);
        }
    }
}