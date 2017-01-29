using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class SyntaxTree
    {
        public ICollection<UsingSyntax> Usings { get; } = new List<UsingSyntax>();

        public BlockSyntax Body { get; } = new BlockSyntax(1, 1);

        //!TODO: Class definitions

        public SyntaxTree()
        {
        }
    }
}
