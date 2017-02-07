using Moonet.CompilerService.Semantic;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Moonet.CompilerService
{
    public class Builder
    {
        private ICodeProvider _codeProvider;

        private INameProvider _nameProvider;

        private string _prefix;

        private Dictionary<string, MethodInfo> _builtReference = new Dictionary<string, MethodInfo>();

        public Builder(ICodeProvider codeProvider, INameProvider nameProvider, string prefix = "Moonet.Execute.")
        {
            _codeProvider = codeProvider;
            _nameProvider = nameProvider;
            _prefix = prefix;
        }

        public bool TryBuild(string referenceName, out AssemblyBuilder assembly, out MethodInfo rootMethod, out Queue<Error> errors)
        {
            if (_builtReference.ContainsKey(referenceName))
            {
                assembly = null;
                rootMethod = _builtReference[referenceName];
                errors = null;
                return true;
            }

            var input = _codeProvider.GetCode(referenceName);

            var parser = new Parser.Parser(input);
            var syntaxTree = parser.Parse();

            if (parser.ErrorQueue.Count != 0)
            {
                assembly = null;
                rootMethod = null;
                errors = parser.ErrorQueue;
                return false;
            }

            var semaErrors = new Queue<Error>();
            var sema = new SemanticTree(syntaxTree, semaErrors);

            if (semaErrors.Count != 0)
            {
                assembly = null;
                rootMethod = null;
                errors = semaErrors;
                return false;
            }

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(_prefix + referenceName), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(_prefix + referenceName + ".dll");
            
            assembly = assemblyBuilder;
            rootMethod = sema.CodeGen(moduleBuilder, this);
            errors = null;
            return true;
        }
    }
}
