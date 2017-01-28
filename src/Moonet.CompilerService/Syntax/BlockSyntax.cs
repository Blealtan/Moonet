using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{

    public class BlockSyntax : SyntaxNode
    {
        public ICollection<StatementSyntax> Statements { get; } = new List<StatementSyntax>();

        public BlockSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }
}
