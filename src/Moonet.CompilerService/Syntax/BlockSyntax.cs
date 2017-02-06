namespace Moonet.CompilerService.Syntax
{

    public class BlockSyntax : SyntaxNode
    {
        public readonly StatementSyntax[] Statements;

        public readonly ReturnSyntax Return;

        public BlockSyntax(int line, int colomn, StatementSyntax[] statements, ReturnSyntax @return) : base(line, colomn)
        {
            Statements = statements;
            Return = @return;
        }
    }
}
