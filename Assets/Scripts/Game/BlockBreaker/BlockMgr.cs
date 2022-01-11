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
        public Text totalScoreText;
        public Text numberOfAmmunitionMaxText;
        public Vector2 screenSize;
        public Transform blockRoot;
        RectTransform rectTransform;

        int stage;                  //阶段
        int totalScore;             //总分

        public List<BlockBase> blockBases = new List<BlockBase>();

        float blockSize = 120f;
        int blockCount_X = 0;
        int blockCount_Y = 0;

        public Vector2 stratPos;

        [SerializeField] int numberOfAmmunitionMax = 1;
        [SerializeField] int numberOfAmmunition = -1;

        public void Start()
        {
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
                    GameObject gameObject = Instantiate(blockBases[UnityEngine.Random.Range(0, blockBases.Count)].gameObject);
                    gameObject.transform.SetParent(blockRoot);
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
                    gameObject.GetComponent<BlockBase>().SetPoint(numberOfAmmunitionMax + UnityEngine.Random.Range(0, 4));
                }
            }
        }

        //方块移动
        public void MoveBlock()
        {
            for (int i = 0; i < blockRoot.childCount; i++)
            {
                Transform @transform = blockRoot.GetChild(i);
                Vector3 pos = @transform.GetComponent<RectTransform>().localPosition;
                @transform.GetComponent<RectTransform>().localPosition = new Vector3(pos.x - 120, pos.y, pos.z);
            }
        }

        //方块计分

        public void FixedUpdate()
        {
            totalScoreText.text = totalScore.ToString();
            numberOfAmmunitionMaxText.text = numberOfAmmunitionMax.ToString();
        }
    }
}