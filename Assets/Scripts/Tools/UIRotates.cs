using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 功能：UI旋转
    /// 创建者：长生
    /// 日期：2022年1月8日14:35:42
    /// </summary>
    public class UIRotates : MonoBehaviour
    {
        enum RotatesType
        {
            FollowMouse,
        }

        [SerializeField] RotatesType rotatesType;           //旋转类型
        [SerializeField] Camera camera;                     //摄像机
        public RectTransform uGUICanvas;                    //对应旋转体负载的Canvas
        void Start()
        {

        }

        void Update()
        {
            //跟随鼠标旋转
            if(rotatesType == RotatesType.FollowMouse)
            {
                Vector3 mousePos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(uGUICanvas, new Vector2(Input.mousePosition.x, Input.mousePosition.y), camera, out mousePos);
                float z;
                if (mousePos.x > transform.position.x)
                {
                    z = -Vector3.Angle(Vector3.up, mousePos - transform.position);
                }
                else
                {
                    z = Vector3.Angle(Vector3.up, (mousePos - transform.position));
                }

                //把旋转角度赋给旋转体，进行对应旋转
                transform.localRotation = Quaternion.Euler(0, 0, z);
            }
        }
    }
}
