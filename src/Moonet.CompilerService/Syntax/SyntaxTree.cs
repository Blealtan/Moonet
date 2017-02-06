namespace Moonet.CompilerService.Syntax
{
    public class SyntaxTree
    {
        public readonly UsingSyntax[] Usings;

        public readonly BlockSyntax Body;

        public readonly ClassDefinitionSyntax[] Classes;

        public SyntaxTree(UsingSyntax[] usings,
            BlockSyntax body,
            ClassDefinitionSyntax[] classes)
        {
            Usings = usings;
            Body = body;
            Classes = classes;
        }
    }
}
