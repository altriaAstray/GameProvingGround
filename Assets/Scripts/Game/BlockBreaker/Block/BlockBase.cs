using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：块基类
    /// 创建人：长生
    /// 时间：2022年1月10日17:04:22
    /// </summary>
    public abstract class BlockBase : MonoBehaviour
    {
        public BlockType blockType;

        [SerializeField] public int point;

        public abstract void SetPoint(int value);
        public abstract void DestroyObj();
    }
}