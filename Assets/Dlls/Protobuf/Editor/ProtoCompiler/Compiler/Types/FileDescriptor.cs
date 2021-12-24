using System.Collections.Generic;

namespace PGCompiler.Compiler.Types
{
    public class FileDescriptor
    {
        public Package Package { get; set; }

        public ICollection<Import> Imports { get; set; } = new List<Import>();

        public ICollection<EnumDefinition> Enumerations { get; set; } = new List<EnumDefinition>();

        public ICollection<MessageDefinition> Messages { get; set; } = new List<MessageDefinition>();  

        public string ScriptName
        {
            get
            {
                string name = "Unknown.cs";
                if(Messages.Count > 0)
                {
                    foreach(var msg in Messages)
                    {
                        if(!string.IsNullOrEmpty(msg.Name))
                        {
                            name = string.Concat(msg.Name, ".cs");
                            break;
                        }
                    }
                }
                return name;
            }
        }

    }
}