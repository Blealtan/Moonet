using System;
using System.Collections.Generic;
using System.Globalization;
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
            Errors.Enqueue(new Error(_line, _colomn, CurrentLine, message));
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
            { "while", new Token(TokenType.While) },
            { "class", new Token(TokenType.Class) },
            { "using", new Token(TokenType.Using) },
            { "new", new Token(TokenType.New) },
            { "boolean", new Token(TokenType.Boolean) },
            { "integer", new Token(TokenType.Integer) },
            { "float", new Token(TokenType.Float) },
            { "string", new Token(TokenType.String) }
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
            int pow(int x, int y)
            {
                int r = 1;
                while (y != 0)
                {
                    if ((y & 1) != 0) r *= x;
                    y >>= 1;
                    x *= x;
                }
                return r;
            }

            int number(char ch) => ch <= '9' ? ch - '0' : ch - 'a' + 10;

            string src = CurrentLine.Substring(_colomn).ToLower();
            int pos, scale, powerBase;
            bool allowAlphabet;
            if (src.StartsWith("0x")) // Hexadecimal
            {
                pos = 2;
                scale = 16;
                powerBase = 2;
                allowAlphabet = true;
            }
            else
            {
                pos = 0;
                scale = 10;
                powerBase = 10;
                allowAlphabet = false;
            }
            // Integer part.
            int integerPart = 0;
            while ((src[pos] >= '0' && src[pos] <= '9') || (allowAlphabet && src[pos] >= 'a' && src[pos] <= 'f'))
                integerPart = integerPart * scale + number(src[pos++]);
            // If no float part or exponent, it's a integer.
            if (src[pos] != '.' && src[pos] != 'p')
            {
                _colomn += pos;
                return new Token<int>(TokenType.IntegerLiteral, integerPart);
            }

            // Then for floats.
            double floatResult = integerPart;
            // Float part.
            if (src[pos] == '.')
            {
                ++pos;
                int floatLen = 0;
                int floatPart = 0;
                while ((src[pos] >= '0' && src[pos] <= '9') || (allowAlphabet && src[pos] >= 'a' && src[pos] <= 'f'))
                {
                    floatPart = floatPart * scale + number(src[pos++]);
                    ++floatLen;
                }
                floatResult += floatPart / (double)pow(scale, floatLen);
            }
            // Exponent.
            if (src[pos] == 'p')
            {
                ++pos;
                bool neg = false;
                if (src[pos] == '+') ++pos;
                else if (src[pos] == '-') { neg = true; ++pos; }

                // Actual exponent number.
                int exp = 0;
                while (src[pos] >= '0' && src[pos] <= '9')
                {
                    exp *= 10;
                    exp += src[pos] - '0';
                    ++pos;
                }
                if (neg) floatResult /= pow(powerBase, exp);
                else floatResult *= pow(powerBase, exp);
            }
            _colomn += pos;
            return new Token<double>(TokenType.FloatLiteral, floatResult);
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
            return new Token<string>(TokenType.StringLiteral, LongBracketBody(level));
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
            var quote = (char)Read();
            var sb = new StringBuilder();

            var closePos = CurrentLine.IndexOf(quote, _colomn);
            if (closePos == -1) closePos = CurrentLine.Length;
            int slashPos;
            while ((slashPos = CurrentLine.IndexOf(quote, _colomn)) < closePos)
            {
                if (slashPos == -1) break;
                sb.Append(CurrentLine.Substring(_colomn, slashPos - _colomn));
                switch (CurrentLine[slashPos + 1])
                {
                    // Some normal escape sequences.
                    case 'a': _colomn = slashPos + 2; sb.Append('\a'); break;
                    case 'b': _colomn = slashPos + 2; sb.Append('\b'); break;
                    case 'f': _colomn = slashPos + 2; sb.Append('\f'); break;
                    case 'n': _colomn = slashPos + 2; sb.Append('\n'); break;
                    case 'r': _colomn = slashPos + 2; sb.Append('\r'); break;
                    case 't': _colomn = slashPos + 2; sb.Append('\t'); break;
                    case 'v': _colomn = slashPos + 2; sb.Append('\v'); break;
                    case '\\': _colomn = slashPos + 2; sb.Append('\\'); break;
                    // Quotes.
                    case '\'':
                    case '\"':
                        _colomn = slashPos + 2;
                        closePos = CurrentLine.IndexOf(quote, _colomn);
                        sb.Append(CurrentLine[slashPos + 1]);
                        break;
                    // Decimal.
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
                        char n = (char)0;
                        for (int i = 1; i < 4; i++)
                        {
                            if (CurrentLine[slashPos + i] >= '0' && CurrentLine[slashPos + i] <= '9')
                                break;
                            else
                                n = (char)(n * 10 + CurrentLine[slashPos + i] - '0');
                        }
                        sb.Append(n);
                        break;
                    // Hexadecimal.
                    case 'x':
                        int x;
                        if (!int.TryParse(CurrentLine.Substring(slashPos + 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out x))
                        {
                            AddError(@"Expected two hexadecimal digits for '\x' escape sequence.");
                            x = 'x';
                            _colomn = slashPos + 2;
                        }
                        else _colomn = slashPos + 4;
                        sb.Append((char)x);
                        break;
                    case 'u':
                        int u;
                        if (!int.TryParse(CurrentLine.Substring(slashPos + 2, 3), out u))
                        {
                            AddError(@"Expected two hexadecimal digits for '\u' escape sequence.");
                            u = 'u';
                            _colomn = slashPos + 2;
                        }
                        else _colomn = slashPos + 4;
                        sb.Append((char)u);
                        break;
                    case '\n':
                        NextLine();
                        closePos = CurrentLine.IndexOf(quote, _colomn);
                        if (closePos == -1) closePos = CurrentLine.Length;
                        sb.Append('\n');
                        break;
                    default:
                        AddError("Unrecognized escape sequence.");
                        _colomn = slashPos + 2;
                        sb.Append(CurrentLine[slashPos + 1]);
                        break;
                }
            }
            if (closePos == CurrentLine.Length)
            {
                AddError("Unexpected line ending in literal string.");
                sb.Append(CurrentLine.Substring(_colomn));
                NextLine();
            }
            else
            {
                sb.Append(CurrentLine.Substring(_colomn, closePos - _colomn));
                _colomn = closePos + 1;
            }
            return new Token<string>(TokenType.StringLiteral, sb.ToString());
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
