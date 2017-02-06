using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class ClassDefinitionSyntax : SyntaxNode
    {
        public readonly string Name;

        public readonly string[] BaseNames;

        public readonly (string, string, ExpressionSyntax)[] Fields;

        public readonly (string, FunctionDefinitionExpression)[] Members;

        public readonly (string, FunctionDefinitionExpression>)[] StaticMembers;

        public ClassDefinitionSyntax(int line, int colomn,
            string name,
            string[] baseNames,
            (string, string, ExpressionSyntax)[] fields,
            (string, FunctionDefinitionExpression>)[] members,
            (string, FunctionDefinitionExpression>)[] staticMembers) : base(line, colomn)
        {
            Name = name;
            BaseNames = baseNames;
            Fields = fields;
            Members = members;
            StaticMembers = staticMembers;
        }
    }
}
