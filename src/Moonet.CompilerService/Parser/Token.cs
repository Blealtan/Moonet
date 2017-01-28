using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService.Parser
{
    internal class Token
    {
        public TokenType Type { get; }

        public Token(TokenType type)
        {
            Type = type;
        }
    }

    internal class Token<TValue> : Token
    {
        public TValue Value { get; }

        public Token(TokenType type, TValue value) : base(type)
        {
            Value = value;
        }
    }

    internal enum TokenType
    {
        Name, // Identifiers
        And, // and
        Break, // break
        Do, // do
        Else, // else
        Elseif, // elseif
        End, // end
        False, // false
        For, // for
        Function, // function
        Goto, // goto
        If, // if
        In, // in
        Local, // local
        Nil, // nil
        Not, // not
        Or, // or
        Repeat, // repeat
        Return, // return
        Then, // then
        True, // true
        Until, // until
        While, // while
        Add, // +
        Minus, // -
        Multiply, // *
        FloatDivide, // /
        FloorDivide, // //
        Modulo, // %
        Exponent, // ^
        BitAnd, // &
        BitOr, // |
        BitXorOrNot, // ~
        BitRShift, // >>
        BitLShift, // <<
        Equal, // ==
        Inequal, // ~=
        Less, // <
        Greater, // >
        LessEqual, // <=
        GreaterEqual, // >=
        Length, // #
        Concat, // ..
        Assign, // =
        LeftParen, // (
        RightParen, // )
        LeftBrace, // {
        RightBrace, // }
        LeftSquareBracket, // [
        RightSquareBracket, // ]
        LabelMark, // ::
        Semicolon, // ;
        Colon, // :
        Comma, // ,
        Dot, // .
        VarArg, // ...
        StringLiteral,
        IntegerLiteral,
        FloatLiteral,
        EndOfFile,

        // Moonet added:
        Class, // class
        Using, // using
        Namespace, // namespace
        As, // as
        New, // new
        Boolean, // boolean
        Integer, // integer
        Float, // float
        String // string
    }
}
