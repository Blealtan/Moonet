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
        public readonly string Namespace;

        public UsingNamespaceSyntax(int line, int colomn, string @namespace) : base(line, colomn)
        {
            Namespace = @namespace;
        }
    }

    public class UsingFileSyntax : UsingSyntax
    {
        public readonly string UsingFileName;

        public UsingFileSyntax(int line, int colomn, string usingFileName) : base(line, colomn)
        {
            UsingFileName = usingFileName;
        }
    }
}
