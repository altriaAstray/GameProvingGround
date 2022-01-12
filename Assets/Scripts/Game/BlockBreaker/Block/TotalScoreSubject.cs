using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.BlockBreaker
{
    public class TotalScoreSubject : MonoBehaviour
    {
        private List<TotalScoreObserver> observers = new List<TotalScoreObserver>();

        public void SetState()
        {
            NotifyAllObservers();
        }

        public void Attach(TotalScoreObserver observer)
        {
            observers.Add(observer);
        }

        public void NotifyAllObservers()
        {
            foreach (TotalScoreObserver observer in observers)
            {
                if (observer != null)
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