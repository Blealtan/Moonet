using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
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

    public class UsingSyntax : SyntaxNode
    {
        public UsingSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class UsingNamespaceSyntax : UsingSyntax
    {
        public string Namespace { get; }

        public UsingNamespaceSyntax(int line, int colomn, string namespace_) : base(line, colomn)
        {
            Namespace = namespace_;
        }
    }

    public class UsingFileSyntax : UsingSyntax
    {
        public string UsingFileName { get; }

        public string AsVar { get; }

        public UsingFileSyntax(int line, int colomn, string usingFileName, string asVar) : base(line, colomn)
        {
            UsingFileName = usingFileName;
            AsVar = asVar;
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
}
