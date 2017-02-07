using Moonet.CompilerService.Syntax;
using System;
using System.Collections.Generic;

namespace Moonet.CompilerService.Semantic
{
    public class SemanticTree
    {
        private Queue<Error> _semaErrors;

        public SemanticTree(SyntaxTree syntaxTree, Queue<Error> semaErrors)
        {
            _semaErrors = semaErrors;
            //!TODO: Semantic checking and build the tree.
            throw new NotImplementedException();
        }
    }
}
