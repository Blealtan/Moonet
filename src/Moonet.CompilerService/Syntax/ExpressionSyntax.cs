using System;
using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public abstract class ExpressionSyntax : SyntaxNode
    {
        public ExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class LiteralExpressionSyntax<T> : ExpressionSyntax
    {
        public T LiteralValue { get; set; }

        public LiteralExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class VariableExpression : ExpressionSyntax
    {
        public string Name { get; }

        public VariableExpression(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class FunctionDefinitionExpressionSyntax : ExpressionSyntax
    {
        public ICollection<Tuple<string, string>> Parameters { get; } = new List<Tuple<string, string>>();

        public BlockSyntax Body { get; set; }

        public FunctionDefinitionExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class TableConstructorExpressionSyntax : ExpressionSyntax
    {
        public ICollection<Tuple<ExpressionSyntax, ExpressionSyntax>> TableDefinition { get; } = new List<Tuple<ExpressionSyntax, ExpressionSyntax>>();

        public TableConstructorExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class VarArgExpressionSyntax : ExpressionSyntax
    {
        public VarArgExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class BinaryOperatorExpressionSyntax : ExpressionSyntax
    {
        public BinaryOperator Operator { get; set; }

        public ExpressionSyntax LHS { get; set; }

        public ExpressionSyntax RHS { get; set; }

        public BinaryOperatorExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class UnaryOperatorExpressionSyntax : ExpressionSyntax
    {
        public UnaryOperator Operator { get; set; }

        public ExpressionSyntax RHS { get; set; }

        public UnaryOperatorExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class FunctionCallExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Function { get; set; }

        public ICollection<ExpressionSyntax> Arguments { get; } = new List<ExpressionSyntax>();

        public FunctionCallExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }
}
