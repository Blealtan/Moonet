﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Moonet.CompilerService.Parser
{
    internal class Lexer
    {
        private readonly TextReader _input;

        public Lexer(TextReader input)
        {
            _input = input;
            CurrentLine = _input.ReadLine();
        }

        private int _line = 1;

        private int _colomn = 0;

        public string CurrentLine { get; private set; }

        private int Peek() => CurrentLine?[_colomn] ?? -1;

        private void NextChar()
        {
            if (++_colomn == CurrentLine.Length)
                NextLine();
        }

        private int Read()
        {
            var ret = Peek();
            NextChar();
            return ret;
        }

        private void NextLine()
        {
            CurrentLine = _input.ReadLine() + '\n';
            _colomn = 0;
            ++_line;
        }

        public Queue<Error> Errors { get; } = new Queue<Error>();

        private void AddError(string message)
        {
            Errors.Enqueue(new Error(_line, _colomn, message));
        }

        private readonly Dictionary<TokenType, Token> _basicTokenMap = new Dictionary<TokenType, Token>()
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
            { TokenType.VarArg, new Token(TokenType.VarArg) },
            { TokenType.EndOfFile, new Token(TokenType.EndOfFile) }
        };

        public (int line, int colomn, Token token) AnalyzeNextToken()
        {
            var x = Peek();
            Token result = null;
            var initLine = _line;
            var initCol = _colomn;

            // Skip white spaces
            while (x == ' ' || x == '\t' || x == '\n')
            {
                NextChar();
                x = Peek();
            }

            // Analyze this token
            switch (x)
            {
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
                    result = MatchName();
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
                    result = MatchNumber();
                    break;
                case '+':
                    NextChar();
                    result = _basicTokenMap[TokenType.Add];
                    break;
                case '-':
                    NextChar();
                    if (Peek() == '-')
                    {
                        NextChar();
                        SkipComment();
                        return AnalyzeNextToken();
                    }
                    else result = _basicTokenMap[TokenType.Minus];
                    break;
                case '*':
                    NextChar();
                    result = _basicTokenMap[TokenType.Multiply];
                    break;
                case '/':
                    NextChar();
                    if (Peek() == '/')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.FloorDivide];
                    }
                    else result = _basicTokenMap[TokenType.FloatDivide];
                    break;
                case '%':
                    NextChar();
                    result = _basicTokenMap[TokenType.Modulo];
                    break;
                case '^':
                    NextChar();
                    result = _basicTokenMap[TokenType.Exponent];
                    break;
                case '#':
                    NextChar();
                    result = _basicTokenMap[TokenType.Length];
                    break;
                case '&':
                    NextChar();
                    result = _basicTokenMap[TokenType.BitAnd];
                    break;
                case '~':
                    NextChar();
                    if (Peek() == '=')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.Inequal];
                    }
                    else result = _basicTokenMap[TokenType.BitXorOrNot];
                    break;
                case '|':
                    NextChar();
                    result = _basicTokenMap[TokenType.BitAnd];
                    break;
                case '<':
                    if (Peek() == '<')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.BitLShift];
                    }
                    else if (Peek() == '=')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.LessEqual];
                    }
                    else result = _basicTokenMap[TokenType.Less];
                    break;
                case '>':
                    if (Peek() == '>')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.BitRShift];
                    }
                    else if (Peek() == '=')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.GreaterEqual];
                    }
                    else result = _basicTokenMap[TokenType.Greater];
                    break;
                case '=':
                    NextChar();
                    if (Peek() == '=')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.Equal];
                    }
                    else result = _basicTokenMap[TokenType.Assign];
                    break;
                case '(':
                    NextChar();
                    result = _basicTokenMap[TokenType.LeftParen];
                    break;
                case ')':
                    NextChar();
                    result = _basicTokenMap[TokenType.RightParen];
                    break;
                case '{':
                    NextChar();
                    result = _basicTokenMap[TokenType.LeftBrace];
                    break;
                case '}':
                    NextChar();
                    result = _basicTokenMap[TokenType.RightBrace];
                    break;
                case ':':
                    NextChar();
                    if (Peek() == ':')
                    {
                        NextChar();
                        result = _basicTokenMap[TokenType.LabelMark];
                    }
                    else result = _basicTokenMap[TokenType.Colon];
                    break;
                case ';':
                    NextChar();
                    result = _basicTokenMap[TokenType.Semicolon];
                    break;
                case ',':
                    NextChar();
                    result = _basicTokenMap[TokenType.Comma];
                    break;
                case '.':
                    NextChar();
                    if (Peek() == '.')
                    {
                        NextChar();
                        if (Peek() == '.')
                        {
                            NextChar();
                            result = _basicTokenMap[TokenType.VarArg];
                        }
                        else result = _basicTokenMap[TokenType.Concat];
                    }
                    else result = _basicTokenMap[TokenType.Dot];
                    break;
                case '[':
                    NextChar();
                    if (Peek() == '=' || Peek() == '[') result = MatchRawString();
                    else result = _basicTokenMap[TokenType.LeftSquareBracket];
                    break;
                case ']':
                    NextChar();
                    result = _basicTokenMap[TokenType.RightSquareBracket];
                    break;
                case '"':
                case '\'':
                    result = MatchString();
                    break;
                case -1:
                    result = _basicTokenMap[TokenType.EndOfFile];
                    break;
                default:
                    NextChar();
                    AddError("Unknown character found; ignoring.");
                    break;
            }
            return (line: initLine, colomn: initCol, token: result);
        }

        private readonly Dictionary<string, Token> _nameTokenMap = new Dictionary<string, Token>()
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

        private Token MatchName()
        {
            int start = _colomn;
            int x;
            do
            {
                NextChar();
                x = Peek();
            }
            while ((x >= 'a' && x <= 'z') || (x >= 'A' && x <= 'Z') || (x >= '0' && x <= '9') || x == '_');
            string s = CurrentLine.Substring(start, _colomn - start);
            if (_nameTokenMap.ContainsKey(s)) return _nameTokenMap[s];
            return new Token<string>(TokenType.Name, s);
        }

        private Token MatchNumber()
        {
            throw new NotImplementedException();
        }

        private Token<string> MatchRawString()
        {
            int level = 0;
            while (Peek() == '=')
            {
                Read();
                level++;
            }
            if (Peek() != '[')
                AddError("Unrecognized lexical structure; assuming you forget a '[' at the long bracket start.");
            else Read();
            if (Peek() == '\n') Read(); // Skip newline at start.
            return new Token<string>(TokenType.String, LongBracketBody(level));
        }

        private string LongBracketBody(int level)
        {
            var endBuilder = new StringBuilder();
            endBuilder.Append(']');
            endBuilder.Append('=', level);
            endBuilder.Append(']');
            var end = endBuilder.ToString();

            var retBuilder = new StringBuilder();
            int endFound = 0;
            // Deal with the case that there are contents in the start line of the long brackets section.
            if (_colomn != 0)
            {
                // Body ends this line.
                if ((endFound = CurrentLine.IndexOf(end, _colomn)) != -1)
                {
                    var ret = CurrentLine.Substring(_colomn, endFound - _colomn);
                    _colomn = endFound + level + 2;
                    return ret;
                }
                // Body continues.
                retBuilder.Append(CurrentLine.Substring(_colomn));
                NextLine();
            }
            // Handle those lines contain neither long bracket open nor close.
            while (CurrentLine != null && (endFound = CurrentLine.IndexOf(end)) == -1)
            {
                retBuilder.Append(CurrentLine);
                NextLine();
            }

            // Handle the case that long bracket body meets EOF.
            if (CurrentLine == null)
            {
                AddError("Long bracket body meets EOF.");
                return null;
            }
            else // Handle last line.
            {
                retBuilder.Append(CurrentLine.Substring(0, endFound));
                _colomn = endFound + level + 2;
                if (_colomn == CurrentLine.Length)
                    NextLine();
                return retBuilder.ToString();
            }
        }

        private Token<string> MatchString()
        {
            throw new NotImplementedException();
        }

        private void SkipComment()
        {
            var match = -1;
            var firstLine = CurrentLine.Substring(_colomn);
            if (firstLine[0] == '[')
            {
                int i = 1;
                while (i < firstLine.Length && firstLine[i] == '=')
                    ++i;
                if (i < firstLine.Length && firstLine[i] == '[')
                    match = i;
            }
            _colomn = CurrentLine.Length;
            if (match >= 0) LongBracketBody(match);
        }
    }
}
