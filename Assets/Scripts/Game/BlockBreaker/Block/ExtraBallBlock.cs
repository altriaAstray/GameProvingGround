using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：球变为多个的块
    /// 创建者：长生
    /// 时间：2021年11月20日10:32:26
    /// </summary>
    public class ExtraBallBlock : BlockBase
    {
        public GameObject objectToSpawn;
        private float[] direction = { -1f, 1f };

        private void Start()
        {
            isDestroy = false;
            blockType = BlockType.ExtraBall;
            point = 1;
        }

        public override void DestroyObj()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                isDestroy = true;
                Ball ball = collision.gameObject.GetComponent<Ball>();
                if (!ball.Duplicate)
                {
                    float angle = direction[UnityEngine.Random.Range(0, 2)] * 45;
                    Vector2 newVelocity = ball.AngleVelocityByDegree(angle);
                    BallDuplicate(ball.transform.position, newVelocity, angle);
                }
            }
        }

        /// <summary>
        /// 复制球
        /// </summary>
        /// <param name="spawnPosition"></param>
        /// <param name="newVelocity"></param>
        /// <param name="angle"></param>
        private void BallDuplicate(Vector2 spawnPosition, Vector2 newVelocity, float angle)
        {
            if (objectToSpawn != null)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                Ball duplicate = spawnedObject.GetComponent<Ball>();
                spawnedObject.transform.SetParent(Launcher.Instance.GetBallRoot());
                spawnedObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.1f);
                duplicate.Duplicate = true;
                spawnedObject.transform.Find("Icon").GetComponent<SpriteRenderer>().color = Tools.HexToColor("3EFF2FFF");
                duplicate.InitialVelocity = duplicate.AngleVelocityByDegree(-angle * 2, newVelocity);
            }
        }

        public override void SetPoint(int value)
        {
            point = value;
        }
    }
}