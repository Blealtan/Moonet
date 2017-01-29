using Moonet.CompilerService.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moonet.CompilerService.Parser
{
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
            var tree = new SyntaxTree();
            // Start prediction
            Next();
            if (ParseUsing(tree.Usings) && ParseBody(tree.Body))
                return tree;
            else return null;
        }

        private bool ParseUsing(ICollection<UsingSyntax> usings)
        {
            while (Type == TokenType.Using)
            {
                if (ErrorQueue.Count >= _maxErrors) return false;
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
            return true;
        }

        private bool ParseBody(BlockSyntax body)
        {
            throw new NotImplementedException();
        }
    }
}
