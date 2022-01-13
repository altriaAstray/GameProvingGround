using UnityEngine;
using System.Collections;

namespace GameLogic
{
    /// <summary>
    /// 功能：贝塞尔曲线
    /// 创建者：长生
    /// 日期：2021年11月23日11:22:09
    /// </summary>
    
    [ExecuteInEditMode]
    public class Bezier : MonoBehaviour
    {

        [SerializeField]
        Transform[] points;
        public Transform[] Points
        {
            get
            {
                return this.points;
            }
        }

        void Update()
        {
            int count = 20;
            Vector3 prev_pos = this.points[0].position;
            for (int i = 0; i <= count; ++i)
            {
                Vector3 to = bezier(i / (float)count);
                prev_pos = to;
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < this.points.Length; ++i)
            {
                if (i < this.points.Length - 1)
                {
                    Vector3 current = this.points[i].position;
                    Vector3 next = this.points[i + 1].position;
                    //Gizmos.DrawLine(current, next);
                }
            }
        }

        /// <summary>
        /// 贝塞尔（根据顶点数判断贝塞尔类型）
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 bezier(float t)
        {
            if (this.points.Length == 3)
            {
                return bezier2(t);
            }
            else if (this.points.Length == 4)
            {
                return bezier3(t);
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 根据三个顶点生成的贝塞尔曲线
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 bezier2(float t)
        {
            Vector3 a = this.points[0].position;
            Vector3 b = this.points[1].position;
            Vector3 c = this.points[2].position;

            Vector3 aa = a + (b - a) * t;
            Vector3 bb = b + (c - b) * t;
            return aa + (bb - aa) * t;
        }

        /// <summary>
        /// 根据四个顶点生成的贝塞尔曲线
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Vector3 bezier3(float t)
        {
            Vector3 a = this.points[0].position;
            Vector3 b = this.points[1].position;
            Vector3 c = this.points[2].position;
            Vector3 d = this.points[3].position;

            Vector3 aa = a + (b - a) * t;
            Vector3 bb = b + (c - b) * t;
            Vector3 cc = c + (d - c) * t;

            Vector3 aaa = aa + (bb - aa) * t;
            Vector3 bbb = bb + (cc - bb) * t;
            return aaa + (bbb - aaa) * t;
        }
    }

}