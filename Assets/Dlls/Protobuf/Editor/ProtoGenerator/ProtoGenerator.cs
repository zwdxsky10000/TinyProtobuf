using PGCompiler.Compiler.Types;

namespace PGGenerator
{
    public static class ProtoGenerator
    {
        public static bool Generator(string path, FileDescriptor descriptor)
        {
            return GeneratorEngine.Generator(path, descriptor);
        }
    }
}


