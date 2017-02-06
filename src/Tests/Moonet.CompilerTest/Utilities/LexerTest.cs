using System.Collections.Generic;
using System.IO;
using Moonet.CompilerService;
using Moonet.CompilerService.Parser;
using Xunit;

namespace Moonet.CompilerTest.Utilities
{
    internal static class LexerTest
    {
        public static void TestTokenStream(TextReader input, IEnumerable<Token> tokens)
        {
            var errors = new Queue<Error>();
            var lexer = new Lexer(input, errors);
            foreach (var token in tokens)
                Assert.Equal(token, lexer.AnalyzeNextToken().token);
            Assert.True(errors.Count == 0);
        }
    }
}
