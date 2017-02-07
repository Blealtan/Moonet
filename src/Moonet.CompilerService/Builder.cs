using Moonet.CompilerService.CodeGen;
using Moonet.CompilerService.Semantic;
using Moonet.CompilerService.Syntax;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Moonet.CompilerService
{
    public class Environment
    {
        private ICodeProvider _codeProvider;

        private INameProvider _nameProvider;

        private string _prefix;

        private Dictionary<string, SyntaxTree> _parsed = new Dictionary<string, SyntaxTree>();

        private Dictionary<string, ModuleBuilder> _used = new Dictionary<string, ModuleBuilder>();

        private Dictionary<string, MethodInfo> _built = new Dictionary<string, MethodInfo>();

        private Dictionary<string, TypeInfo> _classes = new Dictionary<string, TypeInfo>();

        public Dictionary<string, TypeInfo> Classes => _classes;

        public Environment(ICodeProvider codeProvider, INameProvider nameProvider, string prefix = "Moonet.Execute.")
        {
            _codeProvider = codeProvider;
            _nameProvider = nameProvider;
            _prefix = prefix;
        }

        public bool TryParse(string referenceName, out SyntaxTree syntax, out Queue<Error> errors)
        {
            if (_parsed.ContainsKey(referenceName))
            {
                syntax = _parsed[referenceName];
                errors = null;
                return true;
            }

            var input = _codeProvider.GetCode(referenceName);

            var parser = new Parser.Parser(input);
            var syntaxTree = parser.Parse();

            if (parser.ErrorQueue.Count != 0)
            {
                syntax = null;
                errors = parser.ErrorQueue;
                return false;
            }

            syntax = syntaxTree;
            errors = null;

            _parsed[referenceName] = syntax;
            return true;
        }

        public bool TryUse(string referenceName, out ModuleBuilder module, out Queue<Error> errors)
        {
            if (_used.ContainsKey(referenceName))
            {
                module = _used[referenceName];
                errors = null;
                return true;
            }

            if (!TryParse(referenceName, out SyntaxTree syntax, out errors))
            {
                module = null;
                return false;
            }

            errors = new Queue<Error>();
            var classes = new List<Class>();
            foreach (var c in syntax.Classes)
            {
                var semaClass = new Class(c, errors);
                if (errors.Count == 0) classes.Add(semaClass);
                else
                {
                    module = null;
                    return false;
                }
            }
            errors = null;

            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(_prefix + referenceName), AssemblyBuilderAccess.Run);
            module = assembly.DefineDynamicModule(_prefix + referenceName + ".dll");
            foreach (var c in classes)
            {
                var tb = module.DefineType(_prefix + c.Name);
                c.CodeGen(tb);
                _classes[c.Name] = tb.CreateTypeInfo();
            }

            _used[referenceName] = module;
            return true;
        }

        public bool TryBuild(string referenceName, out MethodInfo root, out Queue<Error> errors)
        {
            if (_built.ContainsKey(referenceName))
            {
                root = _built[referenceName];
                errors = null;
                return true;
            }

            if (!TryParse(referenceName, out SyntaxTree syntax, out errors))
            {
                root = null;
                return false;
            }

            foreach (var u in syntax.Usings)
                if (u is UsingFileSyntax uf)
                    if (!TryUse(uf.UsingFileName, out ModuleBuilder ignored, out errors))
                    {
                        root = null;
                        return false;
                    }

            if (!TryUse(referenceName, out ModuleBuilder module, out errors))
            {
                root = null;
                return false;
            }

            errors = new Queue<Error>();
            var body = new Block(_parsed[referenceName].Body, errors);
            if (errors.Count != 0)
            {
                root = null;
                return false;
            }
            errors = null;

            var tb = module.DefineType(_prefix + referenceName + "Root<>");
            var mb = tb.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static);
            body.CodeGen(mb);

            _built[referenceName] = root = tb.CreateTypeInfo().GetDeclaredMethod("Main");
            return true;
        }
    }
}
