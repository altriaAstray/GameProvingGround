using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 功能：多语言管理
/// 创建者：长生
/// 日期：2021年11月23日11:22:09
/// </summary>

namespace GameLogic
{
    public class LanguagesMgr : SingleToneManager<LanguagesMgr>
    {
        Dictionary<int, Languages> languages = new Dictionary<int, Languages>();

        LanguageSubject subject = new LanguageSubject();
        
        Multilingual multilingual;

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
            List<Languages> tempConfigs = DataMgr.Instance.GetLanguages();

            foreach (Languages tempData in tempConfigs)
            {
                languages.Add(tempData.Index, tempData);
                //Debug.Log(tempData.Index +"|"+ tempData.EN+ "|"+ tempData.ZH);
            }

            SetMultilingual(DataMgr.Instance.GetConfig()[100200].Value_1, true);
        }

        /// <summary>
        /// 获取多语言类型
        /// </summary>
        /// <returns></returns>
        public Multilingual GetMultilingual()
        {
            return multilingual;
        }

        /// <summary>
        /// 选择语言
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isInit"></param>
        public void SetMultilingual(int value,bool isInit = false)
        {
            switch(value)
            {
                case 0:
                    multilingual = Multilingual.ZH;
                    break;
                case 1:
                    multilingual = Multilingual.EN;
                    break;
            }

            subject.SetState();
            
            if(isInit == false)
            {
                DataMgr.Instance.GetConfig()[100200].Value_1 = value;
                DataMgr.Instance.SetGameConfig(DataMgr.Instance.GetConfig()[100200]);
            }
        }

        /// <summary>
        /// 根据语言和索引获得文本内容
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetTextByIndex(int index)
        {
            string text = "";

            if(languages.ContainsKey(index))
            {
                switch (multilingual)
                {
                    case Multilingual.ZH:
                        text = languages[index].ZH;
                        break;

                    case Multilingual.EN:
                        text = languages[index].EN;
                        break;
                }
            }
            return text;
        }

        /// <summary>
        /// 获得被观察者
        /// </summary>
        /// <returns></returns>
        public LanguageSubject GetLanguageSubject()
        {
            return subject;
        }
    }
}