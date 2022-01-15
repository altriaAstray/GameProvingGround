using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：发射器
    /// 创建者：长生
    /// 日期：2022年1月9日11:06:18
    /// </summary>
    public class Launcher : SingleToneManager<Launcher>
    {
        [SerializeField] Camera camera;
        [SerializeField] GameObject ballObj;
        [SerializeField] Transform ballRoot;
        [SerializeField] Transform launcherPos;
        [SerializeField] AimLine aimLine;

        [SerializeField] float moveSpeed = 5f;
        
        Mouse mouse = Mouse.current;//鼠标
        Keyboard keyboard = Keyboard.current;//键盘

        Vector2 worldPosLeftBottom;
        Vector2 worldPosTopRight;

        [SerializeField] bool createEnable = false;
        Vector2 spawnVelocity;
        float timeMax = 0.1f;
        float time;

        void Start()
        {
            worldPosLeftBottom = camera.ViewportToWorldPoint(Vector2.zero);
            worldPosTopRight = camera.ViewportToWorldPoint(Vector2.one);
        }

        void Update()
        {
            //球存在的情况下不显示瞄准线
            if (ballRoot.childCount <= 0)
            {
                if(aimLine.gameObject.activeSelf == false)
                    aimLine.gameObject.SetActive(true);
            }
            else
            {
                if (aimLine.gameObject.activeSelf == true)
                    aimLine.gameObject.SetActive(false);
            }

            if(BlockMgr.Instance.GetGameStatic() == GameStatic.Play)
            {
                //瞄准线
                if (mouse != null && ballRoot.childCount <= 0)
                {
                    if (mouse.leftButton.wasPressedThisFrame)
                    {
                        spawnVelocity = aimLine.GetLineRenderer().GetPosition(1) - aimLine.GetLineRenderer().GetPosition(0);
                        createEnable = true;
                        BlockMgr.Instance.SetNumberOfAmmunition(BlockMgr.Instance.GetNumberOfAmmunitionMax());
                    }
                }

                //球存在的情况下不能移动
                if (keyboard != null && ballRoot.childCount <= 0)
                {
                    if (keyboard.wKey.isPressed)
                    {
                        transform.Translate(0, moveSpeed * Time.deltaTime, 0);
                    }

                    if (keyboard.sKey.isPressed)
                    {
                        transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
                    }

                    LimitPosition(transform);
                }
            }
            else
            {
                aimLine.gameObject.SetActive(false);
            }

            

            if(createEnable && BlockMgr.Instance.GetGameStatic() == GameStatic.Play)
            {
                time += Time.deltaTime;
                if(time > timeMax)
                {
                    time = 0;
                    
                    if(BlockMgr.Instance.GetNumberOfAmmunition() > 0)
                    {
                        CreateBall();
                        BlockMgr.Instance.SetNumberOfAmmunition(BlockMgr.Instance.GetNumberOfAmmunition() - 1);
                    }
                    else if(BlockMgr.Instance.GetNumberOfAmmunition() == 0 && ballRoot.childCount <= 0)
                    {
                        BlockMgr.Instance.SetNumberOfAmmunition(BlockMgr.Instance.GetNumberOfAmmunition() - 1);
                        BlockMgr.Instance.MoveBlock();
                        BlockMgr.Instance.CreateBlock();
                    }
                }
            }
        }

        /// <summary>
        /// 获得根目录
        /// </summary>
        /// <returns></returns>
        public Transform GetBallRoot()
        {
            return ballRoot;
        }

        /// <summary>
        /// 限制移动
        /// </summary>
        /// <param name="trNeedLimit"></param>
        public void LimitPosition(Transform trNeedLimit)
        {
            trNeedLimit.position = new Vector3(Mathf.Clamp(trNeedLimit.position.x, worldPosLeftBottom.x, worldPosTopRight.x),
                                               Mathf.Clamp(trNeedLimit.position.y, worldPosLeftBottom.y + 1.5F, worldPosTopRight.y - 1.5F),
                                               trNeedLimit.position.z);
        }

        /// <summary>
        /// 创建球
        /// </summary>
        private void CreateBall()
        {
            GameObject go = Instantiate(ballObj, transform.position, Quaternion.identity);
            go.SetActive(true);
            go.transform.SetParent(ballRoot);
            go.transform.localScale = new Vector3(1f, 1f, 1f);

            Ball ball = go.GetComponent<Ball>();
            ball.InitialVelocity = spawnVelocity;
        }
    }
}