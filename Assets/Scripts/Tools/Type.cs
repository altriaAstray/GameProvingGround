/// <summary>
/// 创建者：长生
/// 时间：2021年11月20日10:32:26
/// 功能：枚举
/// </summary>
namespace GameLogic
{
    /// <summary>
    /// 加载资源的返回状态
    /// </summary>
    public enum LoadAssetState
    {
        Success = 0,
        Fail,
        IsLoding,
    }    
    
    /// <summary>
    /// 多语言
    /// </summary>
    public enum Multilingual
    {
        ZH = 0,//中文
        EN,//英文
    }

    /// <summary>
    /// 资源加载路径
    /// </summary>
    public enum AssetLoadPath
    {
        Editor = 0,
        Persistent,
        StreamingAsset,
        EditorLibrary
    }

    /// <summary>
    /// 字段类型
    /// </summary>
    public enum FieldTypes
    {
        Unknown,        //未知类型
        UnknownList,    //未知类型表
        Bool,
        Int,
        Ints,           //Int数组
        Float,
        Floats,         //Float数组
        Long,
        Longs,          //Long数组
        Vector2,
        Vector3,
        Vector4,
        Rect,           //矩阵
        Color,          //颜色
        String,
        Strings,
        CustomType,     //自定义类型
        CustomTypeList  //自定义类型数组
    }
}

namespace GameLogic.Json
{
    public enum JsonType
    {
        None,

        Object,
        Array,
        String,
        Int,
        Long,
        Double,
        Boolean
    }

    public enum JsonToken
    {
        None,

        ObjectStart,
        PropertyName,
        ObjectEnd,

        ArrayStart,
        ArrayEnd,

        Int,
        Long,
        Double,

        String,

        Boolean,
        Null
    }
}