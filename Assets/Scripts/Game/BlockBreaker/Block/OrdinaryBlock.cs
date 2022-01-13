using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：普通块
    /// 创建者：长生
    /// 时间：2021年11月20日10:32:26
    /// </summary>
    
    public class OrdinaryBlock : BlockBase
    {
        [SerializeField] Text pointText;
        [SerializeField] UIEffect uiEffect;

        private void Start()
        {
            isDestroy = false;
            blockType = BlockType.OrdinaryBlock;
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
                point--;
                BlockMgr.Instance.SetTotalScore(BlockMgr.Instance.GetTotalScore() + 1);

                if (point <= 0)
                {
                    DestroyObj();
                }
                else
                {
                    PlayEffect();
                }
            }
        }

        void PlayEffect()
        {
            uiEffect.colorFactor = 0.2f;
            Invoke("OverEffect",0.05f);
        }

        void OverEffect()
        {
            uiEffect.colorFactor = 0.0f;
        }


        public override void SetPoint(int value)
        {
            point = value;
        }
    }
}