using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：块管理器
    /// 创建人：长生
    /// 时间：2022年1月10日17:04:22
    /// </summary>
    public class BlockMgr : SingleToneManager<BlockMgr>
    {
        public Canvas canvas;
        public Text totalScoreText;                                     //积分文本显示
        public Text numberOfAmmunitionMaxText;                          //弹珠总数显示
        public Vector2 screenSize;                                      //屏幕大小
        public Transform blockRoot;                                     //方块根节点
        public Transform gameOverWall;                                  //用于结束游戏判断
        RectTransform rectTransform;                                    //用于查看分辨率

        int stage;                  //阶段
        int totalScore;             //总分

        public List<BlockBase> blockBases = new List<BlockBase>();      

        float blockSize = 120f;                                         //方块大小
        int blockCount_X = 0;                                           //方块X最大数量
        int blockCount_Y = 0;                                           //方块Y最大数量

        public Vector2 stratPos;

        [SerializeField] int numberOfAmmunitionMax = 1;                 //弹珠总数量
        [SerializeField] int numberOfAmmunition = -1;                   //弹珠数量

        public void Start()
        {
            if (AudioMgr.Instance != null)
            {
                List<int> musicKey = AudioMgr.Instance.GetMusicKeyByType(2);
                AudioMgr.Instance.RandomPlayBGM(musicKey);
            }

            rectTransform = canvas.transform as RectTransform;
            screenSize = rectTransform.rect.size;

            blockCount_X = (int)Math.Floor(screenSize.x / blockSize);
            blockCount_Y = (int)Math.Floor(screenSize.y / blockSize);

            float y = ((float)screenSize.y / (float)blockSize - (float)blockCount_Y) * 120 / 2;

            stratPos = new Vector2((screenSize.x / 2) - 200 - 60, (screenSize.y / 2) - 60 - y);

            CreateBlock();
        }

        //获得总分
        public int GetTotalScore()
        {
            return totalScore;
        }

        //设置总分
        public void SetTotalScore(int value)
        {
            totalScore = value;
        }

        public void AddNumberOfAmmunition()
        {
            numberOfAmmunitionMax++;
        }

        public void SetNumberOfAmmunition(int value)
        {
            numberOfAmmunition = value;
        }
        public int GetNumberOfAmmunition()
        {
            return numberOfAmmunition;
        }
        public int GetNumberOfAmmunitionMax()
        {
            return numberOfAmmunitionMax;
        }

        //方块生成
        public void CreateBlock()
        {            
            for(int i = 0; i < blockCount_Y; i++)
            {
                if(UnityEngine.Random.Range(0, 2) == 0)
                {
                    Vector3 pos = new Vector3(stratPos.x, stratPos.y - (i * 120), 0);

                    GameObject gameObject = Instantiate(blockBases[BlockRandom()].gameObject);
                    gameObject.transform.SetParent(blockRoot);
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                    int value = UnityEngine.Random.Range(0, 10);
                    if (value < 3)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(0, 5);
                    }
                    else if (value < 7)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(5, 10);
                    }
                    else if (value <= 10)
                    {
                        value = numberOfAmmunitionMax * 2;
                    }

                    gameObject.GetComponent<BlockBase>().SetPoint(value);
                }
            }
        }

        /// <summary>
        /// 随机方块
        /// </summary>
        /// <returns></returns>
        int BlockRandom()
        {
            int result = 0;

            int value = UnityEngine.Random.Range(0, 1000);

            if (value < 450)
            {
                result = 0;
            }
            else if (value < 850)
            {
                result = 1;
            }
            else if (value <= 1000)
            {
                result = UnityEngine.Random.Range(2, blockBases.Count);
            }

            return result;
        }

        //方块移动
        public void MoveBlock()
        {
            for (int i = 0; i < blockRoot.childCount; i++)
            {
                Transform @transform = blockRoot.GetChild(i);
                Vector3 pos = @transform.GetComponent<RectTransform>().localPosition;
                @transform.GetComponent<RectTransform>().localPosition = new Vector3(pos.x - 120, pos.y, pos.z);

                if(@transform.GetComponent<RectTransform>().localPosition.x < gameOverWall.localPosition.x)
                {
                    GameOver();
                    return;
                }
            }
        }

        public void FixedUpdate()
        {
            totalScoreText.text = totalScore.ToString();
            numberOfAmmunitionMaxText.text = numberOfAmmunitionMax.ToString();
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public void GameOver()
        {
            numberOfAmmunitionMax = 1;
            totalScore = 0;
            Tools.ClearChild(blockRoot);
            
            Launcher.Instance.transform.localPosition = new Vector3(Launcher.Instance.transform.localPosition.x,
                0,
                Launcher.Instance.transform.localPosition.z);
            Debug.Log("游戏结束");
        }
    }
}