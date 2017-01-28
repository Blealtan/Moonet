using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class FileSyntaxTree
    {
        public string FileName { get; }

        public BlockSyntax Body { get; }

        public ICollection<UsingSyntax> Usings { get; }

        //!TODO: Class definitions

        public FileSyntaxTree(string filename)
        {
            FileName = filename;
            Body = new BlockSyntax(1, 1);
            Usings = new List<UsingSyntax>();
        }
    }
}
