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
        public readonly ICollection<string> Variables;

        public readonly ICollection<ExpressionSyntax> Expressions;

        public AssignmentStatementSyntax(int line, int colomn,
            ICollection<string> variables,
            ICollection<ExpressionSyntax> expressions) : base(line, colomn)
        {
            Variables = variables;
            Expressions = expressions;
        }
    }

    public class FunctionCallStatementSyntax : StatementSyntax
    {
        public readonly FunctionCallExpressionSyntax CallExpression;

        public FunctionCallStatementSyntax(int line, int colomn, FunctionCallExpressionSyntax callExpression) : base(line, colomn)
        {
            CallExpression = callExpression;
        }
    }

    public class LabelStatementSyntax : StatementSyntax
    {
        public readonly string Label;

        public LabelStatementSyntax(int line, int colomn, string label) : base(line, colomn)
        {
            Label = label;
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
        public readonly string Label;

        public GoToStatementSyntax(int line, int colomn, string label) : base(line, colomn)
        {
            Label = label;
        }
    }

    public class DoBlockStatementSyntax : StatementSyntax
    {
        public readonly BlockSyntax Body;

        public DoBlockStatementSyntax(int line, int colomn, BlockSyntax body) : base(line, colomn)
        {
            Body = body;
        }
    }

    public class WhileLoopStatementSyntax : StatementSyntax
    {
        public readonly ExpressionSyntax Condition;

        public readonly BlockSyntax Body;

        public WhileLoopStatementSyntax(int line, int colomn, ExpressionSyntax condition, BlockSyntax body) : base(line, colomn)
        {
            Condition = condition;
            Body = body;
        }
    }

    public class RepeatLoopStatementSyntax : StatementSyntax
    {
        public readonly BlockSyntax Body;

        public readonly ExpressionSyntax Condition;

        public RepeatLoopStatementSyntax(int line, int colomn, BlockSyntax body, ExpressionSyntax condition) : base(line, colomn)
        {
            Body = body;
            Condition = condition;
        }
    }

    public class IfStatementSyntax : StatementSyntax
    {
        public readonly IList<Tuple<ExpressionSyntax, BlockSyntax>> Conditions;

        /// <summary>
        /// Null for no else presented.
        /// </summary>
        public readonly BlockSyntax ElseBody;

        public IfStatementSyntax(int line, int colomn, IList<Tuple<ExpressionSyntax, BlockSyntax>> conditions, BlockSyntax elseBody = null) : base(line, colomn)
        {
            Conditions = conditions;
            ElseBody = elseBody;
        }
    }

    public class ForStepStatementSyntax : StatementSyntax
    {
        public readonly string LoopVarName;

        public readonly ExpressionSyntax Start;

        public readonly ExpressionSyntax End;

        public readonly ExpressionSyntax Step;

        public ForStepStatementSyntax(int line, int colomn,
            string loopVarName,
            ExpressionSyntax start,
            ExpressionSyntax end,
            ExpressionSyntax step) : base(line, colomn)
        {
            LoopVarName = loopVarName;
            Start = start;
            End = end;
            Step = step;
        }
    }

    public class ForIteratorStatementSyntax : StatementSyntax
    {
        public readonly IList<string> Names;

        public readonly IList<ExpressionSyntax> Iterator;

        public ForIteratorStatementSyntax(int line, int colomn,
            IList<string> names,
            IList<ExpressionSyntax> iterator) : base(line, colomn)
        {
            Names = names;
            Iterator = iterator;
        }
    }

    public class FunctionDefinitionStatementSyntax : StatementSyntax
    {
        /// <summary>
        /// Should add at least one element to this list.
        /// </summary>
        public readonly IList<string> ReferenceChain;

        /// <summary>
        /// Null for no member reference presented.
        /// </summary>
        public readonly string MemberName;

        public readonly FunctionDefinitionExpressionSyntax Function;

        public FunctionDefinitionStatementSyntax(int line, int colomn,
            IList<string> referenceChain,
            string memberName,
            FunctionDefinitionExpressionSyntax function) : base(line, colomn)
        {
            ReferenceChain = referenceChain;
            MemberName = memberName;
            Function = function;
        }
    }

    public class LocalFunctionDefinition : StatementSyntax
    {
        public readonly string Name;

        public readonly FunctionDefinitionExpressionSyntax Function;

        public LocalFunctionDefinition(int line, int colomn,
            string name,
            FunctionDefinitionExpressionSyntax function) : base(line, colomn)
        {
            Name = name;
            Function = function;
        }
    }

    public class LocalDefinitionStatementSyntax : StatementSyntax
    {
        public readonly IList<Tuple<string, string>> Variables;

        public readonly IList<ExpressionSyntax> InitExpressions;

        public LocalDefinitionStatementSyntax(int line, int colomn,
            IList<Tuple<string, string>> variables,
            IList<ExpressionSyntax> initExpressions) : base(line, colomn)
        {
            Variables = variables;
            InitExpressions = initExpressions;
        }
    }
}
