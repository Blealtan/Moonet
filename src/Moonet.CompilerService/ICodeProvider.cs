using System.IO;

namespace Moonet.CompilerService
{
    public interface ICodeProvider
    {
        TextReader GetCode(string referenceName);
    }
}
