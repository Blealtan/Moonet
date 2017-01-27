using System.Collections.Generic;

namespace Moonet.CompilerService.Parser
{
    public class SyntaxNode
    {
        public int Line { get; }

        public int Colomn { get; }

        public SyntaxNode(int line, int colomn)
        {
            Line = line;
            Colomn = colomn;
        }
    }

    public class ExpressionSyntax : SyntaxNode
    {
        //!TODO: Type info

        public ExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    //!TODO: All expressions

    public class StatementSyntax : SyntaxNode
    {
        public StatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    //!TODO: All statements

    public class BlockSyntax : SyntaxNode
    {
        public ICollection<StatementSyntax> Statements { get; }

        public BlockSyntax(int line, int colomn) : base(line, colomn)
        {
            Statements = new List<StatementSyntax>();
        }
    }

    public class FileSyntaxTree
    {
        public string FileName { get; }

        public BlockSyntax Body { get; }

        //!TODO: Class definitions

        public FileSyntaxTree(string filename)
        {
            FileName = filename;
            Body = new BlockSyntax(1, 1);
        }
    }
}
