using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BlockBreaker
{
    /// <summary>
    /// 功能：块基类
    /// 创建人：长生
    /// 时间：2022年1月10日17:04:22
    /// </summary>
    public abstract class BlockBase : MonoBehaviour
    {
        public Image icon;
        public BlockType blockType;
        public BlockAttributes blockAttributes;
        public bool isDestroy;
        public int point;
        public int row;
        public Text pointText;

        public abstract void Init();
        public abstract void SetPoint(int value);
        public abstract void SetRow();
        public abstract int GetRow();
        public abstract void DestroyObj();
    }
}