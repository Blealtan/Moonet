using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService.Syntax
{
    public enum BinaryOperator
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

    public enum UnaryOperator
    {
        Minus, // -
        Not, // not
        Length, // #
        BinNot // ~
    }
}
