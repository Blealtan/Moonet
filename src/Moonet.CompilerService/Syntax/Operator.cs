using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService.Syntax
{
    public enum BinaryOperator : byte
    {
        Add, // +
        Minus, // -
        Multiply, // *
        FloatDivide, // /
        FloorDivide, // //
        Exponent, // ^
        Modulo, // %
        BitAnd, // &
        BitXor, // ~
        BitOr, // |
        BitRShift, // >>
        BitLShift, // <<
        Concat, // ..
        Less, // <
        LessEqual, // <=
        Greater, // >
        GreaterEqual, // >=
        Equal, // ==
        Inequal, // ~=
        And, // and
        Or // or
    }

    public enum UnaryOperator : byte
    {
        Negative, // -
        Not, // not
        Length, // #
        BitNot // ~
    }

    public static class OperatorHelper
    {
        private static readonly int[] _binopPrec = new int[]
        {
            10, // +
            10, // -
            11, // *
            11, // /
            11, // //
            14, // ^
            11, // %
            6, // &
            5, // ~
            4, // |
            7, // >>
            7, // <<
            9, // ..
            3, // <
            3, // <=
            3, // >
            3, // >=
            3, // ==
            3, // ~=
            1, // and
            0 // or
        };

        public static int GetPrecedence(this BinaryOperator binop)
        {
            return _binopPrec[(byte)binop];
        }

        private static readonly int[] _binopSubExprPrec = new int[]
        {
            10, // +
            10, // -
            11, // *
            11, // /
            11, // //
            13, // ^
            11, // %
            6, // &
            5, // ~
            4, // |
            7, // >>
            7, // <<
            8, // ..
            3, // <
            3, // <=
            3, // >
            3, // >=
            3, // ==
            3, // ~=
            1, // and
            0 // or
        };

        public static int GetSubExprPrecedence(this BinaryOperator binop)
        {
            return _binopSubExprPrec[(byte)binop];
        }

        public static int GetPrecedence(this UnaryOperator unop)
        {
            return 12;
        }
    }
}
