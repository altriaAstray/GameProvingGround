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
    public class AttackBlock : BlockBase
    {

        private void Start()
        {
            isDestroy = false;
            blockType = BlockType.AttackBall;
            point = 1;
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
                BlockMgr.Instance.SetAttack(BlockMgr.Instance.GetAttack() + 1);
                BlockMgr.Instance.SetTotalScore(BlockMgr.Instance.GetTotalScore() + 100);
                DestroyObj();
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