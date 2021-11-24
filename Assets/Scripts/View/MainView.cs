using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class MainView : MonoBehaviour
    {
        private void Start()
        {
            
        }

        public void LoadPreScene()
        {
            SceneMgr.Instance.LoadPreScene();
        }
    }
}