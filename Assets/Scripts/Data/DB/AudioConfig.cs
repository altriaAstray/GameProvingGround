//此脚本为自动生成 <ExcelToScript>

using SQLite.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameLogic
{
    [UnityEngine.Scripting.Preserve]
    public class AudioConfig
    {
        [PrimaryKey]
        [AutoIncrement]
        
        public int Id { get; set; }    //id
        public int Index { get; set; }    //索引
        public string Path { get; set; }    //路径
        public string Name { get; set; }    //名称
        public int Type { get; set; }    //类型


        public override string ToString()
        {
            return string.Format(
                "[Id={1},Index={2},Path={3},Name={4},Type={5}]",
                this.Id,
                this.Index,
                this.Path,
                this.Name,
                this.Type
            );
        }
    }
}
