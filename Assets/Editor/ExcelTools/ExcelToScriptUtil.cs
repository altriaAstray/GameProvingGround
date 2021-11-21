//using GameLogic;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Reflection;
//using System.Text.RegularExpressions;
//using UnityEngine;

//namespace GameLogic
//{
//    public class ExcelToScriptUtil : MonoBehaviour
//    {
//        //static Regex reg_color32 = new Regex(@"^[A-Fa-f0-9]{8}$");
//        //static Regex reg_color24 = new Regex(@"^[A-Fa-f0-9]{6}$");

//        //public static bool SetFieldValue(FieldInfo fieldInfo, System.Object target, string value)
//        //{
//        //    try
//        //    {
//        //        if (fieldInfo.FieldType == typeof(bool))
//        //        {
//        //            fieldInfo.SetValue(target, (value == "true" || value == "1") ? true : false);
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(int))
//        //        {
//        //            if (string.IsNullOrEmpty(value))
//        //            {
//        //                fieldInfo.SetValue(target, 0);
//        //            }
//        //            else
//        //            {
//        //                try
//        //                {
//        //                    fieldInfo.SetValue(target, int.Parse(value));
//        //                }
//        //                catch (Exception)
//        //                {
//        //                    float tValue = float.Parse(value);
//        //                    fieldInfo.SetValue(target, Mathf.RoundToInt(tValue));
//        //                }
//        //            }
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(List<int>))
//        //        {
//        //            fieldInfo.SetValue(target, GetIntsFromString(value));
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(float))
//        //        {
//        //            fieldInfo.SetValue(target, float.Parse(value));
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(List<float>))
//        //        {
//        //            fieldInfo.SetValue(target, GetFloatsFromString(value));
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(long))
//        //        {
//        //            fieldInfo.SetValue(target, long.Parse(value));
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(List<long>))
//        //        {
//        //            fieldInfo.SetValue(target, GetLongsFromString(value));
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(string))
//        //        {
//        //            fieldInfo.SetValue(target, value);
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(List<string>))
//        //        {
//        //            fieldInfo.SetValue(target, GetStringsFromString(value));
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(Vector2))
//        //        {
//        //            List<float> floatsRect = GetFloatsFromString(value);
//        //            if (floatsRect.Count == 2)
//        //            {
//        //                fieldInfo.SetValue(target, new Vector2(floatsRect[0], floatsRect[1]));
//        //            }
//        //            else
//        //            {
//        //                fieldInfo.SetValue(target, new Vector2());
//        //            }
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(Vector3))
//        //        {
//        //            List<float> floatsRect = GetFloatsFromString(value);
//        //            if (floatsRect.Count == 3)
//        //            {
//        //                fieldInfo.SetValue(target, new Vector3(floatsRect[0], floatsRect[1], floatsRect[2]));
//        //            }
//        //            else
//        //            {
//        //                fieldInfo.SetValue(target, new Vector3());
//        //            }
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(Vector4))
//        //        {
//        //            List<float> floatsRect = GetFloatsFromString(value);
//        //            if (floatsRect.Count == 4)
//        //            {
//        //                fieldInfo.SetValue(target, new Vector4(floatsRect[0], floatsRect[1], floatsRect[2], floatsRect[3]));
//        //            }
//        //            else
//        //            {
//        //                fieldInfo.SetValue(target, new Vector4());
//        //            }
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(Rect))
//        //        {
//        //            List<float> floatsRect = GetFloatsFromString(value);
//        //            if (floatsRect.Count == 4)
//        //            {
//        //                fieldInfo.SetValue(target, new Rect(floatsRect[0], floatsRect[1], floatsRect[2], floatsRect[3]));
//        //            }
//        //            else
//        //            {
//        //                fieldInfo.SetValue(target, new Rect());
//        //            }
//        //        }
//        //        else if (fieldInfo.FieldType == typeof(Color))
//        //        {
//        //            Color c = GetColorFromString(value);
//        //            fieldInfo.SetValue(target, c);
//        //        }
//        //        //else if (fieldInfo.FieldType == typeof(KVIntMap))
//        //        //{
//        //        //    fieldInfo.SetValue(target, ConvertToMap(value));
//        //        //}
//        //        //else if (fieldInfo.FieldType == typeof(List<KVIntMap>))
//        //        //{
//        //        //    fieldInfo.SetValue(target, ConvertToMapList(value));
//        //        //}
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        Debug.LogError(string.Format("field setValue failed:{0},{1},{2},{3},", target.GetType(), fieldInfo.FieldType, fieldInfo.Name, value));
//        //        Debug.LogError(e.Message);
//        //        return false;
//        //    }

//        //    return true;
//        //}

//        //static List<int> GetIntsFromString(string str)
//        //{
//        //    str = TrimBracket(str);
//        //    if (string.IsNullOrEmpty(str))
//        //    {
//        //        return new List<int>();
//        //    }
//        //    string[] splits = str.Split(',');
//        //    List<int> ints = new List<int>();
//        //    for (int i = 0, imax = splits.Length; i < imax; i++)
//        //    {
//        //        int intValue;
//        //        if (int.TryParse(splits[i].Trim(), out intValue))
//        //        {
//        //            ints.Add(intValue);
//        //        }
//        //        else
//        //        {
//        //            ints.Add(0);
//        //        }
//        //    }
//        //    return ints;
//        //}

//        //static List<float> GetFloatsFromString(string str)
//        //{
//        //    str = TrimBracket(str);
//        //    if (string.IsNullOrEmpty(str))
//        //    {
//        //        return new List<float>();
//        //    }
//        //    string[] splits = str.Split(',');
//        //    List<float> floats = new List<float>();
//        //    for (int i = 0, imax = splits.Length; i < imax; i++)
//        //    {
//        //        float floatValue;
//        //        if (float.TryParse(splits[i].Trim(), out floatValue))
//        //        {
//        //            floats.Add(floatValue);
//        //        }
//        //        else
//        //        {
//        //            floats.Add(0);
//        //        }
//        //    }
//        //    return floats;
//        //}

//        //private static Color GetColorFromString(string str)
//        //{
//        //    if (string.IsNullOrEmpty(str)) { return Color.clear; }
//        //    uint colorUInt;
//        //    if (GetColorUIntFromString(str, out colorUInt))
//        //    {
//        //        uint r = (colorUInt >> 24) & 0xffu;
//        //        uint g = (colorUInt >> 16) & 0xffu;
//        //        uint b = (colorUInt >> 8) & 0xffu;
//        //        uint a = colorUInt & 0xffu;
//        //        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
//        //    }
//        //    str = TrimBracket(str);
//        //    string[] splits = str.Split(',');
//        //    if (splits.Length == 4)
//        //    {
//        //        int r, g, b, a;
//        //        if (int.TryParse(splits[0].Trim(), out r) && int.TryParse(splits[1].Trim(), out g) &&
//        //            int.TryParse(splits[2].Trim(), out b) && int.TryParse(splits[3].Trim(), out a))
//        //        {
//        //            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
//        //        }
//        //    }
//        //    else if (splits.Length == 3)
//        //    {
//        //        int r, g, b;
//        //        if (int.TryParse(splits[0].Trim(), out r) && int.TryParse(splits[1].Trim(), out g) &&
//        //            int.TryParse(splits[2].Trim(), out b))
//        //        {
//        //            return new Color(r / 255f, g / 255f, b / 255f);
//        //        }
//        //    }
//        //    return Color.clear;
//        //}

//        //private static bool GetColorUIntFromString(string str, out uint color)
//        //{
//        //    if (reg_color32.IsMatch(str))
//        //    {
//        //        color = System.Convert.ToUInt32(str, 16);
//        //    }
//        //    else if (reg_color24.IsMatch(str))
//        //    {
//        //        color = (System.Convert.ToUInt32(str, 16) << 8) | 0xffu;
//        //    }
//        //    else
//        //    {
//        //        color = 0u;
//        //        return false;
//        //    }
//        //    return true;
//        //}

//        //private static List<long> GetLongsFromString(string str)
//        //{
//        //    str = TrimBracket(str);
//        //    if (string.IsNullOrEmpty(str))
//        //    {
//        //        return new List<long>();
//        //    }
//        //    string[] splits = str.Split(',');
//        //    List<long> longs = new List<long>();
//        //    for (int i = 0, imax = splits.Length; i < imax; i++)
//        //    {
//        //        long longValue;
//        //        if (long.TryParse(splits[i].Trim(), out longValue))
//        //        {
//        //            longs.Add(longValue);
//        //        }
//        //        else
//        //        {
//        //            longs.Add(0L);
//        //        }
//        //    }
//        //    return longs;
//        //}

//        //private static List<string> GetStringsFromString(string str)
//        //{
//        //    str = TrimBracket(str);
//        //    if (string.IsNullOrEmpty(str))
//        //    {
//        //        return new List<string>();
//        //    }

//        //    List<string> strings = new List<string>();
//        //    strings.AddRange(str.Split(','));
//        //    return strings;
//        //}

//        //static string TrimBracket(string str)
//        //{
//        //    if (str.StartsWith("[") && str.EndsWith("]"))
//        //    {
//        //        return str.Substring(1, str.Length - 2);
//        //    }
//        //    return str;
//        //}
//    }


//}