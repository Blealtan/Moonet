using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService
{
    public class Error
    {
        public int Line { get; }
        public int Colomn { get; }
        public string CurrentLine { get; }
        public string Message { get; }
        public Error(int line, int colomn, string currentLine, string message = "")
        {
            Line = line;
            Colomn = colomn;
            CurrentLine = currentLine;
            Message = message;
        }
    }
}
