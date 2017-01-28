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
        public T LiteralValue { get; }

        public LiteralExpressionSyntax(int line, int colomn, T val) : base(line, colomn)
        {
            LiteralValue = val;
        }
    }

    public class VariableExpression : ExpressionSyntax
    {
        public string Name { get; }

        public VariableExpression(int line, int colomn, string name) : base(line, colomn)
        {
            Name = name;
        }
    }

    public class FunctionDefinitionExpressionSyntax : ExpressionSyntax
    {
        public ICollection<Tuple<string, string>> Parameters { get; } = new List<Tuple<string, string>>();

        public BlockSyntax Body { get; }

        public FunctionDefinitionExpressionSyntax(int line, int colomn) : base(line, colomn)
        {
            Body = new BlockSyntax(line, colomn);
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
        public BinaryOperator Operator { get; }

        public ExpressionSyntax LHS { get; set; }

        public ExpressionSyntax RHS { get; set; }

        public BinaryOperatorExpressionSyntax(int line, int colomn, BinaryOperator op) : base(line, colomn)
        {
            Operator = op;
        }
    }

    public class UnaryOperatorExpressionSyntax : ExpressionSyntax
    {
        public UnaryOperator Operator { get; }

        public ExpressionSyntax RHS { get; set; }

        public UnaryOperatorExpressionSyntax(int line, int colomn, UnaryOperator op) : base(line, colomn)
        {
            Operator = op;
        }
    }

    public class FunctionCallExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Function { get; }

        public ICollection<ExpressionSyntax> Arguments { get; } = new List<ExpressionSyntax>();

        public FunctionCallExpressionSyntax(int line, int colomn, ExpressionSyntax func) : base(line, colomn)
        {
            Function = func;
        }
    }
}
