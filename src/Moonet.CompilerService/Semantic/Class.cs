using Moonet.CompilerService.Syntax;
using System;
using System.Collections.Generic;

namespace Moonet.CompilerService.Semantic
{
    public class Class : SemanticNode
    {
        public string Name { get; private set; }

        public Class(ClassDefinitionSyntax syntax, Queue<Error> errors)
        {
            throw new NotImplementedException();
        }
    }
}
