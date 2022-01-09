using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：发射器
    /// 创建者：长生
    /// 日期：2022年1月9日11:06:18
    /// </summary>
    public class Launcher : SingleToneManager<Launcher>
    {
        [SerializeField] GameObject ballObj;
        [SerializeField] Transform ballRoot;
        [SerializeField] AimLine aimLine;

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                CreateSpawner();
            }
        }

        private void CreateSpawner()
        {
            GameObject go = Instantiate(ballObj, transform.position, Quaternion.identity);
            go.SetActive(true);
            go.transform.SetParent(ballRoot);
            go.transform.localScale = new Vector3(0.5f, 0.5f, 0.2f);

            Vector2 spawnVelocity = aimLine.GetLineRenderer().GetPosition(1) - aimLine.GetLineRenderer().GetPosition(0);
            Ball ball = go.GetComponent<Ball>();
            ball.InitialVelocity = spawnVelocity;
        }
    }
}