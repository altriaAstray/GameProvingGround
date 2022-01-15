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
        [SerializeField] UIEffect uiEffect;
        [SerializeField] OrdinaryBlockAttributes ordinaryBlockAttributes;

        private void Start()
        {
            isDestroy = false;
            blockType = BlockType.OrdinaryBlock;

            Init();
        }

        public void Init()
        {
            int value = Random.Range(0, 1000);

            if(value < 400)
            {
                ordinaryBlockAttributes = OrdinaryBlockAttributes.SoilBlock;
                if(ResourcesMgr.Instance != null)
                {
                    icon.sprite = ResourcesMgr.Instance.LoadAsset<Sprite>("Images/Block/SoilBlock");
                }
            }
            else if(value < 800)
            {
                ordinaryBlockAttributes = OrdinaryBlockAttributes.WoodBlock;
                if (ResourcesMgr.Instance != null)
                {
                    icon.sprite = ResourcesMgr.Instance.LoadAsset<Sprite>("Images/Block/WoodBlock");
                }
            }
            else if (value < 900)
            {
                ordinaryBlockAttributes = OrdinaryBlockAttributes.BrickBlock;
                if (ResourcesMgr.Instance != null)
                {
                    icon.sprite = ResourcesMgr.Instance.LoadAsset<Sprite>("Images/Block/BrickBlock");
                }
            }
            else if (value <= 1000)
            {
                ordinaryBlockAttributes = OrdinaryBlockAttributes.IronBlock;
                if (ResourcesMgr.Instance != null)
                {
                    icon.sprite = ResourcesMgr.Instance.LoadAsset<Sprite>("Images/Block/IronBlock");
                }
            }
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