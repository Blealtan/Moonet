using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService.Parser
{
    internal class Token
    {
        private TokenType _type;
        public TokenType Type => _type;

        private object _val;
        public object Val => _val;

        public Token(TokenType type, object val)
        {
            _type = type;
            _val = val;
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
        Vararg, // ...
        String,
        Numeral
    }
}
