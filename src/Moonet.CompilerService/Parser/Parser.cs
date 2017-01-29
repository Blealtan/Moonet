using Moonet.CompilerService.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moonet.CompilerService.Parser
{
    public class Parser
    {
        private int _line, _colomn;

        private Token _current;

        private Lexer _lexer;

        private Token Accept(TokenType type)
        {
            if (_current.Type == type)
            {
                var res = _current;
                (_line, _colomn, _current) = _lexer.AnalyzeNextToken();
                return res;
            }
            else return null;
        }

        public Queue<Error> ErrorQueue { get; }

        private void AddError(string message)
        {
            ErrorQueue.Enqueue(new Error(_line, _colomn, _lexer.CurrentLine, message));
        }

        public Parser(TextReader src)
        {
            ErrorQueue = new Queue<Error>();
            _lexer = new Lexer(src, ErrorQueue);
        }

        public FileSyntaxTree Parse()
        {
            throw new NotImplementedException();
        }
    }
}
