using PGCompiler.Compiler.Errors;
using PGCompiler.Compiler.Nodes;
using PGCompiler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGCompiler.Compiler.Types
{
    internal class BuilderVisitor : IVisitor
    {
        public FileDescriptor FileDescriptor { get; internal set; } = new FileDescriptor();

        public ICollection<CompilerError> Errors { get; internal set; } = new List<CompilerError>();

        internal BuilderVisitor()
        {
        }

        public void Visit(Node node)
        {
            if (!node.NodeType.Equals(NodeType.Root))
                throw new InvalidOperationException("Cannot use BuilderVisitor on non-root Node");

            var packageNode = node.Children.SingleOrDefault(t => t.NodeType.Equals(NodeType.Package));
            var packageVisitor = new PackageVisitor(Errors);
            packageNode?.Accept(packageVisitor);
            FileDescriptor.Package = packageVisitor.Package;

            var importNodes = node.Children.Where(t => t.NodeType.Equals(NodeType.Import));
            foreach (var import in importNodes)
            {
                var importVisitor = new ImportVisitor(Errors);
                import.Accept(importVisitor);
                FileDescriptor.Imports.Add(importVisitor.Import);
            }

            var messageNodes = node.Children.Where(t => t.NodeType.Equals(NodeType.Message));
            foreach (var message in messageNodes)
            {
                var messageVisitor = new MessageVisitor(Errors);
                message.Accept(messageVisitor);
                FileDescriptor.Messages.Add(messageVisitor.Message);
            }

            var enumNodes = node.Children.Where(t => t.NodeType.Equals(NodeType.Enum));
            foreach (var option in enumNodes)
            {
                var enumVisitor = new EnumVisitor(Errors);
                option.Accept(enumVisitor);
                FileDescriptor.Enumerations.Add(enumVisitor.EnumDefinition);
            }
        }
    }
}