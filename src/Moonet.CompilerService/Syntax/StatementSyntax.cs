using System;
using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public class StatementSyntax : SyntaxNode
    {
        public StatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class LocalDefinitionStatementSyntax : StatementSyntax
    {
        public string Name { get; }

        public ExpressionSyntax InitExpr { get; set; } = null;

        public LocalDefinitionStatementSyntax(int line, int colomn, string name) : base(line, colomn)
        {
            Name = name;
        }
    }

    public class FunctionDefinitionStatementSyntax : StatementSyntax
    {
        public ICollection<Tuple<string, string>> Parameters { get; } = new List<Tuple<string, string>>();

        public BlockSyntax Body { get; }

        public FunctionDefinitionStatementSyntax(int line, int colomn) : base(line, colomn)
        {
            Body = new BlockSyntax(line, colomn);
        }
    }

    //!TODO: All statements
}
