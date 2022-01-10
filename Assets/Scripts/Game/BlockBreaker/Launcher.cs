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
        [SerializeField] AimLine aimLine;

        [SerializeField] float moveSpeed = 5f;
        [SerializeField] bool moveEnable = true;

        Mouse mouse = Mouse.current;//鼠标
        Keyboard keyboard = Keyboard.current;//键盘


        Vector2 worldPosLeftBottom;
        Vector2 worldPosTopRight;
        void Start()
        {
            worldPosLeftBottom = camera.ViewportToWorldPoint(Vector2.zero);
            worldPosTopRight = camera.ViewportToWorldPoint(Vector2.one);

            Debug.Log(worldPosLeftBottom);
            Debug.Log(worldPosTopRight);
        }

        void Update()
        {

            if(mouse != null)
            {
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    CreateSpawner();
                }
            }

            if (keyboard != null)
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

        /// <summary>
        /// 限制移动
        /// </summary>
        /// <param name="trNeedLimit"></param>
        public void LimitPosition(Transform trNeedLimit)
        {
            trNeedLimit.position = new Vector3(Mathf.Clamp(trNeedLimit.position.x, worldPosLeftBottom.x, worldPosTopRight.x),
                                               Mathf.Clamp(trNeedLimit.position.y, worldPosLeftBottom.y + 0.3F, worldPosTopRight.y - 0.3F),
                                               trNeedLimit.position.z);
        }

        private void CreateSpawner()
        {
            GameObject go = Instantiate(ballObj, transform.position, Quaternion.identity);
            go.SetActive(true);
            go.transform.SetParent(ballRoot);
            go.transform.localScale = new Vector3(0.5f, 0.5f, 0.2f);

            Vector2 spawnVelocity = aimLine.GetLineRenderer().GetPosition(1) - aimLine.GetLineRenderer().GetPosition(0);
            Ball ball = go.GetComponent<Ball>();
            ball.InitialVelocity = spawnVelocity;
        }
    }
}