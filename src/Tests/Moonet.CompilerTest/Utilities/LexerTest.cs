using System.Collections.Generic;
using System.IO;
using Moonet.CompilerService;
using Moonet.CompilerService.Parser;
using Xunit;
using System.Text;

namespace Moonet.CompilerTest.Utilities
{
    internal static class LexerTest
    {
        public static (IList<string> tokens, IList<string> errors) SerializeTokenStream(TextReader input)
        {
            var errorsQueue = new Queue<Error>();
            var lexer = new Lexer(input, errorsQueue);
            var tokens = new List<string>();
            while (!lexer.EndReached)
                tokens.Add(lexer.AnalyzeNextToken().token.ToString());
            var errors = new List<string>();
            foreach (var error in errorsQueue)
                errors.Add(error.ToString());
            return (tokens: tokens, errors: errors);
        }
    }
}
