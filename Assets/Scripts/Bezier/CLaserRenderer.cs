using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GameLogic
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(CBezier))]
    public class CLaserRenderer : MonoBehaviour
    {
        // 节点数越多，曲线越柔和。
        [SerializeField]
        int node_count;

        LineRenderer line_renderer;
        CBezier bezier;

        void Awake()
        {
            this.line_renderer = gameObject.GetComponent<LineRenderer>();
            this.bezier = gameObject.GetComponent<CBezier>();
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
