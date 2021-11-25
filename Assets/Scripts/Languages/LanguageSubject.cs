using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    //---------------------------------
    //  <author> 杨长生 </author>
    //  <descride> 被观察者 - 语言 </descride>	
    //  <time> [2020-4-21 13:37:19] </time>	
    //---------------------------------
    public class LanguageSubject
    {
        private List<LanguageObserver> observers = new List<LanguageObserver>();

        public void SetState()
        {
            NotifyAllObservers();
        }

        public void Attach(LanguageObserver observer)
        {
            observers.Add(observer);
        }

        public void NotifyAllObservers()
        {
            foreach(LanguageObserver observer in observers)
            {
                if(observer != null)
                {
                    observer.UpdateData();
                }
            }
        }

        public void Clear()
        {
            observers.Clear();
        }
    }
}

