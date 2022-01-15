using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：球
    /// 创建者：长生
    /// 日期：2022年1月9日11:06:18
    /// </summary>

    public class Ball : MonoBehaviour
    {
        private Rigidbody2D rb;                     //刚体
        private BallStuckWatchdog watchdog;         //监察人


        private Vector2 initialVelocity;            //初始速度
        public Vector2 InitialVelocity
        {
            set
            {
                initialVelocity = value;
            }
        }

        private bool alreadyEnteredBouncePowerUp;       //已进入开机状态

        private bool bounce;                            //反弹
        public bool Bounce
        {
            set
            {
                if (value && !alreadyEnteredBouncePowerUp)
                {
                    alreadyEnteredBouncePowerUp = true;
                    bounce = true;
                }
                else if (!value)
                {
                    bounce = false;
                }
            }
            get
            {
                return bounce;
            }
        }

        private bool duplicate;                         //复制
        public bool Duplicate
        {
            set
            {
                duplicate = value;
            }
            get
            {
                return duplicate;
            }
        }

        private static float SPEED = 20f;                                    //速度
        private static float MIN_SPEED = 0.7f;                               //最小速度
        private static float MIN_SPEED_BEFORE_CONSIDERED_STUCK = 0.35f;      //卡滞前的最小速度
        private static float UNSTUCK_BOOST = 20f;                            //防卡助推
        private static float[] DIRECTION = { 1, -1 };                        //方向

        [SerializeField] BallStatic ballStatic;                              //球的状态
        [SerializeField] private float deleteTime = 0f;                      //防止删除时间
        [SerializeField] private float deleteMaxTime = 0.02f;                //防止删除最大时间

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = initialVelocity;
            }

            bounce = false;
            alreadyEnteredBouncePowerUp = false;

            ballStatic = BallStatic.Launch;

            watchdog = Launcher.Instance.gameObject.GetComponent<BallStuckWatchdog>();
        }

        void FixedUpdate()
        {
            deleteTime += Time.deltaTime;
            switch (ballStatic)
            {
                case BallStatic.Launch:
                    if (deleteTime > deleteMaxTime)
                    {
                        ballStatic = BallStatic.InFlight;
                    }
                    Move();
                    break;
                case BallStatic.InFlight:
                    Move();
                    break;
                case BallStatic.EndOfFlight:
                    rb.velocity = rb.velocity.normalized * SPEED;
                    break;
            }

            if (rb.velocity.x < MIN_SPEED_BEFORE_CONSIDERED_STUCK)
            {
                watchdog.AddPossibleStuckBall(this);
            }
            else
            {
                watchdog.RemovePossibleStuckBall(this);
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        void Move()
        {
            rb.velocity = rb.velocity.normalized * SPEED;

            if (rb.velocity.y < MIN_SPEED && rb.velocity.y > -MIN_SPEED)
            {
                float signXVel = Mathf.Sign(rb.velocity.x);
                float signYVel = Mathf.Sign(rb.velocity.y);
                rb.velocity = new Vector2(signXVel * SPEED - MIN_SPEED, signYVel * MIN_SPEED);
            }

            //if (rb.velocity.x < MIN_SPEED && rb.velocity.x > -MIN_SPEED)
            //{
            //    float signXVel = Mathf.Sign(rb.velocity.x);
            //    float signYVel = Mathf.Sign(rb.velocity.y);
            //    rb.velocity = new Vector2(signXVel * MIN_SPEED, signYVel * SPEED - MIN_SPEED);
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlaySound(collision.gameObject);

            //碰撞Player后销毁自己
            if (collision.gameObject.CompareTag("Player") && deleteTime > deleteMaxTime)
            {
                Destroy(this.gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            PlaySound(collision.gameObject);

            //碰撞Wall2后调整运动方向为Player
            if (collision.gameObject.CompareTag("Wall2"))
            {
                ballStatic = BallStatic.EndOfFlight;
                rb.velocity = Vector2.zero;
                rb.velocity = Launcher.Instance.transform.position - this.transform.position;
            }

            if(collision.gameObject.CompareTag("Block"))
            {
                watchdog.RemovePossibleStuckBall(this);
            }
        }

        void PlaySound(GameObject go)
        {
            if(go != null)
            {
                BlockBase blockBase = go.GetComponent<BlockBase>();
                if(blockBase != null)
                {
                    switch(blockBase.blockType)
                    {
                        case BlockType.AddBall:
                            if (AudioMgr.Instance != null)
                                AudioMgr.Instance.PlaySound(100028);
                            break;
                        case BlockType.OrdinaryBlock:
                            if (AudioMgr.Instance != null)
                                AudioMgr.Instance.PlaySound(100022);
                            break;
                        case BlockType.SpecialShapedBlock:
                            if (AudioMgr.Instance != null)
                                AudioMgr.Instance.PlaySound(100023);
                            break;
                        case BlockType.ElementBlock:
                            break;
                        case BlockType.ExtraBall:
                            break;
                        case BlockType.KillBall:
                            break;
                        case BlockType.AttackBall:
                            if (AudioMgr.Instance != null)
                                AudioMgr.Instance.PlaySound(100030);
                            break;
                    }

                }
                else
                {
                    if (!go.CompareTag("Player") && AudioMgr.Instance != null)
                        AudioMgr.Instance.PlaySound(100022);
                }
            }

            
        }

        /// <summary>
        /// 度角速度
        /// </summary>
        /// <param name="degree">度数</param>
        /// <returns></returns>
        public Vector2 AngleVelocityByDegree(float degree)
        {
            Vector2 currentVelocity = rb.velocity;
            rb.velocity = Quaternion.Euler(0, 0, degree) * currentVelocity;
            return rb.velocity;
        }

        /// <summary>
        /// 度角速度
        /// </summary>
        /// <param name="degree">度数</param>
        /// <param name="velocityToChange">速度变化</param>
        /// <returns></returns>
        public Vector2 AngleVelocityByDegree(float degree, Vector2 velocityToChange)
        {
            return Quaternion.Euler(0, 0, degree) * velocityToChange;
        }

        /// <summary>
        /// 防止卡住
        /// </summary>
        public void UnstuckMe()
        {
            rb.velocity = new Vector2(DIRECTION[UnityEngine.Random.Range(0, 2)] * rb.velocity.y, UNSTUCK_BOOST);
        }
    }
}