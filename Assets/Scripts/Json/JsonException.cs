using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameLogic.Json
{
    internal enum ParserToken
    {
        // Lexer tokens (see section A.1.1. of the manual)
        None = System.Char.MaxValue + 1,
        Number,
        True,
        False,
        Null,
        CharSeq,
        // Single char
        Char,

        // Parser Rules (see section A.2.1 of the manual)
        Text,
        Object,
        ObjectPrime,
        Pair,
        PairRest,
        Array,
        ArrayPrime,
        Value,
        ValueRest,
        String,

        // End of input
        End,

        // The empty rule
        Epsilon
    }


    /// <summary>
    /// 功能：Json异常
    /// 创建者：长生
    /// 日期：2021年11月21日14:31:00
    /// </summary>
    public class JsonException : ApplicationException
    {
        public JsonException() : base()
        {
        }

        internal JsonException(ParserToken token) :
            base(String.Format(
                    "Invalid token '{0}' in input string", token))
        {
        }

        internal JsonException(ParserToken token,
                                Exception inner_exception) :
            base(String.Format(
                    "Invalid token '{0}' in input string", token),
                inner_exception)
        {
        }

        internal JsonException(int c) :
            base(String.Format("Invalid character '{0}' in input string", (char)c))
        {
        }

        internal JsonException(int c, Exception inner_exception) :
            base(String.Format("Invalid character '{0}' in input string", (char)c),
                inner_exception)
        {
        }


        public JsonException(string message) : base(message)
        {
        }

        public JsonException(string message, Exception inner_exception) :
            base(message, inner_exception)
        {
        }
    }
}

