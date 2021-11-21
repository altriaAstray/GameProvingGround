/// <summary>
/// 创建者：长生
/// 时间：2021年11月20日10:32:26
/// 功能：枚举
/// </summary>
namespace GameLogic
{

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