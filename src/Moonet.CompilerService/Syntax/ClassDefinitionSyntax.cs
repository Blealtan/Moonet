using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class ClassDefinitionSyntax : SyntaxNode
    {
        public readonly string Name;

        public readonly string[] BaseNames;

        public readonly (string name, string type, ExpressionSyntax init)[] Fields;

        public readonly (string name, FunctionDefinitionExpression func)[] Members;

        public readonly (string name, FunctionDefinitionExpression func)[] StaticMembers;

        public ClassDefinitionSyntax(int line, int colomn,
            string name,
            string[] baseNames,
            (string name, string type, ExpressionSyntax init)[] fields,
            (string name, FunctionDefinitionExpression func)[] members,
            (string name, FunctionDefinitionExpression func)[] staticMembers) : base(line, colomn)
        {
            Name = name;
            BaseNames = baseNames;
            Fields = fields;
            Members = members;
            StaticMembers = staticMembers;
        }
    }
}
