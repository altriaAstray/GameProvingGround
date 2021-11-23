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
        private Multilingual language;

        public Multilingual GetState()
        {
            return language;
        }

        public void SetState(Multilingual state)
        {
            this.language = state;
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
                observer.UpdateData();
            }
        }
    }
}

