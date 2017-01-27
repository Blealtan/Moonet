using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService.Parser
{
    class SyntaxNode
    {
        public int Line { get; }

        public int Colomn { get; }

        public SyntaxNode(int line, int colomn)
        {
            Line = line;
            Colomn = colomn;
        }
    }
}
