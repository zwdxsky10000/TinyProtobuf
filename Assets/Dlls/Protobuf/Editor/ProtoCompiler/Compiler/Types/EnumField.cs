using PGCompiler.Extensions;
using System;

namespace PGCompiler.Compiler.Types
{
    public class EnumField : IEquatable<EnumField>
    {
        public string Name { get; }
        public int FieldNumber { get; }

        internal EnumField(string name, int fieldNum)
        {
            Name = Check.NotNull(name, nameof(name));
            FieldNumber = fieldNum;
        }

        public bool Equals(EnumField other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                   FieldNumber.Equals(other.FieldNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as EnumField);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + FieldNumber.GetHashCode();
            return hash;
        }
    }
}