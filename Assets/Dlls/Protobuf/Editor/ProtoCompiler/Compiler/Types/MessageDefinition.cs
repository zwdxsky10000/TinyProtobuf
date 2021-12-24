using System;
using System.Collections.Generic;
using System.Linq;

namespace PGCompiler.Compiler.Types
{
    public class MessageDefinition : IEquatable<MessageDefinition>
    {
        public string Name { get; }

        public string ParentName { get; }

        public ICollection<Field> Fields { get; }

        internal MessageDefinition(string name, string parent,
            ICollection<Field> fields)
        {
            Name = name;
            ParentName = parent ?? string.Empty;
            Fields = fields ?? new List<Field>();
        }

        public bool Equals(MessageDefinition other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                   ParentName.Equals(other.ParentName, StringComparison.OrdinalIgnoreCase) &&
                   Fields.SequenceEqual(other.Fields);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as MessageDefinition);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + ParentName.GetHashCode();
            hash = (hash * 7) + Fields.GetHashCode();
            return hash;
        }
    }
}