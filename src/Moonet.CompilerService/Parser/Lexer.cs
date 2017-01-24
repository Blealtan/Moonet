using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moonet.CompilerService.Parser
{
    internal class Lexer
    {
        private StreamReader _input;

        public Lexer(StreamReader input)
        {
            _input = input;
        }

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
                        yield return new Token(TokenType.Add);
                        break;
                    case '-':
                        _input.Read();
                        if (_input.Peek() == '-')
                        {
                            _input.Read();
                            SkipComment();
                            break;
                        }
                        else yield return new Token(TokenType.Minus);
                        break;
                    case '*':
                        _input.Read();
                        yield return new Token(TokenType.Multiply);
                        break;
                    case '/':
                        _input.Read();
                        if (_input.Peek() == '/')
                        {
                            _input.Read();
                            yield return new Token(TokenType.FloorDivide);
                        }
                        else yield return new Token(TokenType.FloatDivide);
                        break;
                    case '%':
                        _input.Read();
                        yield return new Token(TokenType.Modulo);
                        break;
                    case '^':
                        _input.Read();
                        yield return new Token(TokenType.Exponent);
                        break;
                    case '#':
                        _input.Read();
                        yield return new Token(TokenType.Length);
                        break;
                    case '&':
                        _input.Read();
                        yield return new Token(TokenType.BitAnd);
                        break;
                    case '~':
                        _input.Read();
                        if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return new Token(TokenType.Inequal);
                        }
                        else yield return new Token(TokenType.BitXorOrNot);
                        break;
                    case '|':
                        _input.Read();
                        yield return new Token(TokenType.BitAnd);
                        break;
                    case '<':
                        if (_input.Peek() == '<')
                        {
                            _input.Read();
                            yield return new Token(TokenType.BitLShift);
                        }
                        else if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return new Token(TokenType.LessEqual);
                        }
                        else yield return new Token(TokenType.Less);
                        break;
                    case '>':
                        if (_input.Peek() == '>')
                        {
                            _input.Read();
                            yield return new Token(TokenType.BitRShift);
                        }
                        else if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return new Token(TokenType.GreaterEqual);
                        }
                        else yield return new Token(TokenType.Greater);
                        break;
                    case '=':
                        _input.Read();
                        if (_input.Peek() == '=')
                        {
                            _input.Read();
                            yield return new Token(TokenType.Equal);
                        }
                        else yield return new Token(TokenType.Assign);
                        break;
                    case '(':
                        _input.Read();
                        yield return new Token(TokenType.LeftParen);
                        break;
                    case ')':
                        _input.Read();
                        yield return new Token(TokenType.RightParen);
                        break;
                    case '{':
                        _input.Read();
                        yield return new Token(TokenType.LeftBrace);
                        break;
                    case '}':
                        _input.Read();
                        yield return new Token(TokenType.RightBrace);
                        break;
                    case ':':
                        _input.Read();
                        if (_input.Peek() == ':')
                        {
                            _input.Read();
                            yield return new Token(TokenType.LabelMark);
                        }
                        else yield return new Token(TokenType.Colon);
                        break;
                    case ';':
                        _input.Read();
                        yield return new Token(TokenType.Semicolon);
                        break;
                    case ',':
                        _input.Read();
                        yield return new Token(TokenType.Comma);
                        break;
                    case '.':
                        _input.Read();
                        if (_input.Peek() == '.')
                        {
                            _input.Read();
                            if (_input.Peek() == '.')
                            {
                                _input.Read();
                                yield return new Token(TokenType.VarArg);
                            }
                            else yield return new Token(TokenType.Concat);
                        }
                        else yield return new Token(TokenType.Dot);
                        break;
                    case '[':
                        _input.Read();
                        if (_input.Peek() == '=') yield return MatchRawString();
                        else yield return new Token(TokenType.LeftSquareBracket);
                        break;
                    case ']':
                        _input.Read();
                        yield return new Token(TokenType.RightSquareBracket);
                        break;
                    case '"':
                    case '\'':
                        yield return MatchString();
                        break;
                }
            }
        }

        public Token MatchName()
        {
            throw new NotImplementedException();
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
