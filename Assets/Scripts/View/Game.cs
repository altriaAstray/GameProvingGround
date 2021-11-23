using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class Game : MonoBehaviour
    {
        private void Start()
        {
            Invoke("Play", 3f);
        }

        public void Play()
        {
            List<int> musicKey = AudioMgr.Instance.GetMusicKey();
            AudioMgr.Instance.PlayBGM(musicKey[0]);
        }
    }
}

