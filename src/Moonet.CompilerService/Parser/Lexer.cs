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

        public Token NextToken()
        {
            throw new NotImplementedException();
        }
    }
}
