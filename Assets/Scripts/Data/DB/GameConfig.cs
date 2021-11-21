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
        public string Name { get; set; }    //名称
        public int ValueType { get; set; }    //值类型
        public int Value_1 { get; set; }    //值(int)
        public float Value_2 { get; set; }    //值(float)
        public bool Value_3 { get; set; }    //值(bool)
        public string Value_4 { get; set; }    //值(string)


        public override string ToString()
        {
            return string.Format(
                "[Id={1},Name={2},ValueType={3},Value_1={4},Value_2={5},Value_3={6},Value_4={7}]",
                this.Id,
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
