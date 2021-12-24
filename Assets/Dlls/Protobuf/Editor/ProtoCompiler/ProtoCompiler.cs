using PGCompiler.Compiler;
using PGCompiler.Compiler.Types;
using PGCompiler.Interfaces;
using System.IO;

namespace PGCompiler
{
    public class ProtoCompiler : IProtoCompiler
    {
        public Compilation Compile(string filePath)
        {
            var streamReader = File.OpenText(filePath);
            var compileSource = new Source(streamReader);
            var lexicalAnalyzer = new LexicalAnalyzer(compileSource);

            lexicalAnalyzer.Tokenize();

            var syntaxAnalyzer = new RootSyntaxAnalyzer(lexicalAnalyzer.TokenStream);

            var rootNode = syntaxAnalyzer.Analyze();

            var builderVisitor = new BuilderVisitor();
            builderVisitor.Visit(rootNode);

            return new Compilation
            {
                FileDescriptor = builderVisitor.FileDescriptor,
                Errors = rootNode.Errors
            };
        }
    }
}