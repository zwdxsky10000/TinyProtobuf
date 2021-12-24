using PGCompiler.Compiler.Errors;
using PGCompiler.Compiler.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace PGCompiler.Compiler.Types
{
    internal class EnumVisitor : SemanticBaseVisitor
    {
        public EnumDefinition EnumDefinition { get; internal set; }

        internal EnumVisitor(ICollection<CompilerError> errors) : base(errors)
        {
        }

        public override void Visit(Node node)
        {
            var name = node.Children.Single(t => t.NodeType.Equals(NodeType.Identifier)).NodeValue;
            var enumFieldNodes = node.Children.Where(t => t.NodeType.Equals(NodeType.EnumField));
            var enumFields = new List<EnumField>();
            foreach (var ef in enumFieldNodes)
            {
                var vis = new EnumFieldVisitor(Errors);
                ef.Accept(vis);
                enumFields.Add(vis.EnumField);
            }

            EnumDefinition = new EnumDefinition(name, enumFields);
        }
    }
}
