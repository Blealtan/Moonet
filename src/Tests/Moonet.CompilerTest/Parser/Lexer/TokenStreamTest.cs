using System.IO;
using Moonet.CompilerTest.Utilities;
using Xunit;
using System.Collections.Generic;

namespace Moonet.CompilerTest.Parser.Lexer
{
    public class TokenStreamTest
    {
        [Fact]
        public void TokenStreamTest1()
        {
            string source =
@"local function checkArgumentType(expected, actual, fn, ud, level)
   local level = level or 3
   if expected ~= actual then
      checkArgument(false, fn, ud, expected .. "" expected, got "" .. actual, level + 1)
   end
end";
            var expectedTokens = new List<string>()
            {
                "Local",
                "Function",
                "Name:checkArgumentType",
                "LeftParen",
                "Name:expected",
                "Comma",
                "Name:actual",
                "Comma",
                "Name:fn",
                "Comma",
                "Name:ud",
                "Comma",
                "Name:level",
                "RightParen",
                "Local",
                "Name:level",
                "Assign",
                "Name:level",
                "Or",
                "IntegerLiteral:3",
                "If",
                "Name:expected",
                "Inequal",
                "Name:actual",
                "Then",
                "Name:checkArgument",
                "LeftParen",
                "False",
                "Comma",
                "Name:fn",
                "Comma",
                "Name:ud",
                "Comma",
                "Name:expected",
                "Concat",
                "StringLiteral: expected, got ",
                "Concat",
                "Name:actual",
                "Comma",
                "Name:level",
                "Add",
                "IntegerLiteral:1",
                "RightParen",
                "End",
                "End",
                "EndOfFile"
            };
            (var tokens, var errors) = LexerTest.SerializeTokenStream(new StringReader(source));
            Assert.Equal(expectedTokens as IEnumerable<string>, tokens);
            Assert.Equal(errors.Count, 0);
        }
    }
}
