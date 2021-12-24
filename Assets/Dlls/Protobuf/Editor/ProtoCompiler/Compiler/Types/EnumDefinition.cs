using PGCompiler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGCompiler.Compiler.Types
{
    public class EnumDefinition : IEquatable<EnumDefinition>
    {
        public string Name { get; }
        public IEnumerable<EnumField> EnumFields { get; }

        internal EnumDefinition(string name, IEnumerable<EnumField> fields)
        {
            Name = Check.NotNull(name, nameof(name));
            EnumFields = fields ?? new List<EnumField>();
        }

        public bool Equals(EnumDefinition other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                   EnumFields.SequenceEqual(other.EnumFields);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as EnumDefinition);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + EnumFields.GetHashCode();
            return hash;
        }
    }
}