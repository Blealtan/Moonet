using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class SyntaxTree
    {
        public readonly ICollection<UsingSyntax> Usings;

        public readonly BlockSyntax Body;

        public readonly ICollection<ClassDefinitionSyntax> Classes;

        public SyntaxTree(ICollection<UsingSyntax> usings,
            BlockSyntax body,
            ICollection<ClassDefinitionSyntax> classes)
        {
            Usings = usings;
            Body = body;
            Classes = classes;
        }
    }
}
