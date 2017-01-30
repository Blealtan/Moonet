using System;

namespace Moonet.CompilerService.Syntax
{
    public abstract class SyntaxNode
    {
        public readonly int Line;

        public readonly int Colomn;

        public SyntaxNode(int line, int colomn)
        {
            Line = line;
            Colomn = colomn;
        }
    }
}
