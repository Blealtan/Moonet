using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class FileSyntaxTree
    {
        public string FileName { get; }

        public BlockSyntax Body { get; } = new BlockSyntax(1, 1);

        public ICollection<UsingSyntax> Usings { get; } = new List<UsingSyntax>();

        //!TODO: Class definitions

        public FileSyntaxTree(string filename)
        {
            FileName = filename;
        }
    }
}
