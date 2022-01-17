using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：增加球总数的块
    /// 创建者：长生
    /// 时间：2021年11月20日10:32:26
    /// </summary>
    public class AddBlock : BlockBase
    {

        private void Start()
        {
            isDestroy = false;
            blockType = BlockType.AddBall;
            point = 1;
            blockAttributes = BlockAttributes.None;
        }

        public override void Init()
        {

        }

        public override void DestroyObj()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                point--;
                BlockMgr.Instance.AddNumberOfAmmunition();
                BlockMgr.Instance.SetTotalScore(BlockMgr.Instance.GetTotalScore() + 2);
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