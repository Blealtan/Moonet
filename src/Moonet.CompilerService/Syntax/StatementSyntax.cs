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

    public class ForStepStatementSyntax : StatementSyntax
    {
        public string LoopVarName { get; set; }

        public ExpressionSyntax Start { get; set; }

        public ExpressionSyntax End { get; set; }

        public ExpressionSyntax Step { get; set; }

        public ForStepStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class ForIteratorStatementSyntax : StatementSyntax
    {
        public IList<string> Names { get; } = new List<string>();

        public IList<ExpressionSyntax> Iterator { get; } = new List<ExpressionSyntax>();

        public ForIteratorStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class FunctionDefinitionStatementSyntax : StatementSyntax
    {
        /// <summary>
        /// Should add at least one element to this list.
        /// </summary>
        public IList<string> ReferenceChain { get; } = new List<string>();

        /// <summary>
        /// Null for no member reference presented.
        /// </summary>
        public string MemberName { get; set; } = null;

        public FunctionDefinitionExpressionSyntax Function { get; set; }

        public FunctionDefinitionStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class LocalFunctionDefinition : StatementSyntax
    {
        public string Name { get; set; }

        public FunctionDefinitionExpressionSyntax Function { get; set; }

        public LocalFunctionDefinition(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class LocalDefinitionStatementSyntax : StatementSyntax
    {
        public IList<Tuple<string, string>> Variables { get; } = new List<Tuple<string, string>>();

        public IList<ExpressionSyntax> InitExpressions { get; } = new List<ExpressionSyntax>();

        public LocalDefinitionStatementSyntax(int line, int colomn) : base(line, colomn)
        {
        }
    }
}
