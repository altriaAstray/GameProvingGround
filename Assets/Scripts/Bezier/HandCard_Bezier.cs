using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 功能：手牌渲染用（测试）
    /// 创建者：长生
    /// 日期：2022年1月13日13:07:13
    /// </summary>
    public class HandCard_Bezier : MonoBehaviour
    {
        [SerializeField] int node_count = 7;
        [SerializeField] int node_current_count;

        [SerializeField] LineRenderer line_renderer;
        [SerializeField] Bezier bezier;
        [SerializeField] GameObject CardPrefab;
        [SerializeField] GameObject Point_1;
        [SerializeField] GameObject Point_2;
        [SerializeField] GameObject Point_3;
        [SerializeField] Transform parent;

        [SerializeField] PlayerType playerType;
        [SerializeField] float bentAngle = 27f;     //弯曲角度

        List<GameObject> cards = new List<GameObject>();

        void Awake()
        {
            node_current_count = node_count;
            OnCreateGO();
        }

        void Start()
        {
            
        }

        /// <summary>
        /// 设置点的数量
        /// </summary>
        /// <param name="count"></param>
        void set_vertex_count(int count)
        {
            this.line_renderer.positionCount = count;
        }

        /// <summary>
        /// 创建游戏对象（并设置对象位置）
        /// </summary>
        public void OnCreateGO()
        {
            var offsetZ = -1;
            var layerZ = 0;
            for (int i = 0; i < node_count; i++)
            {
                GameObject go = Instantiate(CardPrefab, transform.position, Quaternion.identity);
                go.transform.SetParent(parent);

                var localCardPosition = go.transform.localPosition;
                localCardPosition.z = layerZ;
                go.transform.localPosition = localCardPosition;
                go.transform.localScale = new Vector3(0.75F, 1F, 0.05F);
                cards.Add(go);

                layerZ += offsetZ;
            }

            Point_1.transform.localPosition = new Vector3(-0.75f * (node_count / 2), 0,0);
            Point_2.transform.localPosition = new Vector3(0, node_count * 0.1f, 0);
            Point_3.transform.localPosition = new Vector3(0.75f * (node_count / 2), 0,0);

            set_vertex_count(node_count);
        }

        /// <summary>
        /// 清除并重新生成
        /// </summary>
        public void OnCloseGO()
        {
            cards.Clear();
            Tools.ClearChild(parent);

            OnCreateGO();
            //set_vertex_count(node_count);
        }

        // 每帧调用一次更新
        void Update()
        {
            if(cards.Count <= 0)
            {
                return;
            }

            if(node_current_count != node_count)
            {
                node_current_count = node_count;
                OnCloseGO();
            }

            float fullAngle = -bentAngle;
            float anglePerCard = fullAngle / cards.Count;
            var firstAngle = CalcFirstAngle(fullAngle);
            int pivotLocationFactor = 1;
            //判断玩家类型
            if (playerType == PlayerType.Player)
            {
                pivotLocationFactor = 1;
            }
            else
            {
                pivotLocationFactor = -1;
            }

            for (int i = 0; i < node_count; ++i)
            {
                float count = (float)node_count - 1;
                Vector3 to = this.bezier.bezier(i / count);

                if(node_count > 1)
                {
                    //设置卡牌位置
                    this.line_renderer.SetPosition(i, to);
                    cards[i].transform.position = this.line_renderer.GetPosition(i);
                    //设置卡牌角度
                    var angleTwist = (firstAngle + i * anglePerCard) * pivotLocationFactor;
                    var zAxisRot = pivotLocationFactor == 1 ? 0 : 180;
                    var rotation = new Vector3(0, 0, angleTwist - zAxisRot);
                    cards[i].transform.eulerAngles = rotation;
                }
            }
        }
        

        /// <summary>
        /// Calculus of the angle of the first card.
        /// 第一张牌的角度的微积分。
        /// </summary>
        /// <param name="fullAngle">全角</param>
        /// <returns></returns>
        static float CalcFirstAngle(float fullAngle)
        {
            //神奇的数学因子
            float magicMathFactor = 0.1f;
            float result = -(fullAngle / 2) + fullAngle * magicMathFactor;

            return result;
        }
    }

}