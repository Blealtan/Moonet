using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService.Syntax
{
    public class ReturnSyntax : SyntaxNode
    {
        public readonly ExpressionSyntax[] ReturnExpressions;

        public ReturnSyntax(int line, int colomn, ExpressionSyntax[] returnExpressions) : base(line, colomn)
        {
        }
    }
}
