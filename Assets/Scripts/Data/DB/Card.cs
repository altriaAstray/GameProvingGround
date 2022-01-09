//此脚本为自动生成 <ExcelToScript>

using SQLite.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameLogic
{
    [UnityEngine.Scripting.Preserve]
    public class Card
    {
        [PrimaryKey]
        [AutoIncrement]
        
        public int Id { get; set; }    //id
        public int Index { get; set; }    //索引
        public string Name { get; set; }    //名称
        public string Detailed { get; set; }    //明细
        public int CardType { get; set; }    //卡片类型
        public string IconPath { get; set; }    //图片路径
        public string IconName { get; set; }    //图片名称
        public int HealthPoints { get; set; }    //生命值
        public int Shield { get; set; }    //护盾
        public int Attack { get; set; }    //攻击
        public int AttackType { get; set; }    //攻击类型
        public int Defense { get; set; }    //防御
        public int EffectType { get; set; }    //效果类型


        public override string ToString()
        {
            return string.Format(
                "[Id={1},Index={2},Name={3},Detailed={4},CardType={5},IconPath={6},IconName={7},HealthPoints={8},Shield={9},Attack={10},AttackType={11},Defense={12},EffectType={13}]",
                this.Id,
                this.Index,
                this.Name,
                this.Detailed,
                this.CardType,
                this.IconPath,
                this.IconName,
                this.HealthPoints,
                this.Shield,
                this.Attack,
                this.AttackType,
                this.Defense,
                this.EffectType
            );
        }
    }
}
