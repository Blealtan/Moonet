using System.IO;
using Moonet.CompilerService.Parser;
using Moonet.CompilerTest.Utilities;
using Xunit;

namespace Moonet.CompilerTest.Parser.Lexer
{
    public class TokenStreamTest
    {
        [Fact]
        public void TokenStreamTest1()
        {
            Token[] tokens =
            {
                new Token(TokenType.Local),
                new Token(TokenType.Function),
                new Token<string>(TokenType.Name, "checkArgumentType"),
                new Token(TokenType.LeftParen),
                new Token<string>(TokenType.Name, "expected"),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "actual"),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "fn"),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "ud"),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "level"),
                new Token(TokenType.RightParen),
                new Token(TokenType.Local),
                new Token<string>(TokenType.Name, "level"),
                new Token(TokenType.Assign),
                new Token<string>(TokenType.Name, "level"),
                new Token(TokenType.Or),
                new Token<int>(TokenType.IntegerLiteral, 3),
                new Token(TokenType.If),
                new Token<string>(TokenType.Name, "expected"),
                new Token(TokenType.Inequal),
                new Token<string>(TokenType.Name, "actual"),
                new Token(TokenType.Then),
                new Token<string>(TokenType.Name, "checkArgument"),
                new Token(TokenType.LeftParen),
                new Token(TokenType.False),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "fn"),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "ud"),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "expected"),
                new Token(TokenType.Concat),
                new Token<string>(TokenType.StringLiteral, " expected, got "),
                new Token(TokenType.Concat),
                new Token<string>(TokenType.Name, "actual"),
                new Token(TokenType.Comma),
                new Token<string>(TokenType.Name, "level"),
                new Token(TokenType.Add),
                new Token<int>(TokenType.IntegerLiteral, 1),
                new Token(TokenType.RightParen),
                new Token(TokenType.End),
                new Token(TokenType.End),
                new Token(TokenType.EndOfFile)
            };
            string source =
@"local function checkArgumentType(expected, actual, fn, ud, level)
   local level = level or 3
   if expected ~= actual then
      checkArgument(false, fn, ud, expected .. "" expected, got "" .. actual, level + 1)
   end
end";
            LexerTest.TestTokenStream(new StringReader(source), tokens);
        }
    }
}
