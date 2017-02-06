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

    public class LiteralExpressionSyntax : ExpressionSyntax
    {
        public readonly LiteralType Type;

        public LiteralExpressionSyntax(int line, int colomn, LiteralType type) : base(line, colomn)
        {
            Type = type;
        }
    }

    public class LiteralExpressionSyntax<T> : LiteralExpressionSyntax
    {
        public readonly T Value;

        public LiteralExpressionSyntax(int line, int colomn, LiteralType type, T value) : base(line, colomn, type)
        {
            Value = value;
        }
    }

    public abstract class VariableExpression : ExpressionSyntax
    {
        public VariableExpression(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class StandaloneVariableExpression : VariableExpression
    {
        public readonly string Name;

        public StandaloneVariableExpression(int line, int colomn, string name) : base(line, colomn)
        {
            Name = name;
        }
    }

    public class IndexedVariableExpression : VariableExpression
    {
        public readonly ExpressionSyntax Table;

        public readonly ExpressionSyntax Key;

        public IndexedVariableExpression(int line, int colomn,
            ExpressionSyntax table,
            ExpressionSyntax key) : base(line, colomn)
        {
            Table = table;
            Key = key;
        }
    }

    public class FieldIndexedVariableExpression : VariableExpression
    {
        public readonly ExpressionSyntax Table;

        public readonly string Key;

        public FieldIndexedVariableExpression(int line, int colomn,
            ExpressionSyntax table,
            string key) : base(line, colomn)
        {
            Table = table;
            Key = key;
        }
    }

    public class FunctionDefinitionExpression : ExpressionSyntax
    {
        public readonly (string name, string type)[] Parameters;

        public readonly bool HasVarArgs;

        public readonly BlockSyntax Body;

        public FunctionDefinitionExpression(int line, int colomn,
            (string name, string type)[] parameters,
            bool hasVarArgs,
            BlockSyntax body) : base(line, colomn)
        {
            Parameters = parameters;
            HasVarArgs = hasVarArgs;
            Body = body;
        }
    }

    public class TableConstructorExpression : ExpressionSyntax
    {
        public readonly (ExpressionSyntax key, ExpressionSyntax value)[] TableDefinition;

        public TableConstructorExpression(int line, int colomn, (ExpressionSyntax key, ExpressionSyntax value)[] tableDefinition) : base(line, colomn)
        {
            TableDefinition = tableDefinition;
        }
    }

    public class VarArgExpression : ExpressionSyntax
    {
        public VarArgExpression(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class BinaryOperatorExpression : ExpressionSyntax
    {
        public readonly BinaryOperator Operator;

        public readonly ExpressionSyntax LHS;

        public readonly ExpressionSyntax RHS;

        public BinaryOperatorExpression(int line, int colomn,
            BinaryOperator op,
            ExpressionSyntax lhs,
            ExpressionSyntax rhs) : base(line, colomn)
        {
            Operator = op;
            lhs = LHS;
            rhs = RHS;
        }
    }

    public class UnaryOperatorExpression : ExpressionSyntax
    {
        public readonly UnaryOperator Operator;

        public readonly ExpressionSyntax RHS;

        public UnaryOperatorExpression(int line, int colomn,
            UnaryOperator op,
            ExpressionSyntax rhs) : base(line, colomn)
        {
            Operator = op;
            RHS = rhs;
        }
    }

    public class FunctionCallExpression : ExpressionSyntax
    {
        public readonly ExpressionSyntax Function;

        public readonly ExpressionSyntax[] Arguments;

        public FunctionCallExpression(int line, int colomn,
            ExpressionSyntax function,
            ExpressionSyntax[] arguments) : base(line, colomn)
        {
            Function = function;
            Arguments = arguments;
        }
    }
}
