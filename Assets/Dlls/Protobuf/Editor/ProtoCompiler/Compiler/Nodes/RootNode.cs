using PGCompiler.Compiler.Errors;
using PGCompiler.Extensions;
using System.Collections.Generic;

namespace PGCompiler.Compiler.Nodes
{
    internal class RootNode : Node
    {
        internal IList<string> Imports { get; }
        internal IList<string> Exports { get; }
        internal ICollection<CompilerError> Errors { get; }

        internal RootNode() : base(NodeType.Root, string.Empty)
        {
            Imports = new List<string>();
            Exports = new List<string>();
            Errors = new List<CompilerError>();
        }

        internal void AddErrors(IEnumerable<CompilerError> errors)
        {
            Errors.AddRange(errors);
        }
    }
}