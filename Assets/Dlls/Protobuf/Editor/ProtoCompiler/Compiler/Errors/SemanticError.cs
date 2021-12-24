using System;

namespace PGCompiler.Compiler.Errors
{
    public class SemanticError : CompilerError, IEquatable<SemanticError>
    {
        internal SemanticError(string message) : base(message)
        {
        }

        public override string ToString()
        {
            return Message;
        }

        public bool Equals(SemanticError other)
        {
            return other != null && Message.Equals(other.Message, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as SemanticError);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + Message.GetHashCode();
            return hash;
        }
    }
}