using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        public Text finalScoreText;                                     //积分文本显示
        public Text numberOfAmmunitionMaxText;                          //弹珠总数显示
        public Text attackText;                                         //弹珠总数显示
        public Vector2 screenSize;                                      //屏幕大小
        public Transform blockRoot;                                     //方块根节点
        public Transform ballRoot;                                     //球根节点
        public Transform gameOverWall;                                  //用于结束游戏判断
        public GameObject exitMenu;                                     //退出菜单
        public GameObject gameoverMenu;                                 //游戏结束菜单
        RectTransform rectTransform;                                    //用于查看分辨率

        int stage;                  //阶段
        int totalScore;             //总分

        public List<BlockBase> blockBases = new List<BlockBase>();      

        float blockSize = 120f;                                         //方块大小
        int blockCount_X = 0;                                           //方块X最大数量
        int blockCount_Y = 0;                                           //方块Y最大数量

        public Vector2 stratPos;

        [SerializeField] int numberOfAmmunitionMax = 1;                 //弹珠总数量
        [SerializeField] int attack = 1;                                //弹珠总数量
        [SerializeField] int numberOfAmmunition = -1;                   //弹珠数量

        Keyboard keyboard = Keyboard.current;//键盘

        GameStatic gameStatic;

        public void Start()
        {
            if (AudioMgr.Instance != null)
            {
                List<int> musicKey = AudioMgr.Instance.GetMusicKeyByType(2);
                AudioMgr.Instance.RandomPlayBGM(musicKey);
            }

            gameStatic = GameStatic.Play;

            rectTransform = canvas.transform as RectTransform;
            screenSize = rectTransform.rect.size;

            blockCount_X = (int)Math.Round(screenSize.x / blockSize);
            blockCount_Y = (int)Math.Round(screenSize.y / blockSize);

            float y = ((float)screenSize.y / (float)blockSize - (float)blockCount_Y) * 120 / 2;
            //设置起始位置
            stratPos = new Vector2((screenSize.x / 2) - 240 - 60, (screenSize.y / 2) - 60 - y - 120);

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

        /// <summary>
        /// 方块生成
        /// </summary>
        public void CreateBlock()
        {            
            for(int i = 0; i < blockCount_Y - 2; i++)
            {
                if(UnityEngine.Random.Range(0, 3) > 0)
                {
                    Vector3 pos = new Vector3(stratPos.x, stratPos.y - (i * 120), 0);

                    GameObject gameObject = Instantiate(blockBases[BlockRandom()].gameObject);
                    gameObject.transform.SetParent(blockRoot);
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                    BlockBase block = gameObject.GetComponent<BlockBase>();
                    block.Init();

                    if (block.blockAttributes != BlockAttributes.None)
                    {
                        int value = 0;

                        switch(block.blockAttributes)
                        {
                            case BlockAttributes.SoilBlock:
                                value = ComputePoint(1, 3, BlockAttributes.SoilBlock);
                                break;

                            case BlockAttributes.GrassBlock:
                                value = ComputePoint(2, 5, BlockAttributes.GrassBlock);
                                break;

                            case BlockAttributes.WoodBlock:
                                value = ComputePoint(2, 5, BlockAttributes.WoodBlock);
                                break;

                            case BlockAttributes.BrickBlock:

                                value = ComputePoint(3, 7, BlockAttributes.BrickBlock);
                                break;

                            case BlockAttributes.IronBlock:
                                value = ComputePoint(1, 5, BlockAttributes.IronBlock);
                                break;
                        }

                        gameObject.GetComponent<BlockBase>().SetPoint(value);
                    }
                }
            }
        }

        public int ComputePoint(int x,int y, BlockAttributes type)
        {
            int value = 0;
            

            switch (type)
            {
                case BlockAttributes.SoilBlock:
                case BlockAttributes.GrassBlock:
                case BlockAttributes.WoodBlock:
                case BlockAttributes.BrickBlock:
                    if (numberOfAmmunitionMax < 5)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(x, y);
                    }
                    else if (numberOfAmmunitionMax < 10)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(x, y) + attack;
                    }
                    else if (numberOfAmmunitionMax < 20)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(x, y) + attack * 2;
                    }
                    else if (numberOfAmmunitionMax < 40)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(x, y) + attack * 4;
                    }
                    else
                    {
                        value = (numberOfAmmunitionMax + UnityEngine.Random.Range(x, y)) * (attack);
                    }
                    break;
                case BlockAttributes.IronBlock:

                    if (numberOfAmmunitionMax < 10)
                    {
                        value = (numberOfAmmunitionMax * UnityEngine.Random.Range(x, y - 3)) + (attack);
                    }
                    else if (numberOfAmmunitionMax < 20)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(x , y - 2) + attack * 2;
                    }
                    else if (numberOfAmmunitionMax < 40)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(x + 1, y) + attack * 4;
                    }
                    else
                    {
                        value = (numberOfAmmunitionMax + UnityEngine.Random.Range(x + 2, y)) * (attack);
                    }
                    break;
            }
            return value;
        }

        /// <summary>
        /// 随机方块
        /// </summary>
        /// <returns></returns>
        int BlockRandom()
        {
            int result = 0;

            int value = UnityEngine.Random.Range(0, 1000);

            if (value < 350)
            {
                result = 0;
            }
            else if (value < 750)
            {
                result = 1;
            }
            else if (value < 850)
            {
                result = 2;
            }
            else if (value <= 920)
            {
                result = 3;
            }
            else if (value <= 1000)
            {
                result = 4;
            }

            return result;
        }

        //方块移动
        public void MoveBlock()
        {
            for (int i = 0; i < blockRoot.childCount; i++)
            {
                Transform @transform = blockRoot.GetChild(i);
                
                if(@transform.GetComponent<BlockBase>() != null && 
                    @transform.GetComponent<BlockBase>().isDestroy == true)
                {
                    Destroy(@transform.gameObject);
                }
                else
                {
                    Vector3 pos = @transform.GetComponent<RectTransform>().localPosition;
                    @transform.GetComponent<RectTransform>().localPosition = new Vector3(pos.x - 120, pos.y, pos.z);

                    if (@transform.GetComponent<RectTransform>().localPosition.x < gameOverWall.localPosition.x)
                    {
                        GameOver();
                        return;
                    }
                }
            }
        }
        //-------------------------------------------
        public void Update()
        {
            //键盘检测
            if (keyboard != null)
            {
                if (keyboard.escapeKey.wasPressedThisFrame)
                {
                    if (exitMenu != null)
                    {
                        if (exitMenu.activeSelf == true)
                        {
                            exitMenu.SetActive(false);
                            gameStatic = GameStatic.Play;
                        }
                        else
                        {
                            exitMenu.SetActive(true);
                            gameStatic = GameStatic.StopPlay;
                        }
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            totalScoreText.text = "x " + totalScore.ToString();
            numberOfAmmunitionMaxText.text = "x " + numberOfAmmunitionMax.ToString();
            attackText.text = "x " + attack.ToString();
        }
        //-------------------------------------------

        /// <summary>
        /// 设置攻击力
        /// </summary>
        /// <param name="value"></param>
        public void SetAttack(int value)
        {
            attack = value;
        }
        /// <summary>
        /// 获取攻击力
        /// </summary>
        /// <param name="value"></param>
        public int GetAttack()
        {
            return attack;
        }

        /// <summary>
        /// 获取游戏状态
        /// </summary>
        /// <returns></returns>
        public GameStatic GetGameStatic()
        {
            return gameStatic;
        }

        /// <summary>
        /// 打开<退出>菜单
        /// </summary>
        public void OpenExitMenu()
        {
            exitMenu.SetActive(true);
            gameStatic = GameStatic.StopPlay;
        }
        /// <summary>
        /// 关闭<退出>菜单
        /// </summary>
        public void CloseExitMenu()
        {
            exitMenu.SetActive(false);
            gameoverMenu.SetActive(false);
            gameStatic = GameStatic.Play;
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public void GameOver()
        {
            if (AudioMgr.Instance != null)
                AudioMgr.Instance.PlaySound(100027);

            gameStatic = GameStatic.StopPlay;
            
            gameoverMenu.SetActive(true);
            finalScoreText.text = totalScore.ToString();

            numberOfAmmunitionMax = 1;
            attack = 1;
            totalScore = 0;

            Tools.ClearChild(blockRoot);
            
            Launcher.Instance.transform.localPosition = new Vector3(Launcher.Instance.transform.localPosition.x,
                0,
                Launcher.Instance.transform.localPosition.z);
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public void ReStart()
        {
            gameStatic = GameStatic.Play;

            numberOfAmmunitionMax = 1;
            attack = 1;
            totalScore = 0;
            Tools.ClearChild(blockRoot);
            Tools.ClearChild(ballRoot);

            Launcher.Instance.transform.localPosition = new Vector3(Launcher.Instance.transform.localPosition.x,
                0,
                Launcher.Instance.transform.localPosition.z);

            exitMenu.SetActive(false);
            gameoverMenu.SetActive(false);

            CreateBlock();
        }

        /// <summary>
        /// 返回到主场景
        /// </summary>
        public void GoBack()
        {
            AudioMgr.Instance.StopBGM();
            SceneMgr.Instance.LoadScene("GameStartScene");
        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        public void Exit()
        {
            Application.Quit();
        }
    }
}