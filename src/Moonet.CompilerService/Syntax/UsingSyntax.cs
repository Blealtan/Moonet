namespace Moonet.CompilerService.Syntax
{
    public abstract class UsingSyntax : SyntaxNode
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
}
