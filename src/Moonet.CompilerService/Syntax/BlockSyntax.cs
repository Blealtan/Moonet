namespace Moonet.CompilerService.Syntax
{

    public class BlockSyntax : SyntaxNode
    {
        public readonly StatementSyntax[] Statements;

        public BlockSyntax(int line, int colomn, StatementSyntax[] statements) : base(line, colomn)
        {
            Statements = statements;
        }
    }
}
