using Moonet.CompilerService.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private int _line, _colomn;

        private Token _current;

        private TokenType Type { get => _current.Type; }

        private int IntegerValue { get => (_current as Token<int>).Value; }

        private double FloatValue { get => (_current as Token<double>).Value; }

        private string StringValue { get => (_current as Token<string>).Value; }

        private Lexer _lexer;

        private void Next()
        {
            (_line, _colomn, _current) = _lexer.AnalyzeNextToken();
        }

        public Queue<Error> ErrorQueue { get; }

        private readonly int _maxErrors;

        private void AddError(string message)
        {
            ErrorQueue.Enqueue(new Error(_line, _colomn, _lexer.CurrentLine, message));
        }

        public Parser(TextReader src, int maxErrors)
        {
            ErrorQueue = new Queue<Error>();
            _lexer = new Lexer(src, ErrorQueue);
            _maxErrors = maxErrors;
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

        private ICollection<UsingSyntax> ParseUsing()
        {
            var usings = new List<UsingSyntax>();
            while (Type == TokenType.Using)
            {
                if (ErrorQueue.Count >= _maxErrors) return null;
                int line = _line, colomn = _colomn;
                Next();
                switch (Type)
                {
                    case TokenType.StringLiteral:
                        var file = (_current as Token<string>).Value;
                        Next();
                        string asVar = null;
                        if (Type == TokenType.As)
                        {
                            Next();
                            if (Type != TokenType.Name)
                                AddError("Expected variable name after 'as' in using statement.");
                            else
                            {
                                asVar = StringValue;
                                Next();
                            }
                        }
                        usings.Add(new UsingFileSyntax(line, colomn, file, asVar));
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
            return usings;
        }

        private (BlockSyntax body, ICollection<ClassDefinitionSyntax> classes) ParseBody()
        {
            var initLine = _line;
            var initColomn = _colomn;

            var statements = new List<StatementSyntax>();
            var classes = new List<ClassDefinitionSyntax>();

            while (Type != TokenType.EndOfFile)
            {
                if (Type == TokenType.Class)
                    classes.AddIfNonNull(ParseClass());
                else
                    statements.AddIfNonNull(ParseStatement());
            }

            return (new BlockSyntax(initLine, initColomn, statements), classes);
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
            var fields = new List<LocalDefinitionStatementSyntax>();
            var members = new Dictionary<string, FunctionDefinitionExpressionSyntax>();
            var staticMembers = new Dictionary<string, FunctionDefinitionExpressionSyntax>();
            while (Type != TokenType.End)
            {
                switch (Type)
                {
                    case TokenType.Name:
                        fields.AddIfNonNull(ParseLocalDefinitionRest());
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
                                if (body != null) members.Add(member, body);
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
                                if (staticBody != null) staticMembers.Add(staticMember, staticBody);
                                break;
                            default:
                                AddError("Member function should start with ':' or '.' to specify if it's static; assuming it's a non-static member.");
                                var f = ParseLocalFunctionRest();
                                if (f != null) members.Add(f.Name, f.Function);
                                break;
                        }
                        break;
                    default:
                        AddError("Member should start with either name (for fields) or 'function' (for member functions).");
                        break;
                }
            }

            Next(); // Eat 'end'

            return new ClassDefinitionSyntax(initLine, initColomn, name, bases, fields, members, staticMembers);
        }

        private LocalFunctionDefinitionSyntax ParseLocalFunctionRest()
        {
            throw new NotImplementedException();
        }

        private FunctionDefinitionExpressionSyntax ParseFunctionBody()
        {
            throw new NotImplementedException();
        }

        private LocalDefinitionStatementSyntax ParseLocalDefinitionRest()
        {
            throw new NotImplementedException();
        }

        private StatementSyntax ParseStatement()
        {
            throw new NotImplementedException();
        }
    }
}
