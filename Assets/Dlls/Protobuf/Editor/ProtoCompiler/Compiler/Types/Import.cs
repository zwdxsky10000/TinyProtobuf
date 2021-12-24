using System;

namespace PGCompiler.Compiler.Types
{
    public class Import : IEquatable<Import>
    {
        public string ImportClass { get; }

        internal Import(string clas)
        {
            ImportClass = clas;
        }

        public bool Equals(Import other)
        {
            if (other == null) return false;
            return ImportClass.Equals(other.ImportClass, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as Import);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + ImportClass.GetHashCode();
            return hash;
        }
    }
}