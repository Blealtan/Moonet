using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class ClassDefinitionSyntax : SyntaxNode
    {
        public string Name { get; }

        public ICollection<string> BaseNames { get; } = new List<string>();

        public ICollection<LocalDefinitionStatementSyntax> Fields { get; } = new List<LocalDefinitionStatementSyntax>();

        public IDictionary<string, FunctionDefinitionStatementSyntax> Members { get; } = new Dictionary<string, FunctionDefinitionStatementSyntax>();

        public IDictionary<string, FunctionDefinitionStatementSyntax> StaticMembers { get; } = new Dictionary<string, FunctionDefinitionStatementSyntax>();

        public ClassDefinitionSyntax(int line, int colomn, string name) : base(line, colomn)
        {
            Name = name;
        }
    }
}
