using PGCompiler.Extensions;
using System;

namespace PGCompiler.Compiler.Types
{
    public class Field : IEquatable<Field>
    {
        public string FieldName { get; }
        public SimpleType SimpleType { get; }
        public string UserType { get; }
        public int FieldNumber { get; }
        public bool Repeated { get; }
        public bool IsUsertype { get; }

        internal Field(string type, string name, int fieldNum, bool isRepeated)
        {
            type = type.ToFirstUpper();
            FieldName = name;
            FieldNumber = fieldNum;
            Repeated = isRepeated;
            IsUsertype = !Enum.IsDefined(typeof(SimpleType), type);
            SimpleType = IsUsertype ? SimpleType.None : (SimpleType)Enum.Parse(typeof(SimpleType), type);
            UserType = IsUsertype ? type : string.Empty;
        }

        public string GetTypeDesc()
        {
            string desc = string.Empty;
            if(IsUsertype)
            {
                desc = UserType;
            }
            else
            {
                switch(SimpleType)
                {
                    case SimpleType.Bool:
                        desc = "bool";
                        break;
                    case SimpleType.Int32:
                        desc = "int";
                        break;
                    case SimpleType.Int64:
                        desc = "long";
                        break;
                    case SimpleType.Uint32:
                        desc = "uint";
                        break;
                    case SimpleType.Uint64:
                        desc = "ulong";
                        break;
                    case SimpleType.Float:
                        desc = "float";
                        break;
                    case SimpleType.Double:
                        desc = "double";
                        break;
                    case SimpleType.String:
                        desc = "string";
                        break;
                    case SimpleType.Bytes:
                        desc = "byte[]";
                        break;
                }
            }
            return desc;
        }

        public bool Equals(Field other)
        {
            if (other == null) return false;
            return FieldName.Equals(other.FieldName, StringComparison.OrdinalIgnoreCase) &&
                   SimpleType.Equals(other.SimpleType) &&
                   FieldNumber.Equals(other.FieldNumber) &&
                   Repeated.Equals(other.Repeated) &&
                   UserType.Equals(other.UserType, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals(obj as Field);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + FieldName.GetHashCode();
            hash = (hash * 7) + SimpleType.GetHashCode();
            hash = (hash * 7) + UserType.GetHashCode();
            hash = (hash * 7) + FieldNumber.GetHashCode();
            hash = (hash * 7) + Repeated.GetHashCode();
            return hash;
        }
    }

    public enum SimpleType
    {
        None,
        Double,
        Float,
        Int32,
        Int64,
        Uint32,
        Uint64,
        Bool,
        String,
        Bytes
    }
}