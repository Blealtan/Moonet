﻿using Moonet.CompilerService.Syntax;
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
            throw new NotImplementedException();
        }
    }
}
