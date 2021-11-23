using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    //---------------------------------
    //  <author> 杨长生 </author>
    //  <descride> 观察者 - 语言 </descride>	
    //  <time> [2020-4-21 13:44:07] </time>	
    //---------------------------------
    public abstract class LanguageObserver : MonoBehaviour
    {
        protected LanguageSubject subject;
        public abstract void UpdateData();
    }
}
