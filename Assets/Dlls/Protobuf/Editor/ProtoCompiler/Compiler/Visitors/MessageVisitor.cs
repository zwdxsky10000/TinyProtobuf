using PGCompiler.Compiler.Errors;
using PGCompiler.Compiler.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace PGCompiler.Compiler.Types
{
    internal class MessageVisitor : SemanticBaseVisitor
    {
        public MessageDefinition Message { get; internal set; }

        internal MessageVisitor(ICollection<CompilerError> errors) : base(errors)
        {
        }

        public override void Visit(Node node)
        {
            var name = node.Children.Single(t => t.NodeType.Equals(NodeType.Identifier)).NodeValue;

            string parent = string.Empty;
            var parentNode = node.Children.SingleOrDefault(t => t.NodeType.Equals(NodeType.Parent));
            if(parentNode != null)
            {
                parent = parentNode.NodeValue;
            }

            var fieldNodes = node.Children.Where(t => t.NodeType.Equals(NodeType.Field));
            var fields = new List<Field>();
            foreach (var field in fieldNodes)
            {
                var fieldVisitor = new FieldVisitor(Errors);
                field.Accept(fieldVisitor);
                fields.Add(fieldVisitor.Field);
            }

            Message = new MessageDefinition(name, parent, fields);
        }
    }

    internal class FieldVisitor : SemanticBaseVisitor
    {
        public Field Field { get; internal set; }

        public FieldVisitor(ICollection<CompilerError> errors) : base(errors)
        {
        }

        public override void Visit(Node node)
        {
            var isRepeated = !ReferenceEquals(null, node.Children.SingleOrDefault(t => t.NodeType.Equals(NodeType.Repeated)));
            var type = node.Children.Single(t => t.NodeType.Equals(NodeType.Type) || t.NodeType.Equals(NodeType.UserType)).NodeValue;
            var name = node.Children.Single(t => t.NodeType.Equals(NodeType.Identifier)).NodeValue;
            var value = node.Children.Single(t => t.NodeType.Equals(NodeType.FieldNumber)).NodeValue;
            var number = int.Parse(value);
            Field = new Field(type, name, number, isRepeated);
        }
    }
}