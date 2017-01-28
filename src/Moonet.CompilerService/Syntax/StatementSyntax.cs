
namespace Moonet.CompilerService.Syntax
{
    public abstract class StatementSyntax : SyntaxNode
    {
        public StatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class LocalDefinitionStatementSyntax : StatementSyntax
    {
        public string Name { get; }

        public ExpressionSyntax InitExpr { get; set; } = null;

        public LocalDefinitionStatementSyntax(int line, int colomn, string name) : base(line, colomn)
        {
            Name = name;
        }
    }

    //!TODO: All statements
}
