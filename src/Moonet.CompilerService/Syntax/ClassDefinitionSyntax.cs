using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class ClassDefinitionSyntax : SyntaxNode
    {
        public readonly string Name;

        public readonly ICollection<string> BaseNames;

        public readonly ICollection<LocalDefinitionStatementSyntax> Fields;

        public readonly IDictionary<string, FunctionDefinitionExpressionSyntax> Members;

        public readonly IDictionary<string, FunctionDefinitionExpressionSyntax> StaticMembers;

        public ClassDefinitionSyntax(int line, int colomn,
            string name,
            ICollection<string> baseNames,
            ICollection<LocalDefinitionStatementSyntax> fields,
            IDictionary<string, FunctionDefinitionExpressionSyntax> members,
            IDictionary<string, FunctionDefinitionExpressionSyntax> staticMembers) : base(line, colomn)
        {
            Name = name;
            BaseNames = baseNames;
            Fields = fields;
            Members = members;
            StaticMembers = staticMembers;
        }
    }
}
