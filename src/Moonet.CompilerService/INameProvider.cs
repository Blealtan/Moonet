namespace Moonet.CompilerService
{
    public interface INameProvider
    {
        string GetReferenceName(string usingName);
    }
}