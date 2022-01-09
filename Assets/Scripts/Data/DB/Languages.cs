//此脚本为自动生成 <ExcelToScript>

using SQLite.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameLogic
{
    [UnityEngine.Scripting.Preserve]
    public class Languages
    {
        [PrimaryKey]
        [AutoIncrement]
        
        public int Id { get; set; }    //id
        public int Index { get; set; }    //索引
        public string ZH { get; set; }    //中文
        public string EN { get; set; }    //英文


        public override string ToString()
        {
            return string.Format(
                "[Id={1},Index={2},ZH={3},EN={4}]",
                this.Id,
                this.Index,
                this.ZH,
                this.EN
            );
        }
    }
}
