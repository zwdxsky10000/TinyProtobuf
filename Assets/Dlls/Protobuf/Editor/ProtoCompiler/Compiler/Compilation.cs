using PGCompiler.Compiler.Errors;
using PGCompiler.Compiler.Types;
using System.Collections.Generic;

namespace PGCompiler.Compiler
{
    public class Compilation
    {
        public FileDescriptor FileDescriptor { get; set; }

        public ICollection<CompilerError> Errors { get; set; }
    }
}