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
        public Vector2 screenSize;                                      //屏幕大小
        public Transform blockRoot;                                     //方块根节点
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
                if(UnityEngine.Random.Range(0, 2) == 0)
                {
                    Vector3 pos = new Vector3(stratPos.x, stratPos.y - (i * 120), 0);

                    GameObject gameObject = Instantiate(blockBases[BlockRandom()].gameObject);
                    gameObject.transform.SetParent(blockRoot);
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

                    int value = UnityEngine.Random.Range(0, 1000);
                    if (value < 400)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(3, 5);
                    }
                    else if (value < 800)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(5, 10);
                    }
                    else if (value < 900)
                    {
                        value = numberOfAmmunitionMax + UnityEngine.Random.Range(10, 20);
                    }
                    else if (value <= 1000)
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
            else if (value < 700)
            {
                result = 1;
            }
            else if (value < 950)
            {
                result = 2;
            }

            else if (value <= 1000)
            {
                result = 3;
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
        }

        public GameStatic GetGameStatic()
        {
            return gameStatic;
        }

        public void OpenExitMenu()
        {
            exitMenu.SetActive(true);
            gameStatic = GameStatic.StopPlay;
        }

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
            gameStatic = GameStatic.StopPlay;
            
            gameoverMenu.SetActive(true);
            finalScoreText.text = totalScore.ToString();

            numberOfAmmunitionMax = 1;
            totalScore = 0;
            Tools.ClearChild(blockRoot);
            
            Launcher.Instance.transform.localPosition = new Vector3(Launcher.Instance.transform.localPosition.x,
                0,
                Launcher.Instance.transform.localPosition.z);
            Debug.Log("游戏结束");
        }

        public void GoBack()
        {
            AudioMgr.Instance.StopBGM();
            SceneMgr.Instance.LoadScene("GameStartScene");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}