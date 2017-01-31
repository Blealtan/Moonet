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

    public class AssignmentStatement : StatementSyntax
    {
        public readonly ICollection<VariableExpression> Variables;

        public readonly ICollection<ExpressionSyntax> Expressions;

        public AssignmentStatement(int line, int colomn,
            ICollection<VariableExpression> variables,
            ICollection<ExpressionSyntax> expressions) : base(line, colomn)
        {
            Variables = variables;
            Expressions = expressions;
        }
    }

    public class FunctionCallStatement : StatementSyntax
    {
        public readonly FunctionCallExpression CallExpression;

        public FunctionCallStatement(int line, int colomn, FunctionCallExpression callExpression) : base(line, colomn)
        {
            CallExpression = callExpression;
        }
    }

    public class LabelStatement : StatementSyntax
    {
        public readonly string Label;

        public LabelStatement(int line, int colomn, string label) : base(line, colomn)
        {
            Label = label;
        }
    }

    public class BreakStatement : StatementSyntax
    {
        public BreakStatement(int line, int colomn) : base(line, colomn)
        {
        }
    }

    public class GoToStatement : StatementSyntax
    {
        public readonly string Label;

        public GoToStatement(int line, int colomn, string label) : base(line, colomn)
        {
            Label = label;
        }
    }

    public class DoBlockStatement : StatementSyntax
    {
        public readonly BlockSyntax Body;

        public DoBlockStatement(int line, int colomn, BlockSyntax body) : base(line, colomn)
        {
            Body = body;
        }
    }

    public class WhileLoopStatement : StatementSyntax
    {
        public readonly ExpressionSyntax Condition;

        public readonly BlockSyntax Body;

        public WhileLoopStatement(int line, int colomn, ExpressionSyntax condition, BlockSyntax body) : base(line, colomn)
        {
            Condition = condition;
            Body = body;
        }
    }

    public class RepeatLoopStatement : StatementSyntax
    {
        public readonly BlockSyntax Body;

        public readonly ExpressionSyntax Condition;

        public RepeatLoopStatement(int line, int colomn, BlockSyntax body, ExpressionSyntax condition) : base(line, colomn)
        {
            Body = body;
            Condition = condition;
        }
    }

    public class IfStatement : StatementSyntax
    {
        public readonly IList<Tuple<ExpressionSyntax, BlockSyntax>> Conditions;

        /// <summary>
        /// Null for no else presented.
        /// </summary>
        public readonly BlockSyntax ElseBody;

        public IfStatement(int line, int colomn, IList<Tuple<ExpressionSyntax, BlockSyntax>> conditions, BlockSyntax elseBody = null) : base(line, colomn)
        {
            Conditions = conditions;
            ElseBody = elseBody;
        }
    }

    public class ForStepStatement : StatementSyntax
    {
        public readonly string LoopVarName;

        public readonly ExpressionSyntax Start;

        public readonly ExpressionSyntax End;

        public readonly ExpressionSyntax Step;

        public ForStepStatement(int line, int colomn,
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

    public class ForIteratorStatement : StatementSyntax
    {
        public readonly IList<string> Names;

        public readonly IList<ExpressionSyntax> Iterator;

        public ForIteratorStatement(int line, int colomn,
            IList<string> names,
            IList<ExpressionSyntax> iterator) : base(line, colomn)
        {
            Names = names;
            Iterator = iterator;
        }
    }

    public class FunctionDefinitionStatement : StatementSyntax
    {
        /// <summary>
        /// Should add at least one element to this list.
        /// </summary>
        public readonly IList<string> ReferenceChain;

        /// <summary>
        /// Null for no member reference presented.
        /// </summary>
        public readonly string MemberName;

        public readonly FunctionDefinitionExpression Function;

        public FunctionDefinitionStatement(int line, int colomn,
            IList<string> referenceChain,
            string memberName,
            FunctionDefinitionExpression function) : base(line, colomn)
        {
            ReferenceChain = referenceChain;
            MemberName = memberName;
            Function = function;
        }
    }

    public class LocalFunctionDefinitionStatement : StatementSyntax
    {
        public readonly string Name;

        public readonly FunctionDefinitionExpression Function;

        public LocalFunctionDefinitionStatement(int line, int colomn,
            string name,
            FunctionDefinitionExpression function) : base(line, colomn)
        {
            Name = name;
            Function = function;
        }
    }

    public class LocalDefinitionStatement : StatementSyntax
    {
        public readonly IList<Tuple<string, string>> Variables;

        public readonly IList<ExpressionSyntax> InitExpressions;

        public LocalDefinitionStatement(int line, int colomn,
            IList<Tuple<string, string>> variables,
            IList<ExpressionSyntax> initExpressions) : base(line, colomn)
        {
            Variables = variables;
            InitExpressions = initExpressions;
        }
    }
}
