using System;
using System.Collections.Generic;

namespace Moonet.CompilerService.Syntax
{
    public abstract class StatementSyntax : SyntaxNode
    {
        public StatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class AssignmentStatementSyntax : StatementSyntax
    {
        public ICollection<string> Variables { get; } = new List<string>();

        public ICollection<ExpressionSyntax> Expressions { get; } = new List<ExpressionSyntax>();

        public AssignmentStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class FunctionCallStatementSyntax : StatementSyntax
    {
        public FunctionCallExpressionSyntax CallExpression { get; set; }

        public FunctionCallStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class LabelStatementSyntax : StatementSyntax
    {
        public string Label { get; set; }

        public LabelStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class BreakStatementSyntax : StatementSyntax
    {
        public BreakStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class GoToStatementSyntax : StatementSyntax
    {
        public string Label { get; set; }

        public GoToStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class DoBlockStatementSyntax : StatementSyntax
    {
        public BlockSyntax Body { get; set; }

        public DoBlockStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class WhileLoopStatementSyntax : StatementSyntax
    {
        public ExpressionSyntax Condition { get; set; }

        public BlockSyntax Body { get; set; }

        public WhileLoopStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class RepeatLoopStatementSyntax : StatementSyntax
    {
        public BlockSyntax Body { get; set; }

        public ExpressionSyntax Condition { get; set; }

        public RepeatLoopStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class IfStatementSyntax : StatementSyntax
    {
        public IList<Tuple<ExpressionSyntax, BlockSyntax>> Conditions { get; } = new List<Tuple<ExpressionSyntax, BlockSyntax>>();

        /// <summary>
        /// Null for no else presented.
        /// </summary>
        public BlockSyntax ElseBody { get; set; }

        public IfStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class LocalDefinitionStatementSyntax : StatementSyntax
    {
        public string Name { get; set; }

        public ExpressionSyntax InitExpr { get; set; }

        public LocalDefinitionStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }
}
