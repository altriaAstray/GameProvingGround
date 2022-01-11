using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    public class AddBlock : BlockBase
    {
        //[SerializeField] Text pointText;

        private void Start()
        {
            blockType = BlockType.AddBall;
            point = 1;
        }

        public void Update()
        {

        }

        public override void DestroyObj()
        {
            Destroy(this.gameObject);
        }

        public override void EffectTrigger()
        {

        }

        public override BlockType GetBlockType()
        {
            return blockType;
        }

        public override void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public override void EndGame()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                point--;
                BlockMgr.Instance.AddNumberOfAmmunition();
                if (point <= 0)
                {
                    DestroyObj();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                point--;
                BlockMgr.Instance.AddNumberOfAmmunition();
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