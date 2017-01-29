using System;
using System.Collections.Generic;
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
    }
}
