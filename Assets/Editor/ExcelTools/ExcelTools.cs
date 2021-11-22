using Excel;
using SqlCipher4Unity3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using GameLogic.Json;
using GameLogic.SQL;
using GameLogic.Core.Tools;

namespace GameLogic.Editor
{
    /// <summary>
    /// 功能：Excel工具
    /// 创建者：长生
    /// 日期：2021年11月21日11:19:10
    /// </summary>
    static public class ExcelTools
    {
        static readonly string toDir = ".\\Config\\ExcelTools";                 // 文件夹名称获取
        static readonly string scriptOutPutPath = "Assets/Scripts/Data/DB/";    // 脚本输出路径

        static int tableRows_Max = 3;                                           // 最大行数
        static int tableRows_1 = 0;                                             // 第一行中文名称
        static int tableRows_2 = 1;                                             // 第二行数据类型
        static int tableRows_3 = 2;                                             // 第三行英文名称

        //------------------------------------------------------------
        /// <summary>
        /// 游戏逻辑的Assembly
        /// </summary>
        static public Type[] Types { get; set; } = new Type[] { };

        //------------------------------------------------------------
        [MenuItem("工具箱/表格->脚本", false, 1)]
        public static void ExcelToScripts()
        {
            List<string> xlsxFiles = GetAllConfigFiles();

            foreach (var path in xlsxFiles)
            {
                ExcelToScripts(path);
            }
        }
        //------------------------------------------------------------
        [MenuItem("工具箱/表格->生成SQLite", false, 2)]
        public static void ExcelToSQLite()
        {
            //BD生命周期启动
            BDApplication.Init();

            //加载主工程的DLL Type
            var assemblyPath = BDApplication.Library + "/ScriptAssemblies/Assembly-CSharp.dll";
            var editorAssemlyPath = BDApplication.Library + "/ScriptAssemblies/Assembly-CSharp-Editor.dll";

            if (File.Exists(assemblyPath) && File.Exists(editorAssemlyPath))
            {
                var gAssembly = Assembly.LoadFile(assemblyPath);
                var eAssemlby = Assembly.LoadFile(editorAssemlyPath);

                //编辑器所有类
                List<Type> typeList = new List<Type>();
                typeList.AddRange(gAssembly.GetTypes());
                typeList.AddRange(eAssemlby.GetTypes());

                Types = typeList.ToArray();
            }

            List<string> xlsxFiles = GetAllConfigFiles();

            foreach (var path in xlsxFiles)
            {
                ExcelToSQLite(path);
            }
        }
        //------------------------------------------------------------
        /// <summary>
        /// Excel到数据库
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool ExcelToSQLite(string path)
        {
            var excel = new ExcelUtility(path);
            var json = excel.GetJson();

            if(json == "")
            {
                return false;
            }

            try
            {
                Json2Sqlite(path, json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                EditorUtility.ClearProgressBar();
            }

            return true;
        }

        /// <summary>
        /// json转sql
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="jsonContent"></param>
        static public void Json2Sqlite(string filePath, string jsonContent)
        {
            SQLiteLoder sqlService = new SQLiteLoder("DataBase.db");

            //表名
            var table = Path.GetFileName(filePath).Replace(Path.GetExtension(filePath), "");
            //jsonStr to jsonData
            var jsonObj = JsonMapper.ToObject(jsonContent);
            //库名
            var dbname = Path.GetFileNameWithoutExtension(sqlService.DBPath);
            //命名空间
            var @namespace = "GameLogic.";

            var type = Types.FirstOrDefault((t) => t.FullName.StartsWith(@namespace) && t.Name.ToLower() == table.ToLower());

            if (type == null)
            {
                Debug.LogError(table + "类不存在，请检查!");
                return;
            }

            sqlService.CreateTable(type);

            for (int i = 0; i < jsonObj.Count; i++)
            {
                try
                {
                    var json = jsonObj[i].ToJson();
                    var jobj = JsonMapper.ToObject(type, json);
                    sqlService.Insert(jobj);
                }
                catch (Exception e)
                {
                    Debug.LogError("导出数据有错,跳过! 错误位置:" + type.Name + ":" + i + "/" + jsonObj.Count + "\n" + e);
                }
            }

            AssetDatabase.Refresh();
        }
        //------------------------------------------------------------


        /// <summary>
        /// Excel到表格
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool ExcelToScripts(string path)
        {
            //通过文件获取类名
            string className = Path.GetFileNameWithoutExtension(path);

            //检查类名是否有问题
            if (!Tools.CheckClassName(className))
            {
                string msg = string.Format("Excel文件“{0}”无效，因为xlsx文件的名称应为类名！", path);
                EditorUtility.DisplayDialog("ExcelToScriptableObject", msg, "OK");
                return false;
            }

            //拷贝一份文件
            int indexOfDot = path.LastIndexOf('.');
            string tempExcel = string.Concat(path.Substring(0, indexOfDot), "_temp_", path.Substring(indexOfDot, path.Length - indexOfDot));
            File.Copy(path, tempExcel);

            //读取拷贝的文件
            Stream stream = null;
            try
            {
                stream = File.OpenRead(tempExcel);
            }
            catch
            {
                File.Delete(tempExcel);
                string msg = string.Format("由于共享冲突，无法打开“{0}”。也许您应该先关闭Excel应用程序！", path);
                EditorUtility.DisplayDialog("ExcelToScriptableObject", msg, "OK");
                return false;
            }
            IExcelDataReader reader = path.ToLower().EndsWith(".xls") ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet data = reader.AsDataSet();
            reader.Dispose();
            stream.Close();
            File.Delete(tempExcel);
            if (data == null)
            {
                string msg = string.Format("无法读取“{0}”。似乎这不是一个xlsx文件!", path);
                EditorUtility.DisplayDialog("ExcelToScriptableObject", msg, "OK");
                return false;
            }

            List<SheetData> sheets = new List<SheetData>();
            //处理表数据
            foreach (DataTable table in data.Tables)
            {
                string tableName = table.TableName.Trim();
                //判断表名称前面是否有#  有则忽略
                if (tableName.StartsWith("#"))
                {
                    continue;
                }

                SheetData sheet = new SheetData();
                sheet.table = table;

                if (table.Rows.Count < tableRows_Max)
                {
                    EditorUtility.ClearProgressBar();
                    string msg = string.Format("无法分析“{0}”。Excel文件应至少包含三行（第一行：英文名称，第二行：中文名称，第三行：数据类型）!", path);
                    EditorUtility.DisplayDialog("ExcelToScriptableObject", msg, "OK");
                    return false;
                }
                //设置类名
                sheet.itemClassName = tableName;

                if (!Tools.CheckClassName(sheet.itemClassName))
                {
                    EditorUtility.ClearProgressBar();
                    string msg = string.Format("工作表名称“{0}”无效，因为该工作表的名称应为类名!", sheet.itemClassName);
                    EditorUtility.DisplayDialog("ExcelToScriptableObject", msg, "OK");
                    return false;
                }
                //字段名称
                object[] fieldNames;
                fieldNames = table.Rows[tableRows_3].ItemArray;
                //字段注释
                object[] fieldNotes;
                fieldNotes = table.Rows[tableRows_1].ItemArray;
                //字段类型
                object[] fieldTypes;
                fieldTypes = table.Rows[tableRows_2].ItemArray;

                for (int i = 0, imax = fieldNames.Length; i < imax; i++)
                {
                    string fieldNameStr = fieldNames[i].ToString().Trim();
                    string fieldNoteStr = fieldNotes[i].ToString().Trim();
                    string fieldTypeStr = fieldTypes[i].ToString().Trim();
                    //检查字段名
                    if (string.IsNullOrEmpty(fieldNameStr))
                    {
                        break;
                    }
                    if (!Tools.CheckFieldName(fieldNameStr))
                    {
                        EditorUtility.ClearProgressBar();
                        string msg = string.Format("无法分析“{0}”，因为字段名“{1}”无效!", path, fieldNameStr);
                        EditorUtility.DisplayDialog("ExcelToScriptableObject", msg, "OK");
                        return false;
                    }

                    //解析类型
                    FieldTypes fieldType = GetFieldType(fieldTypeStr);

                    FieldData field = new FieldData();
                    field.fieldName = fieldNameStr;
                    field.fieldNotes = fieldNoteStr;
                    field.fieldIndex = i;
                    field.fieldType = fieldType;
                    field.fieldTypeName = fieldTypeStr;

                    if (fieldType == FieldTypes.Unknown)
                    {
                        fieldType = FieldTypes.UnknownList;
                        if (fieldTypeStr.StartsWith("[") && fieldTypeStr.EndsWith("]"))
                        {
                            fieldTypeStr = fieldTypeStr.Substring(1, fieldTypeStr.Length - 2).Trim();
                        }
                        else if (fieldTypeStr.EndsWith("[]"))
                        {
                            fieldTypeStr = fieldTypeStr.Substring(0, fieldTypeStr.Length - 2).Trim();
                        }
                        else
                        {
                            fieldType = FieldTypes.Unknown;
                        }

                        field.fieldType = field.fieldType == FieldTypes.UnknownList ? FieldTypes.CustomTypeList : FieldTypes.CustomType;
                    }

                    sheet.fields.Add(field);
                }

                sheets.Add(sheet);
            }

            for (int i = 0; i < sheets.Count; i++)
            {
                GenerateScript(sheets[i]);
            }

            return true;
        }
        //------------------------------------------------------------
        /// <summary>
        /// 获取所有的xlsx文件路径
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllConfigFiles(string filetype = "*.xlsx")
        {
            List<string> tableList = new List<string>();
            //等待编译结束
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "等待编译结束。", "OK");
                return null;
            }
            //查看路径是否存在
            if (Directory.Exists(toDir) == false)
            {
                Debug.LogWarning(string.Format("没有找到该路径: {0}", toDir));
                return null;
            }
            //查找文件目录
            foreach (var path in Directory.GetFiles(toDir, "*", SearchOption.AllDirectories))
            {
                var suffix = Path.GetExtension(path);
                if (suffix != ".xlsx" && suffix != ".xls")
                {
                    string msg = string.Format("文件“{0}”不是表格！", path);
                    EditorUtility.DisplayDialog("ExcelToScriptableObject", msg, "OK");
                    continue;
                }
                tableList.Add(path);
            }
            return tableList;
        }
        //------------------------------------------------------------
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        static async Task SaveFile(string fullpath, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            await SaveFileAsync(fullpath, buffer);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        static async Task<int> SaveFileAsync(string fullpath, byte[] content)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (content == null)
                    {
                        content = new byte[0];
                    }

                    string dir = PathUtils.GetParentDir(fullpath);

                    if (!Directory.Exists(dir))
                    {
                        try
                        {
                            Directory.CreateDirectory(dir);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(string.Format("SaveFile() CreateDirectory Error! Dir:{0}, Error:{1}", dir, e.Message));
                            return -1;
                        }
                    }

                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(fullpath, FileMode.Create, FileAccess.Write);
                        fs.Write(content, 0, content.Length);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(string.Format("SaveFile() Path:{0}, Error:{1}", fullpath, e.Message));
                        fs.Close();
                        return -1;
                    }

                    fs.Close();
                    return content.Length;
                });
            }
            catch (Exception ex)
            {
                Debug.LogError(ex + " SaveFile");
                throw;
            }
        }
        //---------------------------------------------------

        /// <summary>
        /// 生成脚本
        /// </summary>
        /// <param name="sheet"></param>
        static async void GenerateScript(SheetData sheet)
        {
            string ScriptTemplate = @"//此脚本为自动生成 <ExcelToScript>

using SQLite.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameLogic
{
    [UnityEngine.Scripting.Preserve]
    public class {_0_}
    {
        [PrimaryKey]
        [AutoIncrement]
        {_1_}

        public override string ToString()
        {
            return string.Format(
                {_2_},
                {_3_}
            );
        }
    }
}
";
            var dataName = sheet.itemClassName;
            var str = GenerateDataScript(ScriptTemplate, dataName, sheet.fields);
            await SaveFile(scriptOutPutPath + dataName + ".cs", str);

            AssetDatabase.Refresh();
        }

        //---------------------------------------------------

        /// <summary>
        /// 创建数据结构脚本
        /// </summary>
        /// <param name="template"></param>
        /// <param name="scriptName"></param>
        /// <param name="fieldDatas"></param>
        /// <returns></returns>
        static string GenerateDataScript(string template, string scriptName, List<FieldData> fieldDatas)
        {
            StringBuilder privateType = new StringBuilder();
            privateType.AppendLine();

            string toString_1 = "";
            string toString_2 = "";

            string additional = "{{ get; set; }}";

            for (int i = 0; i < fieldDatas.Count; i++)
            {
                var typeName = GetFieldTypeString(fieldDatas[i].fieldType, fieldDatas[i].fieldTypeName);

                string attribute = string.Format("        public {0} {1} {2}    //{3}", typeName, fieldDatas[i].fieldName, additional, fieldDatas[i].fieldNotes);
                privateType.AppendFormat(attribute);
                privateType.AppendLine();

                int value = i + 1;
                toString_1 += fieldDatas[i].fieldName + "={" + value + "}";
                if (i < fieldDatas.Count - 1)
                    toString_1 += ",";

                toString_2 += "this." + fieldDatas[i].fieldName;
                if (i < fieldDatas.Count - 1)
                    toString_2 += ",\r\n                ";

            }

            string str = template;
            str = str.Replace("{_0_}", scriptName);
            str = str.Replace("{_1_}", privateType.ToString());
            str = str.Replace("{_2_}", "\"[" + toString_1 + "]\"");
            str = str.Replace("{_3_}", toString_2);
            return str;
        }

        /// <summary>
        /// 第一个字符大写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string CapitalFirstChar(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        static FieldTypes GetFieldType(string typeName)
        {
            FieldTypes type = FieldTypes.Unknown;
            if (!string.IsNullOrEmpty(typeName))
            {
                switch (typeName.Trim().ToLower())
                {
                    case "bool":
                        type = FieldTypes.Bool;
                        break;
                    case "int":
                    case "int32":
                        type = FieldTypes.Int;
                        break;
                    case "ints":
                    case "int[]":
                    case "[int]":
                    case "int32s":
                    case "int32[]":
                    case "[int32]":
                        type = FieldTypes.Ints;
                        break;
                    case "float":
                        type = FieldTypes.Float;
                        break;
                    case "floats":
                    case "float[]":
                    case "[float]":
                        type = FieldTypes.Floats;
                        break;
                    case "long":
                    case "int64":
                        type = FieldTypes.Long;
                        break;
                    case "longs":
                    case "long[]":
                    case "[long]":
                    case "int64s":
                    case "int64[]":
                    case "[int64]":
                        type = FieldTypes.Longs;
                        break;
                    case "vector2":
                        type = FieldTypes.Vector2;
                        break;
                    case "vector3":
                        type = FieldTypes.Vector3;
                        break;
                    case "vector4":
                        type = FieldTypes.Vector4;
                        break;
                    case "rect":
                    case "rectangle":
                        type = FieldTypes.Rect;
                        break;
                    case "color":
                    case "colour":
                        type = FieldTypes.Color;
                        break;
                    case "string":
                        type = FieldTypes.String;
                        break;
                    case "strings":
                    case "string[]":
                    case "[string]":
                        type = FieldTypes.Strings;
                        break;
                }
            }
            return type;
        }

        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="fieldTypes"></param>
        /// <returns></returns>
        static string GetFieldTypeString(FieldTypes fieldTypes, string fieldTypeName)
        {
            string result = string.Empty;
            switch (fieldTypes)
            {
                case FieldTypes.Bool:
                    result = "bool";
                    break;
                case FieldTypes.Int:
                    result = "int";
                    break;
                case FieldTypes.Ints:
                    result = "List<int>";
                    break;
                case FieldTypes.Float:
                    result = "float";
                    break;
                case FieldTypes.Floats:
                    result = "List<float>";
                    break;
                case FieldTypes.Long:
                    result = "long";
                    break;
                case FieldTypes.Longs:
                    result = "List<long>";
                    break;
                case FieldTypes.Vector2:
                    result = "Vector2";
                    break;
                case FieldTypes.Vector3:
                    result = "Vector3";
                    break;
                case FieldTypes.Vector4:
                    result = "Vector4";
                    break;
                case FieldTypes.Rect:
                    result = "Rect";
                    break;
                case FieldTypes.Color:
                    result = "Color";
                    break;
                case FieldTypes.String:
                    result = "string";
                    break;
                case FieldTypes.Strings:
                    result = "List<string>";
                    break;
                case FieldTypes.CustomType:
                    result = "fieldTypeName";
                    break;
                case FieldTypes.CustomTypeList:
                    result = "List<fieldTypeName>";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string ToFirstLetterUpper(string str)
        {
            return str.First().ToString().ToUpper() + str.Substring(1);
        }
    }
    
    /// <summary>
    /// 单张表数据
    /// </summary>
    public class SheetData
    {
        public DataTable table;
        public string itemClassName;
        public bool keyToMultiValues;
        public bool internalData;
        public List<FieldData> fields = new List<FieldData>();
    }

    /// <summary>
    /// 字段数据
    /// </summary>
    public class FieldData
    {
        public string fieldName;        //字段名称
        public string fieldNotes;       //字段注释
        public int fieldIndex;          //字段索引
        public FieldTypes fieldType;    //字段类型
        public string fieldTypeName;    //字段类型名称
    }
}