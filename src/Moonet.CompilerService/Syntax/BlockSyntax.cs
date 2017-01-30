using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{

    public class BlockSyntax : SyntaxNode
    {
        public readonly ICollection<StatementSyntax> Statements;

        public BlockSyntax(int line, int colomn, ICollection<StatementSyntax> statements) : base(line, colomn)
        {
            Statements = statements;
        }
    }
}
