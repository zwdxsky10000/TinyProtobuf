using System;

namespace PGCompiler.Compiler.Errors
{
    public class ParseError : CompilerError, IEquatable<ParseError>
    {
        internal Token Token { get; }

        internal ParseError(string message, Token token = null) : base(message)
        {
            Token = token;
        }

        public override string ToString()
        {
            return Token == null ? $"{Message}" : $"{Message} : @ {Token}";
        }

        public bool Equals(ParseError other)
        {
            if (other == null) return false;
            return Token.Equals(other.Token) && Message.Equals(other.Message, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as ParseError);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + Token.GetHashCode();
            hash = (hash * 7) + Message.GetHashCode();
            return hash;
        }
    }
}