//此脚本为自动生成 <ExcelToScript>

using SQLite.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameLogic
{
    [UnityEngine.Scripting.Preserve]
    public class Save
    {
        [PrimaryKey]
        [AutoIncrement]
        
        public int Id { get; set; }    //id
        public int Index { get; set; }    //索引
        public string Name { get; set; }    //名称
        public int Value { get; set; }    //值(int)


        public override string ToString()
        {
            return string.Format(
                "[Id={1},Index={2},Name={3},Value={4}]",
                this.Id,
                this.Index,
                this.Name,
                this.Value
            );
        }
    }
}
