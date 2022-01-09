using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    //---------------------------------
    //  <author> 杨长生 </author>
    //  <descride> UI 文本 </descride>	
    //  <time> [2020-4-21 13:52:01] </time>	
    //---------------------------------
    public delegate void DelegateGetText();
    public class UIText : LanguageObserver
    {
        [SerializeField]
        private int key;
        // Use this for initialization

        void Start()
        {
            if (subject == null && LanguagesMgr.Instance != null)
            {
                subject = LanguagesMgr.Instance.GetLanguageSubject();
                subject.Attach(this);
            }

            GetTextLanguage();
        }
        
        public void GetTextLanguage()
        {
            if (LanguagesMgr.Instance == null)
                return;

            string value = LanguagesMgr.Instance.GetTextByIndex(key);

            if (!string.IsNullOrEmpty(value))
            {
                GetComponent<Text>().text = value;
            }
        }

        public override void UpdateData()
        {
            GetTextLanguage();
        }
    }
}