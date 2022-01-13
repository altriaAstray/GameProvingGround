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
        [SerializeField] Text pointText;
        [SerializeField] UIShiny uiShiny;

        private void Start()
        {
            blockType = BlockType.SpecialShapedBlock;
        }

        public void Update()
        {
            pointText.text = point.ToString();
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
                point--;
                BlockMgr.Instance.SetTotalScore(BlockMgr.Instance.GetTotalScore() + 1);
                if (point <= 0)
                {
                    DestroyObj();
                }
            }
        }

        public override void SetPoint(int value)
        {
            point = value;
        }
    }
}