using System;
using System.Collections.Generic;
using System.Text;

namespace Moonet.CompilerService
{
    public class Error
    {
        public int Line { get; }
        public int Colomn { get; }
        public string Message { get; }
        public Error(int line, int colomn, string message = "")
        {
            Line = line;
            Colomn = colomn;
            Message = message;
        }
    }
}
