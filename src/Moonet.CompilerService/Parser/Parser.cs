using Moonet.CompilerService.Syntax;
using System.Collections.Generic;
using System.IO;
using System;

namespace Moonet.CompilerService.Parser
{
    static class ParserHelper
    {
        internal static void AddIfNonNull<T>(this ICollection<T> coll, T t) where T : class
        {
            if (t != null) coll.Add(t);
        }
    }

    public class Parser
    {
        #region Lexer Helper
        private int _line, _colomn;

        private Token _current;

        private TokenType Type => _current.Type;

        private int IntegerValue => (_current as Token<int>).Value;

        private double FloatValue => (_current as Token<double>).Value;

        private string StringValue => (_current as Token<string>).Value;

        private Lexer _lexer;

        private void Next()
        {
            (_line, _colomn, _current) = _lexer.AnalyzeNextToken();
        }
        #endregion

        #region Error Helper
        public Queue<Error> ErrorQueue { get; }

        private void AddError(string message)
        {
            ErrorQueue.Enqueue(new Error(_line, _colomn, _lexer.CurrentLine, message));
        }
        #endregion

        public Parser(TextReader src)
        {
            ErrorQueue = new Queue<Error>();
            _lexer = new Lexer(src, ErrorQueue);
        }

        public SyntaxTree Parse()
        {
            // Start prediction
            Next();
            var usings = ParseUsing();
            if (usings == null) return null;

            var (body, classes) = ParseBody();
            if (body == null) return null;

            return new SyntaxTree(usings, body, classes);
        }

        private UsingSyntax[] ParseUsing()
        {
            var usings = new List<UsingSyntax>();
            while (Type == TokenType.Using)
            {
                int line = _line, colomn = _colomn;
                Next();
                switch (Type)
                {
                    case TokenType.StringLiteral:
                        var file = (_current as Token<string>).Value;
                        Next();
                        usings.Add(new UsingFileSyntax(line, colomn, file));
                        break;
                    case TokenType.Namespace:
                        Next();
                        if (_current.Type == TokenType.StringLiteral)
                        {
                            usings.Add(new UsingNamespaceSyntax(line, colomn, StringValue));
                            Next();
                        }
                        else
                            AddError("Expected string literal after 'namespace' in using statement.");
                        break;
                    default:
                        AddError("Expected string literal or 'namespace' after 'using' in using statement.");
                        continue;
                }
            }
            return usings.ToArray();
        }

        private (BlockSyntax body, ClassDefinitionSyntax[] classes) ParseBody()
        {
            var initLine = _line;
            var initColomn = _colomn;

            var statements = new List<StatementSyntax>();
            var classes = new List<ClassDefinitionSyntax>();
            var @return = null as ReturnSyntax;

            while (Type != TokenType.EndOfFile)
            {
                if (Type == TokenType.Class)
                    classes.AddIfNonNull(ParseClass());
                else if (_statementFirst.Contains(Type))
                    statements.AddIfNonNull(ParseStatement());
                else if (Type == TokenType.Return)
                {
                    @return = ParseReturn();
                    break;
                }
                else
                {
                    AddError("Unrecognized token.");
                    Next();
                }
            }

            if (Type != TokenType.EndOfFile)
                AddError("Return in root block should occur only right before end of file.");

            return (new BlockSyntax(initLine, initColomn, statements.ToArray(), @return), classes.ToArray());
        }

        private ClassDefinitionSyntax ParseClass()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'class'

            // Process class name
            if (Type != TokenType.Name)
            {
                AddError("Expected class name after 'class' in class definition; ignoring class definition.");
                return null;
            }
            var name = StringValue;
            Next(); // Eat class name

            // Process base classes list
            var bases = new List<string>();
            if (Type == TokenType.Colon)
            {
                do
                {
                    Next();
                    if (Type != TokenType.Name)
                        AddError("Expected class name in super class list of a class definition; ignoring.");
                    else
                    {
                        bases.Add(StringValue);
                        Next();
                    }
                } while (Type == TokenType.Comma);
            }

            // Process members
            var fields = new List<(string, string, ExpressionSyntax)>();
            var members = new List<(string, FunctionDefinitionExpression)>();
            var staticMembers = new List<(string, FunctionDefinitionExpression)>();
            while (true)
            {
                switch (Type)
                {
                    case TokenType.Name:
                        var fieldName = StringValue;
                        var fieldType = null as string;
                        var fieldInit = null as ExpressionSyntax;
                        Next();
                        if (Type == TokenType.Colon)
                        {
                            Next();
                            if (Type != TokenType.Name) AddError("Field name expected.");
                            else
                            {
                                fieldType = StringValue;
                                Next();
                            }
                        }
                        if (Type == TokenType.Assign)
                        {
                            Next();
                            fieldInit = ParseExpression();
                        }
                        fields.Add((fieldName, fieldType, fieldInit));
                        break;
                    case TokenType.Function:
                        Next();
                        switch (Type)
                        {
                            case TokenType.Colon:
                                Next();
                                if (Type != TokenType.Name)
                                {
                                    AddError("Expected member name after 'function :' in class definition; ignoring this member.");
                                    continue;
                                }
                                var member = StringValue;
                                Next();
                                var body = ParseFunctionBody();
                                if (body != null) members.Add((member, body));
                                break;
                            case TokenType.Dot:
                                Next();
                                if (Type != TokenType.Name)
                                {
                                    AddError("Expected member name after 'function .' in class definition; ignoring this static member.");
                                    continue;
                                }
                                var staticMember = StringValue;
                                Next();
                                var staticBody = ParseFunctionBody();
                                if (staticBody != null) staticMembers.Add((staticMember, staticBody));
                                break;
                            default:
                                AddError("Member function should start with ':' or '.' to specify if it's static; assuming it's a non-static member.");
                                var f = ParseLocalFunctionRest();
                                if (f != null) members.Add((f.Name, f.Function));
                                break;
                        }
                        break;
                    default:
                        AddError("Member should start with either name (for fields) or 'function' (for member functions).");
                        break;
                }
                switch (Type)
                {
                    case TokenType.Semicolon:
                    case TokenType.Comma:
                        continue;
                    case TokenType.End:
                        break;
                    default:
                        AddError("';' or ',' expected between class members.");
                        continue;
                }
                break;
            }

            Next(); // Eat 'end'

            return new ClassDefinitionSyntax(initLine, initColomn, name, bases.ToArray(), fields.ToArray(), members.ToArray(), staticMembers.ToArray());
        }

        private BlockSyntax ParseBlock()
        {
            var initLine = _line;
            var initColomn = _colomn;

            var statements = new List<StatementSyntax>();

            while (_statementFirst.Contains(Type))
                statements.AddIfNonNull(ParseStatement());

            var @return = null as ReturnSyntax;
            if (Type == TokenType.Return)
                @return = ParseReturn();

            return new BlockSyntax(initLine, initColomn, statements.ToArray(), @return);
        }

        private ReturnSyntax ParseReturn()
        {
            var initLine = _line;
            var initColomn = _colomn;

            var expressions = new List<ExpressionSyntax>();
            do
            {
                Next(); // Eat 'return' or ','
                expressions.AddIfNonNull(ParseExpression());
            } while (Type == TokenType.Comma);

            return new ReturnSyntax(initLine, initColomn, expressions.ToArray());
        }

        #region Statement Parsing
        private static readonly ISet<TokenType> _statementFirst = new HashSet<TokenType>()
        {
            TokenType.Semicolon,
            TokenType.Name,
            TokenType.LeftParen,
            TokenType.LabelMark,
            TokenType.Break,
            TokenType.Goto,
            TokenType.Do,
            TokenType.While,
            TokenType.Repeat,
            TokenType.If,
            TokenType.For,
            TokenType.Function,
            TokenType.Local
        };

        private StatementSyntax ParseStatement()
        {
            while (true)
                switch (Type)
                {
                    case TokenType.Semicolon:
                        // Ignore
                        continue;
                    case TokenType.LabelMark:
                        return ParseLabel();
                    case TokenType.Break:
                        return ParseBreak();
                    case TokenType.Goto:
                        return ParseGoto();
                    case TokenType.Do:
                        return ParseDoBlock();
                    case TokenType.While:
                        return ParseWhile();
                    case TokenType.Repeat:
                        return ParseRepeat();
                    case TokenType.If:
                        return ParseIf();
                    case TokenType.For:
                        return ParseFor();
                    case TokenType.Function:
                        return ParseNamedFunctionDef();
                    case TokenType.Local:
                        Next();
                        if (Type == TokenType.Function)
                        {
                            Next();
                            return ParseLocalFunctionRest();
                        }
                        else return ParseLocalDefinitionRest();
                    case TokenType.Name:
                    case TokenType.LeftParen:
                        var start = ParsePrefixExpression();
                        if (Type == TokenType.Assign || Type == TokenType.Comma)
                            return ParseAssignment(start);
                        if (start is FunctionCallExpression f)
                            return new FunctionCallStatement(f.Line, f.Colomn, f);
                        AddError("Illegal statement.");
                        return null;
                }
        }

        private AssignmentStatement ParseAssignment(ExpressionSyntax start)
        {
            var initLine = _line;
            var initColomn = _colomn;

            var variables = new List<VariableExpression>();
            while (Type == TokenType.Comma)
            {
                Next(); // Eat ','
                if (start is VariableExpression v)
                {
                    variables.Add(v);
                    start = ParsePrefixExpression();
                }
                else
                    AddError("Expected variable in left hand of an assignment statement.");
            }

            if (Type != TokenType.Assign)
            {
                AddError("Expected '=' after the variable list of an assignment statement.");
                return null;
            }

            var expressions = new List<ExpressionSyntax>();
            do
            {
                Next(); // Eat '=' or ','
                expressions.AddIfNonNull(ParseExpression());
            } while (Type == TokenType.Comma);

            return new AssignmentStatement(initLine, initColomn, variables.ToArray(), expressions.ToArray());
        }

        private BreakStatement ParseBreak()
        {
            var breakStmt = new BreakStatement(_line, _colomn);
            Next();
            return breakStmt;
        }

        private DoBlockStatement ParseDoBlock()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'do'

            var body = ParseBlock();

            if (Type != TokenType.End) AddError("Expected 'end' at end of do block; assuming you forgot to write it.");
            else Next();

            return new DoBlockStatement(initLine, initColomn, body);
        }

        private ForStatement ParseFor()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'for'

            if (Type != TokenType.Name)
            {
                AddError("Expected variable name after 'for' in a for loop statement.");
                return null;
            }

            var first = StringValue;
            Next();

            if (Type == TokenType.Assign)
            {
                var start = ParseExpression();
                if (Type != TokenType.Comma) AddError("Expected ',' after start expression in for loop.");
                else Next();

                var end = ParseExpression();

                var step = null as ExpressionSyntax;
                if (Type == TokenType.Comma)
                {
                    Next();
                    step = ParseExpression();
                }
                return new ForStepStatement(initLine, initColomn, first, start, end, step);
            }
            else if (Type == TokenType.Comma || Type == TokenType.In)
            {
                var names = new List<string>() { first };
                while (Type != TokenType.In)
                {
                    if (Type != TokenType.Comma) AddError("Expected ',' after a variable name in the loop variable list of for loop.");
                    else Next();
                    if (Type != TokenType.Name)
                    {
                        AddError("Expected variable name in the loop variable list of for loop.");
                        return null;
                    }
                    names.Add(StringValue);
                }
                Next();
                var iterator = new List<ExpressionSyntax>();
                ExpressionSyntax e;
                while ((e = ParseExpression()) != null)
                {
                    iterator.Add(e);
                    if (Type != TokenType.Comma) break;
                    else Next();
                }
                if (Type != TokenType.Do)
                {
                    AddError("Illegal for by iterator loop.");
                    return null;
                }
                return new ForIteratorStatement(initLine, initColomn, names.ToArray(), iterator.ToArray());
            }
            else
            {
                AddError("Unrecognized for loop.");
                return null;
            }
        }

        private GotoStatement ParseGoto()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'goto'

            if (Type != TokenType.Name)
            {
                AddError("Name required after 'goto'.");
                return null;
            }

            var label = new GotoStatement(initLine, initColomn, StringValue);
            Next();

            return label;
        }

        private IfStatement ParseIf()
        {
            var initLine = _line;
            var initColomn = _colomn;

            var conditions = new List<(ExpressionSyntax, BlockSyntax)>();

            do
            {
                Next(); // Eat 'if' or 'elseif'
                var condExpr = ParseExpression();
                if (Type != TokenType.Then)
                    AddError("Expected 'then' after condition expression in if statement.");
                var condBody = ParseBlock();
                conditions.Add((condExpr, condBody));
            } while (Type == TokenType.Elseif);

            var elseBody = null as BlockSyntax;

            if (Type == TokenType.Else)
                elseBody = ParseBlock();

            if (Type != TokenType.End)
                AddError("Expected 'end' after if statement.");

            return new IfStatement(initLine, initColomn, conditions.ToArray(), elseBody);
        }

        private LabelStatement ParseLabel()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat the first '::'

            if (Type != TokenType.Name)
            {
                AddError("Name required after label mark '::'.");
                return null;
            }

            var label = new LabelStatement(initLine, initColomn, StringValue);
            Next();

            if (Type != TokenType.LabelMark) AddError("Label mark '::' required after label name; assuming you forgot to write it.");
            else Next(); // Eat the second '::'

            return label;
        }

        private LocalDefinitionStatement ParseLocalDefinitionRest()
        {
            var initLine = _line;
            var initColomn = _colomn;

            var variables = new List<(string, string)>();
            do
            {
                Next(); // Eat ','

                if (Type != TokenType.Name)
                {
                    AddError("Variable name expected.");
                    return null;
                }
                var name = StringValue;
                Next();

                var type = null as string;
                if (Type == TokenType.Colon)
                {
                    Next();
                    if (Type != TokenType.Name)
                    {
                        AddError("Type name expected.");
                        return null;
                    }
                    type = StringValue;
                    Next();
                }

                variables.Add((name, type));
            } while (Type == TokenType.Comma);

            var expressions = new List<ExpressionSyntax>();

            if (Type == TokenType.Assign)
                do
                {
                    Next(); // Eat '=' or ','
                    expressions.AddIfNonNull(ParseExpression());
                } while (Type == TokenType.Comma);

            return new LocalDefinitionStatement(initLine, initColomn, variables.ToArray(), expressions.ToArray());
        }

        private LocalFunctionDefinitionStatement ParseLocalFunctionRest()
        {
            var initLine = _line;
            var initColomn = _colomn;

            if (Type != TokenType.Name)
            {
                AddError("Function name expected.");
                return null;
            }
            var name = StringValue;
            Next();
            return new LocalFunctionDefinitionStatement(initLine, initColomn, name, ParseFunctionBody());
        }

        private FunctionDefinitionStatement ParseNamedFunctionDef()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'function'

            var referenceChain = new List<string>();
            var memberName = null as string;
            while (Type == TokenType.Name)
            {
                referenceChain.Add(StringValue);
                if (Type == TokenType.Dot) Next();
                else if (Type == TokenType.Colon)
                {
                    Next();
                    if (Type != TokenType.Name) AddError("Expected member name after ':' in function name.");
                    else
                    {
                        memberName = StringValue;
                        Next();
                    }
                    continue;
                }
                else
                {
                    AddError("Expected '.' or ':' in named function definition between referecing names.");
                    return null;
                }
            }

            var function = ParseFunctionBody();

            return new FunctionDefinitionStatement(initLine, initColomn, referenceChain.ToArray(), memberName, function);
        }

        private RepeatLoopStatement ParseRepeat()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'repeat'

            var body = ParseBlock();

            if (Type != TokenType.Until) AddError("Expected 'until' at end of a repeat block; assuming you forgot to write it.");
            else Next();

            var condition = ParseExpression();

            if (condition is null)
                AddError("Unrecognized condition expression for repeat-until loop.");

            return new RepeatLoopStatement(initLine, initColomn, body, condition);
        }

        private WhileLoopStatement ParseWhile()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'while'

            var condition = ParseExpression();

            if (Type != TokenType.Do) AddError("Expected 'do' after condition expression in a while loop; assuming you forgot to write it.");
            else Next();

            var body = ParseBlock();

            if (Type != TokenType.End) AddError("Expected 'end' at end of while loop; assuming you forgot to write it.");
            else Next();

            return new WhileLoopStatement(initLine, initColomn, condition, body);
        }
        #endregion

        #region Expression Parsing
        private Dictionary<TokenType, BinaryOperator> _binops = new Dictionary<TokenType, BinaryOperator>()
        {
            { TokenType.Add, BinaryOperator.Add },
            { TokenType.Minus, BinaryOperator.Minus },
            { TokenType.Multiply, BinaryOperator.Multiply },
            { TokenType.FloatDivide, BinaryOperator.FloatDivide },
            { TokenType.FloorDivide, BinaryOperator.FloorDivide },
            { TokenType.Exponent, BinaryOperator.Exponent },
            { TokenType.Modulo, BinaryOperator.Modulo },
            { TokenType.BitAnd, BinaryOperator.BitAnd },
            { TokenType.BitXorOrNot, BinaryOperator.BitXor },
            { TokenType.BitOr, BinaryOperator.BitOr },
            { TokenType.BitRShift, BinaryOperator.BitRShift },
            { TokenType.BitLShift, BinaryOperator.BitLShift },
            { TokenType.Concat, BinaryOperator.Concat },
            { TokenType.Less, BinaryOperator.Less },
            { TokenType.LessEqual, BinaryOperator.LessEqual },
            { TokenType.Greater, BinaryOperator.Greater },
            { TokenType.GreaterEqual, BinaryOperator.GreaterEqual },
            { TokenType.Equal, BinaryOperator.Equal },
            { TokenType.Inequal, BinaryOperator.Inequal },
            { TokenType.And, BinaryOperator.And },
            { TokenType.Or, BinaryOperator.Or }
        };

        private Dictionary<TokenType, UnaryOperator> _unops = new Dictionary<TokenType, UnaryOperator>()
        {
            { TokenType.Minus, UnaryOperator.Negative },
            { TokenType.Not, UnaryOperator.Not },
            { TokenType.Length, UnaryOperator.Length },
            { TokenType.BitXorOrNot, UnaryOperator.BitNot },
        };

        private ExpressionSyntax ParseExpression(int prec = -1)
        {
            var initLine = _line;
            var initColomn = _colomn;

            var lhs = null as ExpressionSyntax;
            if (_unops.ContainsKey(Type))
            {
                var op = _unops[Type];
                Next();
                lhs = new UnaryOperatorExpression(initLine, initColomn, op, ParseExpression(op.GetPrecedence()));
            }
            else lhs = ParseSimpleExpression();

            while (_binops.ContainsKey(Type) && _binops[Type].GetPrecedence() > prec)
            {
                var op = _binops[Type];
                Next();
                lhs = new BinaryOperatorExpression(initLine, initColomn, op, lhs, ParseExpression(op.GetSubExprPrecedence()));
            }

            return lhs;
        }

        private ExpressionSyntax ParseSimpleExpression()
        {
            switch (Type)
            {
                case TokenType.Nil:
                    return new LiteralExpressionSyntax(_line, _colomn, LiteralType.Nil);
                case TokenType.True:
                    return new LiteralExpressionSyntax<bool>(_line, _colomn, LiteralType.Boolean, true);
                case TokenType.False:
                    return new LiteralExpressionSyntax<bool>(_line, _colomn, LiteralType.Boolean, false);
                case TokenType.IntegerLiteral:
                    return new LiteralExpressionSyntax<int>(_line, _colomn, LiteralType.Integer, IntegerValue);
                case TokenType.FloatLiteral:
                    return new LiteralExpressionSyntax<double>(_line, _colomn, LiteralType.Float, FloatValue);
                case TokenType.StringLiteral:
                    return new LiteralExpressionSyntax<string>(_line, _colomn, LiteralType.String, StringValue);
                case TokenType.VarArg:
                    return new VarArgExpression(_line, _colomn);
                case TokenType.Function:
                    Next();
                    return ParseFunctionBody();
                case TokenType.LeftBrace:
                    return ParseTableConstructorExpression();
                case TokenType.New:
                    return ParseNew();
                case TokenType.Name:
                case TokenType.LeftParen:
                    return ParsePrefixExpression();
                default:
                    AddError("Unexpected expression start.");
                    return null;
            }
        }

        private NewExpression ParseNew()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat 'new'

            if (Type != TokenType.Name)
            {
                AddError("Type name expected after 'new'.");
                return null;
            }

            var type = StringValue;
            Next();

            var args = ParseArguments();
            if (args is null) AddError("Arguments expected after type name in new expression.");

            return new NewExpression(initLine, initColomn, type, args);
        }

        private ExpressionSyntax ParsePrefixExpression()
        {
            var initLine = _line;
            var initColomn = _colomn;

            var current = null as ExpressionSyntax;
            switch (Type)
            {
                case TokenType.Name:
                    current = new StandaloneVariableExpression(initLine, initColomn, StringValue);
                    break;
                case TokenType.LeftParen:
                    Next();
                    current = ParseExpression();
                    break;
                default:
                    AddError("Unrecognized start of prefix expression.");
                    return null;
            }

            while (true)
            {
                switch (Type)
                {
                    case TokenType.LeftSquareBracket:
                        Next();
                        current = new IndexedVariableExpression(initLine, initColomn, current, ParseExpression());
                        if (Type != TokenType.RightSquareBracket) AddError("Square brackets not match.");
                        else Next();
                        continue;
                    case TokenType.Dot:
                        Next();
                        if (Type != TokenType.Name) AddError("Expected member name after '.'.");
                        else current = new FieldIndexedVariableExpression(initLine, initColomn, current, StringValue);
                        Next();
                        continue;
                    case TokenType.LeftParen:
                    case TokenType.StringLiteral:
                    case TokenType.LeftBrace:
                        current = new FunctionCallExpression(initLine, initColomn, current, ParseArguments());
                        continue;
                    case TokenType.Colon:
                        continue;
                }
                break;
            }

            return current;
        }

        private ExpressionSyntax[] ParseArguments()
        {
            switch (Type)
            {
                case TokenType.LeftParen:
                    var args = new List<ExpressionSyntax>();
                    do
                    {
                        Next(); // Eat '(' or ','
                        args.AddIfNonNull(ParseExpression());
                    } while (Type == TokenType.Comma);
                    if (Type != TokenType.RightParen) AddError("Parentheses not match.");
                    return args.ToArray();
                case TokenType.StringLiteral:
                    var s = new ExpressionSyntax[] { new LiteralExpressionSyntax<string>(_line, _colomn, LiteralType.String, StringValue) };
                    Next();
                    return s;
                case TokenType.LeftBrace:
                    var t = new ExpressionSyntax[] { ParseTableConstructorExpression() };
                    Next();
                    return t;
                default:
                    return null;
            }
        }

        private FunctionDefinitionExpression ParseFunctionBody()
        {
            var initLine = _line;
            var initColomn = _colomn;

            if (Type != TokenType.LeftParen)
            {
                AddError("Expected '(' after function name.");
                return null;
            }
            else Next();

            var parameters = new List<(string, string)>();
            while (Type == TokenType.Name)
            {
                var name = StringValue;
                var type = null as string;
                Next();
                if (Type == TokenType.Colon)
                {
                    Next();
                    if (Type != TokenType.Name) AddError("Expected type name after ':' in function parameters.");
                    else
                    {
                        type = StringValue;
                        Next();
                    }
                }
                parameters.Add((name, type));
                if (Type == TokenType.Comma)
                    Next();
                else if (Type == TokenType.Assign)
                    AddError("Default value in parameter list not supported.");
                else break;
            }

            var hasVarArgs = false;
            if (Type == TokenType.VarArg)
            {
                hasVarArgs = true;
                Next();
            }

            if (Type != TokenType.RightParen) AddError("Expected ')' after parameter list; assuming you forgot to write it.");
            else Next();

            var body = ParseBlock();

            if (Type != TokenType.End) AddError("Expected 'end' at end of while loop; assuming you forgot to write it.");
            else Next();

            return new FunctionDefinitionExpression(initLine, initColomn, parameters.ToArray(), hasVarArgs, body);
        }

        private TableConstructorExpression ParseTableConstructorExpression()
        {
            var initLine = _line;
            var initColomn = _colomn;

            Next(); // Eat '{'

            var table = new List<(ExpressionSyntax, ExpressionSyntax)>();
            int count = 1;
            while (true)
            {
                var key = null as ExpressionSyntax;
                switch (Type)
                {
                    case TokenType.LeftSquareBracket:
                        Next();
                        key = ParseExpression();
                        if (Type != TokenType.RightSquareBracket) AddError("Square brackets not match.");
                        else Next();
                        if (Type != TokenType.Assign) AddError("'=' expected after key in table constructor.");
                        else Next();
                        break;
                    case TokenType.Name:
                        key = new LiteralExpressionSyntax<string>(_line, _colomn, LiteralType.String, StringValue);
                        if (Type != TokenType.Assign) AddError("'=' expected after key in table constructor.");
                        else Next();
                        break;
                    default:
                        key = new LiteralExpressionSyntax<int>(_line, _colomn, LiteralType.Integer, count++);
                        break;
                }

                var value = ParseExpression();

                table.Add((key, value));

                switch (Type)
                {
                    case TokenType.Comma:
                    case TokenType.Semicolon:
                        continue;
                    case TokenType.RightBrace:
                        break;
                    default:
                        AddError("Unexpected token.");
                        return null;
                }
                break;
            }

            return new TableConstructorExpression(initLine, initColomn, table.ToArray());
        }
        #endregion
    }
}
