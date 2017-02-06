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
            IEnumerable<string> expectedTokens = new List<string>()
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
            Assert.Equal(expectedTokens, tokens);
            Assert.Equal(errors.Count, 0);
        }

        [Fact]
        public void TokenStreamTest2()
        {
            string source =
@"-- Test comment
local a = [[
blabla
\\aaa]]
local b = [==[]=]
]=]]==]
--[[
Test block comment]]
--[=[]===]=]";
            IEnumerable<string> expectedTokens = new List<string>()
            {
                "Local",
                "Name:a",
                "Assign",
                "StringLiteral:blabla\n\\\\aaa",
                "Local",
                "Name:b",
                "Assign",
                "StringLiteral:]=]\n]=]",
                "EndOfFile"
            };
            (var tokens, var errors) = LexerTest.SerializeTokenStream(new StringReader(source));
            Assert.Equal(expectedTokens, tokens);
            Assert.Equal(errors.Count, 0);
        }

        [Fact]
        public void TokenStreamTest3()
        {
            string source =
@"          local escapedString = '\z
            Something? Something.\n\z
            Well, another problem.\
\u{4e2d}\u{6587}\u{6d4b}\u{8bd5}\u{3002}\n\z
            \97lo\10\04923'";
            IEnumerable<string> expectedTokens = new List<string>()
            {
                "Local",
                "Name:escapedString",
                "Assign",
                "StringLiteral:" +
                "Something? Something.\n" +
                "Well, another problem.\n" +
                "中文测试。\n" +
                "alo\n123",
                "EndOfFile"
            };
            (var tokens, var errors) = LexerTest.SerializeTokenStream(new StringReader(source));
            Assert.Equal(expectedTokens, tokens);
            Assert.Equal(errors.Count, 0);
        }
    }
}
