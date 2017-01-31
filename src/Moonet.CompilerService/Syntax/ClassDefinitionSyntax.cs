﻿using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class ClassDefinitionSyntax : SyntaxNode
    {
        public readonly string Name;

        public readonly ICollection<string> BaseNames;

        public readonly ICollection<LocalDefinitionStatement> Fields;

        public readonly IDictionary<string, FunctionDefinitionExpression> Members;

        public readonly IDictionary<string, FunctionDefinitionExpression> StaticMembers;

        public ClassDefinitionSyntax(int line, int colomn,
            string name,
            ICollection<string> baseNames,
            ICollection<LocalDefinitionStatement> fields,
            IDictionary<string, FunctionDefinitionExpression> members,
            IDictionary<string, FunctionDefinitionExpression> staticMembers) : base(line, colomn)
        {
            Name = name;
            BaseNames = baseNames;
            Fields = fields;
            Members = members;
            StaticMembers = staticMembers;
        }
    }
}
