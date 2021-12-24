using PGCompiler.Compiler.Errors;
using PGCompiler.Compiler.Nodes;
using PGCompiler.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PGCompiler.Compiler.Types
{
    internal abstract class SemanticBaseVisitor : IErrorTrackingVisitor
    {
        public ICollection<CompilerError> Errors { get; internal set; }

        internal SemanticBaseVisitor(ICollection<CompilerError> errors)
        {
            Errors = errors;
        }

        public abstract void Visit(Node node);
    }

    internal class PackageVisitor : SemanticBaseVisitor
    {
        public Package Package { get; internal set; }

        internal PackageVisitor(ICollection<CompilerError> errors) : base(errors)
        {
        }

        public override void Visit(Node node)
        {
            var package = node.Children.SingleOrDefault(t => t.NodeType.Equals(NodeType.Identifier));
            Package = new Package(package?.NodeValue);
        }
    }

    internal class ImportVisitor : SemanticBaseVisitor
    {
        public Import Import { get; internal set; }

        internal ImportVisitor(ICollection<CompilerError> errors) : base(errors)
        {
        }

        public override void Visit(Node node)
        {
            var clas = node.Children.SingleOrDefault(t => t.NodeType.Equals(NodeType.StringLiteral));
            Import = new Import(clas?.NodeValue);
        }
    }

    

    internal class EnumFieldVisitor : SemanticBaseVisitor
    {
        public EnumField EnumField { get; internal set; }

        internal EnumFieldVisitor(ICollection<CompilerError> errors) : base(errors)
        {
        }

        public override void Visit(Node node)
        {
            var name = node.Children.Single(t => t.NodeType.Equals(NodeType.Identifier)).NodeValue;
            var value = node.Children.Single(t => t.NodeType.Equals(NodeType.FieldNumber)).NodeValue;
            var number = int.Parse(value);
            EnumField = new EnumField(name, number);
        }
    }
}