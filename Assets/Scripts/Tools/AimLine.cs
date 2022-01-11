using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 功能：瞄准线
/// 创建者：长生
/// 日期：2022年1月8日14:35:42
/// </summary>

namespace GameLogic
{
    public class AimLine : MonoBehaviour
    {
        [SerializeField] LineRenderer line_renderer;
        [SerializeField] LineRenderer line_renderer_refraction;
        [SerializeField] Camera camera;
        [SerializeField] Text text;
        [SerializeField] RectTransform arrow;

        public LayerMask mask = (1 << 8); //打开第8的层

        Pointer pointer = Pointer.current;//鼠标指针
        enum Direction
        {
            Top,        //方向 上
            Bottom,     //方向 下
            Left,       //方向 左
            Right,      //方向 右
        };

        void Start()
        {
            this.line_renderer.positionCount = 2;
            this.line_renderer_refraction.positionCount = 2;
        }

        void Update()
        {
            ChangeLine();
        }

        /// <summary>
        /// 改变线
        /// </summary>
        private void ChangeLine()
        {
            line_renderer.SetPosition(0, this.transform.position);

            
            Vector3 mousePosition = pointer.position.ReadValue();
            text.text = mousePosition.ToString();

            Vector3 screenPos = camera.ScreenToWorldPoint(mousePosition);
            Vector2 screenPos2 = screenPos;

            Vector2 startPos = this.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(startPos, screenPos2 - startPos,500f, mask);
            
            if(hit.collider != null)
            {
                if(hit.collider.tag == "Wall" || hit.collider.tag == "Wall2")
                {
                    line_renderer.SetPosition(1, hit.point);

                }
            }

            //line_renderer_refraction.SetPosition(0, hit.point);
            //Vector3 incomingVec = hit.point - startPos;
            //Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            //Debug.DrawRay(hit.point, reflectVec, Color.green);
            //RaycastHit2D hit2 = Physics2D.Raycast(hit.point, reflectVec, 500f, mask);
            
            //if (hit.collider != null)
            //{
            //    if (hit.collider.tag == "Wall" || hit.collider.tag == "Wall2")
            //    {
            //        line_renderer_refraction.SetPosition(1, hit2.point);
            //    }
            //}
        }

        public LineRenderer GetLineRenderer()
        {
            return line_renderer;
        }
    }
}