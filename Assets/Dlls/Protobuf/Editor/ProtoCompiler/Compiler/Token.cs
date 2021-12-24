using System;

namespace PGCompiler.Compiler
{
    internal enum TokenType
    {
        Id,
        Type,
        String,
        Numeric,
        Control,
        EndLine,
        Comment
    }

    internal class Token : IEquatable<Token>
    {
        internal TokenType Type { get; }
        internal int Column { get; }
        internal int Line { get; }
        internal string Lexeme { get; }

        public Token(TokenType type, int line, int column, string lexeme = "")
        {
            Type = type;
            Lexeme = lexeme;
            Line = line;
            Column = column;
        }

        public bool Equals(Token other)
        {
            if (other == null) return false;
            return Type.Equals(other.Type) &&
                   Column == other.Column &&
                   Line == other.Line &&
                   Lexeme.Equals(other.Lexeme, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as Token);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + Type.GetHashCode();
            hash = (hash * 7) + Column.GetHashCode();
            hash = (hash * 7) + Line.GetHashCode();
            hash = (hash * 7) + Lexeme.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"{Lexeme} L{Line}:C{Column}";
        }
    }
}