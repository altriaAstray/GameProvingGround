using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：异形块
    /// 创建者：长生
    /// 时间：2021年11月20日10:32:26
    /// </summary>
    
    public class SpecialShapedBlock : BlockBase
    {
        [SerializeField] UIShiny uiShiny;
        [SerializeField] SpecialShapedType specialShapedType;
        [SerializeField] PolygonCollider2D polygonCollider;
        public List<GameObject> specialShapeds = new List<GameObject>();

        private void Start()
        {
            isDestroy = false;
            blockType = BlockType.SpecialShapedBlock;
            blockAttributes = BlockAttributes.GrassBlock;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            Vector2[] points = new Vector2[3];
            int value = Random.Range(0, 1000);
            //icon.sprite = ResourcesMgr.Instance.LoadAsset<Sprite>("Images/Block/Grass");
            if (value < 250)
            {
                specialShapedType = SpecialShapedType.UpperLeft;
                specialShapeds[0].gameObject.SetActive(true);
                uiShiny = specialShapeds[0].GetComponent<UIShiny>();
                pointText = specialShapeds[0].transform.Find("Point").GetComponent<Text>();
                points[0] = new Vector2(-60, 60);
                points[1] = new Vector2(-60, -60);
                points[2] = new Vector2(60, 60);
            }
            else if (value < 500)
            {
                specialShapedType = SpecialShapedType.LowerLeft;
                specialShapeds[1].gameObject.SetActive(true);
                uiShiny = specialShapeds[1].GetComponent<UIShiny>();
                pointText = specialShapeds[1].transform.Find("Point").GetComponent<Text>();
                points[0] = new Vector2(-60, 60);
                points[1] = new Vector2(-60, -60);
                points[2] = new Vector2(60, -60);
            }
            else if (value < 750)
            {
                specialShapedType = SpecialShapedType.UpperRight;
                specialShapeds[2].gameObject.SetActive(true);
                uiShiny = specialShapeds[2].GetComponent<UIShiny>();
                pointText = specialShapeds[2].transform.Find("Point").GetComponent<Text>();
                points[0] = new Vector2(60, 60);
                points[1] = new Vector2(-60, 60);
                points[2] = new Vector2(60, -60);
            }
            else if (value <= 1000)
            {
                specialShapedType = SpecialShapedType.LowerRight;
                specialShapeds[3].gameObject.SetActive(true);
                uiShiny = specialShapeds[3].GetComponent<UIShiny>();
                pointText = specialShapeds[3].transform.Find("Point").GetComponent<Text>();
                points[0] = new Vector2(60, 60);
                points[1] = new Vector2(-60, -60);
                points[2] = new Vector2(60, -60);
            }
            polygonCollider.points = points;
        }

        public void Update()
        {
            if(pointText != null)
            {
                pointText.text = point.ToString();
            }
        }

        public override void DestroyObj()
        {
            Destroy(this.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                uiShiny.Play();
                SetPoint(point - BlockMgr.Instance.GetAttack());
                BlockMgr.Instance.SetTotalScore(BlockMgr.Instance.GetTotalScore() + BlockMgr.Instance.GetAttack());
                if (point <= 0)
                {
                    DestroyObj();
                }
            }
        }

        //设置点数
        public override void SetPoint(int value)
        {
            point = value;
        }

        public override void SetRow()
        {
            row += 1;
        }

        public override int GetRow()
        {
            return row;
        }
    }

    
}