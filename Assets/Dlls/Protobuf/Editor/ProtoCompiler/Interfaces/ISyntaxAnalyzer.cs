using PGCompiler.Compiler.Nodes;

namespace PGCompiler.Interfaces
{
    internal interface ISyntaxAnalyzer<T> where T: Node
    {
        T Analyze();
    }
}