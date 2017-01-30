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
        public readonly T Value;

        public LiteralExpressionSyntax(int line, int colomn, T value) : base(line, colomn)
        {
            Value = value;
        }
    }

    public class VariableExpression : ExpressionSyntax
    {
        public readonly string Name;

        public VariableExpression(int line, int colomn, string name) : base(line, colomn)
        {
            Name = name;
        }
    }

    public class FunctionDefinitionExpressionSyntax : ExpressionSyntax
    {
        public readonly ICollection<Tuple<string, string>> Parameters;

        public readonly BlockSyntax Body;

        public FunctionDefinitionExpressionSyntax(int line, int colomn,
            ICollection<Tuple<string, string>> parameters,
            BlockSyntax body) : base(line, colomn)
        {
            Parameters = parameters;
            Body = body;
        }
    }

    public class TableConstructorExpressionSyntax : ExpressionSyntax
    {
        public readonly ICollection<Tuple<ExpressionSyntax, ExpressionSyntax>> TableDefinition;

        public TableConstructorExpressionSyntax(int line, int colomn, ICollection<Tuple<ExpressionSyntax, ExpressionSyntax>> tableDefinition) : base(line, colomn)
        {
            TableDefinition = tableDefinition;
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
        public readonly BinaryOperator Operator;

        public readonly ExpressionSyntax LHS;

        public readonly ExpressionSyntax RHS;

        public BinaryOperatorExpressionSyntax(int line, int colomn,
            BinaryOperator op,
            ExpressionSyntax lhs,
            ExpressionSyntax rhs) : base(line, colomn)
        {
            Operator = op;
            lhs = LHS;
            rhs = RHS;
        }
    }

    public class UnaryOperatorExpressionSyntax : ExpressionSyntax
    {
        public readonly UnaryOperator Operator;

        public readonly ExpressionSyntax RHS;

        public UnaryOperatorExpressionSyntax(int line, int colomn,
            UnaryOperator op,
            ExpressionSyntax rhs) : base(line, colomn)
        {
            Operator = op;
            RHS = rhs;
        }
    }

    public class FunctionCallExpressionSyntax : ExpressionSyntax
    {
        public readonly ExpressionSyntax Function;

        public readonly ICollection<ExpressionSyntax> Arguments;

        public FunctionCallExpressionSyntax(int line, int colomn,
            ExpressionSyntax function,
            ICollection<ExpressionSyntax> arguments) : base(line, colomn)
        {
            Function = function;
            Arguments = arguments;
        }
    }
}
