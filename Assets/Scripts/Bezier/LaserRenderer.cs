using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GameLogic
{
    /// <summary>
    /// 功能：贝塞尔曲线渲染用
    /// 创建者：长生
    /// 日期：2022年1月13日13:07:13
    /// </summary>
    
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(Bezier))]
    public class LaserRenderer : MonoBehaviour
    {
        // 节点数越多，曲线越柔和。
        [SerializeField]
        int node_count;

        LineRenderer line_renderer;
        Bezier bezier;

        void Awake()
        {
            this.line_renderer = gameObject.GetComponent<LineRenderer>();
            this.bezier = gameObject.GetComponent<Bezier>();
            set_vertex_count(node_count + 1);
        }


        void set_vertex_count(int count)
        {
            this.line_renderer.positionCount = count;
        }


        // 每帧调用一次更新
        void Update()
        {
            for (int i = 0; i <= node_count; ++i)
            {
                Vector3 to = this.bezier.bezier(i / (float)node_count);
                this.line_renderer.SetPosition(i, to);
            }
        }
    }

}
