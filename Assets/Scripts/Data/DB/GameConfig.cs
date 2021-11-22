//此脚本为自动生成 <ExcelToScript>

using SQLite.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameLogic
{
    [UnityEngine.Scripting.Preserve]
    public class GameConfig
    {
        [PrimaryKey]
        [AutoIncrement]
        
        public int Id { get; set; }    //id
        public int Index { get; set; }    //索引
        public string Name { get; set; }    //名称
        public int ValueType { get; set; }    //值类型
        public int Value_1 { get; set; }    //值(int)
        public float Value_2 { get; set; }    //值(float)
        public bool Value_3 { get; set; }    //值(bool)
        public string Value_4 { get; set; }    //值(string)


        public override string ToString()
        {
            return string.Format(
                "[Id={1},Index={2},Name={3},ValueType={4},Value_1={5},Value_2={6},Value_3={7},Value_4={8}]",
                this.Id,
                this.Index,
                this.Name,
                this.ValueType,
                this.Value_1,
                this.Value_2,
                this.Value_3,
                this.Value_4
            );
        }
    }
}
