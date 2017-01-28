using System;

namespace Moonet.CompilerService.Syntax
{
    public abstract class SyntaxNode
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
