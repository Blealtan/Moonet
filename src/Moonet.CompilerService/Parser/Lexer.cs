using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Moonet.CompilerService.Parser
{
    internal class Lexer
    {
        private StreamReader _input;

        public Lexer(StreamReader input)
        {
            _input = input;
        }

        private static Dictionary<TokenType, Token> _basicTokenMap = new Dictionary<TokenType, Token>()
        {
            { TokenType.Add, new Token(TokenType.Add) },
            { TokenType.Minus, new Token(TokenType.Minus) },
            { TokenType.Multiply, new Token(TokenType.Multiply) },
            { TokenType.FloatDivide, new Token(TokenType.FloatDivide) },
            { TokenType.FloorDivide, new Token(TokenType.FloorDivide) },
            { TokenType.Modulo, new Token(TokenType.Modulo) },
            { TokenType.Exponent, new Token(TokenType.Exponent) },
            { TokenType.BitAnd, new Token(TokenType.BitAnd) },
            { TokenType.BitOr, new Token(TokenType.BitOr) },
            { TokenType.BitXorOrNot, new Token(TokenType.BitXorOrNot) },
            { TokenType.BitRShift, new Token(TokenType.BitRShift) },
            { TokenType.BitLShift, new Token(TokenType.BitLShift) },
            { TokenType.Equal, new Token(TokenType.Equal) },
            { TokenType.Inequal, new Token(TokenType.Inequal) },
            { TokenType.Less, new Token(TokenType.Less) },
            { TokenType.Greater, new Token(TokenType.Greater) },
            { TokenType.LessEqual, new Token(TokenType.LessEqual) },
            { TokenType.GreaterEqual, new Token(TokenType.GreaterEqual) },
            { TokenType.Length, new Token(TokenType.Length) },
            { TokenType.Concat, new Token(TokenType.Concat) },
            { TokenType.Assign, new Token(TokenType.Assign) },
            { TokenType.LeftParen, new Token(TokenType.LeftParen) },
            { TokenType.RightParen, new Token(TokenType.RightParen) },
            { TokenType.LeftBrace, new Token(TokenType.LeftBrace) },
            { TokenType.RightBrace, new Token(TokenType.RightBrace) },
            { TokenType.LeftSquareBracket, new Token(TokenType.LeftSquareBracket) },
            { TokenType.RightSquareBracket, new Token(TokenType.RightSquareBracket) },
            { TokenType.LabelMark, new Token(TokenType.LabelMark) },
            { TokenType.Semicolon, new Token(TokenType.Semicolon) },
            { TokenType.Colon, new Token(TokenType.Colon) },
            { TokenType.Comma, new Token(TokenType.Comma) },
            { TokenType.Dot, new Token(TokenType.Dot) },
            { TokenType.VarArg, new Token(TokenType.VarArg) }
        };

        public IEnumerable<Token> AnalyzeTokenStream()
        {
            while (!_input.EndOfStream)
            {
                switch (_input.Peek())
                {
                    case ' ':
                    case '\n':
                    case '\t':
                        break;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                    case '_':
                        yield return MatchName();
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        yield return MatchNumber();
                        break;
                    case '+':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.Add];
                        break;
                    case '-':
                        _input.Read();
                        if (_input.Peek() == '-')
                        {
                            _input.Read();
                            SkipComment();
                            break;
                        }
                        else yield return _basicTokenMap[TokenType.Minus];
                        break;
                    case '*':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.Multiply];
                        break;
                    case '/':
                        _input.Read();
                        if (_input.Peek() == '/')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.FloorDivide];
                        }
                        else yield return _basicTokenMap[TokenType.FloatDivide];
                        break;
                    case '%':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.Modulo];
                        break;
                    case '^':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.Exponent];
                        break;
                    case '#':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.Length];
                        break;
                    case '&':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.BitAnd];
                        break;
                    case '~':
                        _input.Read();
                        if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.Inequal];
                        }
                        else yield return _basicTokenMap[TokenType.BitXorOrNot];
                        break;
                    case '|':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.BitAnd];
                        break;
                    case '<':
                        if (_input.Peek() == '<')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.BitLShift];
                        }
                        else if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.LessEqual];
                        }
                        else yield return _basicTokenMap[TokenType.Less];
                        break;
                    case '>':
                        if (_input.Peek() == '>')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.BitRShift];
                        }
                        else if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.GreaterEqual];
                        }
                        else yield return _basicTokenMap[TokenType.Greater];
                        break;
                    case '=':
                        _input.Read();
                        if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.Equal];
                        }
                        else yield return _basicTokenMap[TokenType.Assign];
                        break;
                    case '(':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.LeftParen];
                        break;
                    case ')':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.RightParen];
                        break;
                    case '{':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.LeftBrace];
                        break;
                    case '}':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.RightBrace];
                        break;
                    case ':':
                        _input.Read();
                        if (_input.Peek() == ':')
                        {
                            _input.Read();
                            yield return _basicTokenMap[TokenType.LabelMark];
                        }
                        else yield return _basicTokenMap[TokenType.Colon];
                        break;
                    case ';':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.Semicolon];
                        break;
                    case ',':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.Comma];
                        break;
                    case '.':
                        _input.Read();
                        if (_input.Peek() == '.')
                        {
                            _input.Read();
                            if (_input.Peek() == '.')
                            {
                                _input.Read();
                                yield return _basicTokenMap[TokenType.VarArg];
                            }
                            else yield return _basicTokenMap[TokenType.Concat];
                        }
                        else yield return _basicTokenMap[TokenType.Dot];
                        break;
                    case '[':
                        _input.Read();
                        if (_input.Peek() == '=') yield return MatchRawString();
                        else yield return _basicTokenMap[TokenType.LeftSquareBracket];
                        break;
                    case ']':
                        _input.Read();
                        yield return _basicTokenMap[TokenType.RightSquareBracket];
                        break;
                    case '"':
                    case '\'':
                        yield return MatchString();
                        break;
                }
            }
        }

        private static Dictionary<string, Token> _nameTokenMap = new Dictionary<string, Token>()
        {
            { "and", new Token(TokenType.And) },
            { "break", new Token(TokenType.Break) },
            { "do", new Token(TokenType.Do) },
            { "else", new Token(TokenType.Else) },
            { "elseif", new Token(TokenType.Elseif) },
            { "end", new Token(TokenType.End) },
            { "false", new Token(TokenType.False) },
            { "for", new Token(TokenType.For) },
            { "function", new Token(TokenType.Function) },
            { "goto", new Token(TokenType.Goto) },
            { "if", new Token(TokenType.If) },
            { "in", new Token(TokenType.In) },
            { "local", new Token(TokenType.Local) },
            { "nil", new Token(TokenType.Nil) },
            { "not", new Token(TokenType.Not) },
            { "or", new Token(TokenType.Or) },
            { "repeat", new Token(TokenType.Repeat) },
            { "return", new Token(TokenType.Return) },
            { "then", new Token(TokenType.Then) },
            { "true", new Token(TokenType.True) },
            { "until", new Token(TokenType.Until) },
            { "while", new Token(TokenType.While) }
        };

        public Token MatchName()
        {
            string res = "" + (char)_input.Read();
            int x = _input.Peek();
            while ((x >= 'a' && x <= 'z') || (x >= 'A' && x <= 'Z') || (x >= '0' && x <= '9') || x == '_')
            {
                res += (char)_input.Read();
                x = _input.Peek();
            }
            if (_nameTokenMap.ContainsKey(res)) return _nameTokenMap[res];
            return new Token(TokenType.Name, res);
        }

        public Token MatchNumber()
        {
            throw new NotImplementedException();
        }

        public Token MatchRawString()
        {
            throw new NotImplementedException();
        }

        public Token MatchString()
        {
            throw new NotImplementedException();
        }

        public void SkipComment()
        {
            throw new NotImplementedException();
        }
    }
}
