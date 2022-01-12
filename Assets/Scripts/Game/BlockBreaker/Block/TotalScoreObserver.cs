using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.BlockBreaker
{
    public abstract class TotalScoreObserver : MonoBehaviour
    {
        protected TotalScoreSubject subject;
        public abstract void UpdateData();
    }
}