using PGCompiler.Compiler.Errors;
using PGCompiler.Compiler.Nodes;
using System.Collections.Generic;

namespace PGCompiler.Interfaces
{
    public interface IVisitor
    {
        void Visit(Node node);
    }

    public interface IErrorTrackingVisitor : IVisitor
    {
        ICollection<CompilerError> Errors { get; }
    }
}