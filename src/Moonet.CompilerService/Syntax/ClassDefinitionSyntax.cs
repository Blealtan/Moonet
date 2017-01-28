using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class ClassDefinitionSyntax : SyntaxNode
    {
        public string Name { get; }

        public ICollection<string> BaseNames { get; } = new List<string>();

        public ICollection<LocalDefinitionStatementSyntax> Fields { get; } = new List<LocalDefinitionStatementSyntax>();

        public IDictionary<string, FunctionDefinitionExpressionSyntax> Members { get; } = new Dictionary<string, FunctionDefinitionExpressionSyntax>();

        public IDictionary<string, FunctionDefinitionExpressionSyntax> StaticMembers { get; } = new Dictionary<string, FunctionDefinitionExpressionSyntax>();

        public ClassDefinitionSyntax(int line, int colomn, string name) : base(line, colomn)
        {
            Name = name;
        }
    }
}
