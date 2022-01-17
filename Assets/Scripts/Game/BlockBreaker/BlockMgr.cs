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
        public Text highestInHistoryText;                               //历史最高分数显示
        public Text attackText;                                         //弹珠总数显示
        public Vector2 screenSize;                                      //屏幕大小
        public Transform blockRoot;                                     //方块根节点
        public Transform ballRoot;                                     //球根节点
        public Transform gameOverWall;                                  //用于结束游戏判断
        public GameObject exitMenu;                                     //退出菜单
        public GameObject gameoverMenu;                                 //游戏结束菜单
        RectTransform rectTransform;                                    //用于查看分辨率

        int totalScore;             //总分
        int highestInHistory;       //历史最高
        int rows;                   //行数
        bool allowLoad;             //允许读档

        public List<BlockBase> blockBases = new List<BlockBase>();      

        float blockSize = 120f;                                         //方块大小
        int blockCount_X = 0;                                           //方块X最大数量
        int blockCount_Y = 0;                                           //方块Y最大数量

        public Vector2 stratPos;

        [SerializeField] int numberOfAmmunitionMax = 1;                 //弹珠总数量
        [SerializeField] int attack = 1;                                //弹珠攻击力
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

            if (DataMgr.Instance != null)
            {
                highestInHistory = DataMgr.Instance.GetSaveData()[100005].Value;
            }
            CreateBlock();
            allowLoad = true;
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
                    block.SetRow();

                    if (block.blockAttributes != BlockAttributes.None)
                    {
                        int value = 0;

                        value = ComputePoint(block.blockAttributes);
                        gameObject.GetComponent<BlockBase>().SetPoint(value);
                    }
                }
            }
        }

        public int ComputePoint(BlockAttributes type)
        {
            int value = 0;

            int attackMax = numberOfAmmunitionMax * attack;

            switch (type)
            {
                case BlockAttributes.SoilBlock:
                    value = (int)Math.Round(attackMax * 0.5f) + 1; 
                    break;
                case BlockAttributes.GrassBlock:
                    value = attackMax;
                    break;
                case BlockAttributes.WoodBlock:
                    value = attackMax + (int)Math.Round(attackMax * 0.5f);
                    break;
                case BlockAttributes.BrickBlock:
                    value = attackMax + (int)Math.Round(attackMax * 1f);
                    break;
                case BlockAttributes.IronBlock:
                    value = attackMax + (int)Math.Round(attackMax * 1.5f);
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

            if (value < 480)
            {
                result = 0;
            }
            else if (value < 750)
            {
                result = 1;
            }
            else if (value < 960)
            {
                result = 2;
            }
            else if (value <= 970)
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
            rows = 0;

            for (int i = 0; i < blockRoot.childCount; i++)
            {
                Transform @transform = blockRoot.GetChild(i);
                BlockBase @base = blockRoot.GetChild(i).GetComponent<BlockBase>();
                
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

                    if(@base != null)
                    {
                        @base.SetRow();

                        if (rows < @base.GetRow())
                        {
                            rows = @base.GetRow();
                        }
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
            highestInHistoryText.text = "x " + highestInHistory.ToString();
        }
        //-------------------------------------------


        /// <summary>
        /// 保存历史最高数据
        /// </summary>
        /// <returns></returns>
        IEnumerator OnSaveHighestInHistory()
        {
            if (DataMgr.Instance != null && highestInHistory < totalScore)
            {
                highestInHistory = totalScore;
                DataMgr.Instance.GetSaveData()[100005].Value = highestInHistory;
                DataMgr.Instance.SetSave(DataMgr.Instance.GetSaveData()[100005]);
            }

            yield return null;
        }

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
            gameStatic = GameStatic.StopPlay;

            if (AudioMgr.Instance != null)
                AudioMgr.Instance.PlaySound(100027);
            
            gameoverMenu.SetActive(true);
            finalScoreText.text = totalScore.ToString();

            StartCoroutine(OnSaveHighestInHistory());

            numberOfAmmunitionMax = 1;
            attack = 1;
            totalScore = 0;
            numberOfAmmunition = -1;

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
            gameStatic = GameStatic.StopPlay;

            numberOfAmmunitionMax = 1;
            numberOfAmmunition = -1;
            attack = 1;
            totalScore = 0;
            rows = 0;
            Tools.ClearChild(blockRoot);
            Tools.ClearChild(ballRoot);

            Launcher.Instance.transform.localPosition = new Vector3(Launcher.Instance.transform.localPosition.x,
                0,
                Launcher.Instance.transform.localPosition.z);

            exitMenu.SetActive(false);
            gameoverMenu.SetActive(false);

            CreateBlock();

            gameStatic = GameStatic.Play;
        }

        /// <summary>
        /// 获取存档数据
        /// </summary>
        public void GetSave()
        {
            if(allowLoad == false)
            {
                Debug.Log("不许读档");
                return;
            }
            StartCoroutine(OnGetSave());
        }

        /// <summary>
        /// 获取存档数据
        /// </summary>
        IEnumerator OnGetSave()
        {
            gameStatic = GameStatic.StopPlay;
            allowLoad = false;
            Tools.ClearChild(blockRoot);
            Tools.ClearChild(ballRoot);

            yield return new WaitForSeconds(1.0f);

            if (DataMgr.Instance != null)
            {
                totalScore = DataMgr.Instance.GetSaveData()[100001].Value;
                if (totalScore <= 0)
                {
                    totalScore = 0;
                }

                int value = DataMgr.Instance.GetSaveData()[100002].Value;
                if (value <= 0)
                {
                    value = 1;
                }

                attack = DataMgr.Instance.GetSaveData()[100003].Value;
                if (attack <= 0)
                {
                    attack = 1;
                }

                numberOfAmmunitionMax = DataMgr.Instance.GetSaveData()[100004].Value;
                if (numberOfAmmunitionMax <= 0)
                {
                    numberOfAmmunitionMax = 1;
                }

                for (int i = 0; i < value; i++)
                {
                    MoveBlock();
                    CreateBlock();
                }
            }

            allowLoad = true;
            CloseExitMenu();
        }

        /// <summary>
        /// 保存存档
        /// </summary>
        public void SetSave()
        {
            if (DataMgr.Instance != null)
            {
                if(totalScore > 0)
                {
                    DataMgr.Instance.GetSaveData()[100001].Value = totalScore;
                    DataMgr.Instance.SetSave(DataMgr.Instance.GetSaveData()[100001]);
                }

                DataMgr.Instance.GetSaveData()[100002].Value = rows;
                DataMgr.Instance.SetSave(DataMgr.Instance.GetSaveData()[100002]);

                if (attack > 0)
                {
                    DataMgr.Instance.GetSaveData()[100003].Value = attack;
                    DataMgr.Instance.SetSave(DataMgr.Instance.GetSaveData()[100003]);
                }

                if (numberOfAmmunitionMax > 0)
                {
                    DataMgr.Instance.GetSaveData()[100004].Value = numberOfAmmunitionMax;
                    DataMgr.Instance.SetSave(DataMgr.Instance.GetSaveData()[100004]);
                }
            }
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