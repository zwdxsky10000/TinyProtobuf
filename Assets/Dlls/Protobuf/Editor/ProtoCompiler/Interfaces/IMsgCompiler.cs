using PGCompiler.Compiler;

namespace PGCompiler.Interfaces
{
    public interface IProtoCompiler
    {
        Compilation Compile(string filePath);
    }
}