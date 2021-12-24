using PGCompiler.Compiler.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace PGGenerator
{
    public class GeneratorEngine
    {
        private static FileDescriptor s_TmpDescriptor;

        public static bool Generator(string path, FileDescriptor descriptor)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            try
            {
                s_TmpDescriptor = descriptor;

                IndentWriter writer = new IndentWriter(path);
                GenNamespaces(writer, descriptor.Imports);
                GenPackage(writer, descriptor);
                writer.Finish();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        static void GenNamespaces(IndentWriter writer, ICollection<Import> Imports)
        {
            writer.Write("/**************************************************************************************************");
            writer.Write("                                  自动生成的Protobuf代码  请勿手动修改");
            writer.Write("**************************************************************************************************/");
            writer.Empty();
            writer.Write("using Protobuf;");
            foreach(var ns in Imports)
            {
                writer.Write("using {0};", ns.ImportClass);
            }

            writer.Empty();
        }

        static void GenPackage(IndentWriter writer, FileDescriptor descriptor)
        {
            writer.Write("namespace {0}", descriptor.Package.Name);
            writer.Write("{");

            foreach (var definition in descriptor.Enumerations)
            {
                GenEnum(writer, definition);
            }

            foreach (var definition in descriptor.Messages)
            {
                GenClass(writer, definition);
            }

            writer.Write("}");
        }

        static void GenEnum(IndentWriter writer, EnumDefinition definition)
        {
            writer.Write("public enum {0}", definition.Name)
                  .Write("{");

            foreach (var field in definition.EnumFields)
            {
                writer.Write("{0} = {1},", field.Name, field.FieldNumber);
            }

            writer.Write("}")
                  .Empty();
        }

        static void GenClass(IndentWriter writer, MessageDefinition msgClass)
        {
            string parentName = msgClass.ParentName;
            if (string.IsNullOrEmpty(msgClass.ParentName))
            {
                parentName = "IMessage";
            }

            writer.Write("public partial class {0} : {1}", msgClass.Name, parentName)
                  .Write("{")
                  .Empty();

            GenFields(writer, msgClass.Fields);

            GenEncodeMethod(writer, msgClass.Fields);

            GenDecodeMethod(writer, msgClass.Fields);

            writer.Write("}")
                  .Empty();
        }

        #region Fields
        static void GenFields(IndentWriter writer, ICollection<Field> fields)
        {
            foreach(var field in fields)
            {
                GenOneField(writer, field);
            }
        }

        static void GenOneField(IndentWriter writer, Field field)
        {
            if (field.Repeated)
            {
                string listIdentifier = string.Format("System.Collections.Generic.List<{0}>", field.GetTypeDesc());
                writer.Write("private {0} _{1} = new {2}();", listIdentifier, field.FieldName, listIdentifier)
                      .Empty()
                      .Write("public {0} {1}", listIdentifier, field.FieldName)
                      .Write("{")
                          .Write("get")
                          .Write("{")
                              .Write("return _{0};", field.FieldName)
                          .Write("}")
                          .Empty()
                          .Write("set")
                          .Write("{")
                              .Write("_{0} = value;", field.FieldName)
                          .Write("}")
                      .Write("}")
                      .Empty();
            }
            else
            {
                string valueIdentifier = field.GetTypeDesc();
                string defaultVal = string.Format("default({0})", valueIdentifier);

                writer.Write("private {0} _{1} = {2};", valueIdentifier, field.FieldName, defaultVal)
                      .Empty()
                      .Write("public {0} {1}", valueIdentifier, field.FieldName)
                      .Write("{")
                          .Write("get")
                          .Write("{")
                              .Write("return _{0};", field.FieldName)
                          .Write("}")
                          .Empty()
                          .Write("set")
                          .Write("{")
                              .Write("_{0} = value;", field.FieldName)
                          .Write("}")
                      .Write("}")
                      .Empty();
            }
        }
        #endregion

        #region Encode
        static void GenEncodeMethod(IndentWriter writer, ICollection<Field> fields)
        {
            writer.Write("public void Encode(Protobuf.CodedOutputStream outStream)");
            writer.Write("{");

            foreach(var field in fields)
            {
                GenOneFieldEncode(writer, field);
            }

            writer.Write("}")
                  .Empty();
        }

        static void GenOneFieldEncode(IndentWriter writer, Field field)
        {
            if(field.IsUsertype)
            {
                if(IsEnumField(field.UserType))
                {
                    GenEnumFieldEncode(writer, field);
                }
                else
                {
                    GenCustomFieldEncode(writer, field);
                }
            }
            else
            {
                switch (field.SimpleType)
                {
                    case SimpleType.Int32:
                    case SimpleType.Int64:
                    case SimpleType.Uint32:
                    case SimpleType.Uint64:
                    case SimpleType.Float:
                    case SimpleType.Double:
                    case SimpleType.Bool:
                    case SimpleType.Bytes:
                        GenValueFieldEncode(writer, field);
                        break;
                    case SimpleType.String:
                        GenStringFieldEncode(writer, field);
                        break;
                    default:
                        break;
                }
            }
        }

        static void GenValueFieldEncode(IndentWriter writer, Field field)
        {
            uint tag = MakeTag(field);
            string desc = field.SimpleType.ToString();
            string name = field.FieldName;

            if (field.Repeated)
            {
                writer.Write("if ( {0} != null && {0}.Count >0 )", name)
                      .Write("{")
                          .Write("outStream.WriteTag({0});", tag)
                          .Write("outStream.BeginLengthRecord();")
                          .Write("for (int i = 0,len = {0}.Count; i < len; ++i)", name)
                          .Write("{")
                              .Write("outStream.Write{0}({1}[i]);", desc, name)
                          .Write("}")
                          .Write("outStream.EndLengthRecord();")
                      .Write("}")
                      .Empty();
            }
            else
            {
                writer.Write("if ({0} != default({1}))", name, field.GetTypeDesc())
                      .Write("{")
                          .Write("outStream.WriteTag({0});", tag)
                          .Write("outStream.Write{0}({1});", desc, name)
                      .Write("}")
                      .Empty();
            }
        }

        static void GenStringFieldEncode(IndentWriter writer, Field field)
        {
            uint tag = MakeTag(field);
            string name = field.FieldName;

            if (field.Repeated)
            {
                writer.Write("if ( {0} != null && {0}.Count >0 )", name)
                      .Write("{")
                          .Write("for (int i = 0,len = {0}.Count; i < len; ++i)", name)
                          .Write("{")
                              .Write("outStream.WriteTag({0});", tag)
                              .Write("outStream.WriteString({0}[i]);", name)
                          .Write("}")
                      .Write("}")
                      .Empty();
            }
            else
            {
                writer.Write("if ({0} != default(string))", name)
                      .Write("{")
                          .Write("outStream.WriteTag({0});", tag)
                          .Write("outStream.WriteString({0});", name)
                      .Write("}")
                      .Empty();
            }
        }

        static void GenEnumFieldEncode(IndentWriter writer, Field field)
        {
            uint tag = MakeTag(field);
            string name = field.FieldName;

            if (field.Repeated)
            {
                writer.Write("if ( {0} != null && {0}.Count >0 )", name)
                      .Write("{")
                          .Write("outStream.WriteTag({0});", tag)
                          .Write("outStream.BeginLengthRecord();")
                          .Write("for (int i = 0,len = {0}.Count; i < len; ++i)", name)
                          .Write("{")
                              .Write("outStream.WriteEnum((int){0}[i]);", name)
                          .Write("}")
                          .Write("outStream.EndLengthRecord();")
                      .Write("}")
                      .Empty();
            }
            else
            {
                writer.Write("if ({0} != default({1}))", name, field.UserType)
                      .Write("{")
                          .Write("outStream.WriteTag({0});", tag)
                          .Write("outStream.WriteEnum((int){0});", name)
                      .Write("}")
                      .Empty();
            }
        }

        static void GenCustomFieldEncode(IndentWriter writer, Field field)
        {
            uint tag = MakeTag(field);
            string name = field.FieldName;

            if (field.Repeated)
            {
                writer.Write("if ( {0} != null && {0}.Count >0 )", name)
                      .Write("{")
                          .Write("for (int i = 0,len = {0}.Count; i < len; ++i)", name)
                          .Write("{")
                              .Write("outStream.WriteTag({0});", tag)
                              .Write("outStream.BeginLengthRecord();")
                              .Write("{0}[i].Encode(outStream);", name)
                              .Write("outStream.EndLengthRecord();")
                          .Write("}")
                      .Write("}")
                      .Empty();
            }
            else
            {
                writer.Write("if ({0} != null)", name)
                      .Write("{")
                          .Write("outStream.WriteTag({0});", tag)
                          .Write("outStream.BeginLengthRecord();")
                          .Write("{0}.Encode(outStream);", name)
                          .Write("outStream.EndLengthRecord();")
                      .Write("}")
                      .Empty();
            }
        }
        #endregion

        #region Decode
        static void GenDecodeMethod(IndentWriter writer, ICollection<Field> fields)
        {
            writer.Write("public void Decode(Protobuf.CodedInputStream inStream)")
                  .Write("{")
                      .Write("uint tag;")
                      .Empty()
                      .Write("while ((tag = inStream.ReadTag()) != 0)")
                      .Write("{")
                          .Write("int fieldNum = Protobuf.WireFormat.GetTagFieldNumber(tag);")
                          .Empty()
                          .Write("switch (fieldNum)")
                          .Write("{");

            foreach(var field in fields)
            {
                GenOneFieldDecode(writer, field);
            }

                        writer.Write("default:")
                              .Write("\tinStream.SkipLastField();")
                              .Write("break;")
                          .Write("}") //switch
                      .Write("}") //while
                  .Write("}") //decode
                  .Empty();
        }

        static void GenOneFieldDecode(IndentWriter writer, Field field)
        {
            if (field.IsUsertype)
            {
                if (IsEnumField(field.UserType))
                {
                    GenEnumFieldDecode(writer, field);
                }
                else
                {
                    GenCustomFieldDecode(writer, field);
                }
            }
            else
            {
                switch (field.SimpleType)
                {
                    case SimpleType.Int32:
                    case SimpleType.Int64:
                    case SimpleType.Uint32:
                    case SimpleType.Uint64:
                    case SimpleType.Float:
                    case SimpleType.Double:
                    case SimpleType.Bool:
                    case SimpleType.Bytes:
                        GenValueFieldDecode(writer, field);
                        break;
                    case SimpleType.String:
                        GenStringFieldDecode(writer, field);
                        break;
                    default:
                        break;
                }
            }
        }

        static void GenValueFieldDecode(IndentWriter writer, Field field)
        {
            int idx = field.FieldNumber;
            string desc = field.SimpleType.ToString();
            string fieldName = field.FieldName;

            if (field.Repeated)
            {
                writer.Write("case {0}:", idx)
                      .Write("{")
                          .Write("if ({0} == null){{ {0} = new System.Collections.Generic.List<{1}>(); }}", fieldName, desc)
                          .Empty()
                          .Write("if (Protobuf.WireFormat.IsPacked(tag))")
                          .Write("{")
                              .Write("int len = inStream.ReadLength();")
                              .Write("if(len > 0)")
                              .Write("{")
                                  .Write("int oldLimit = inStream.PushLimit(len);")
                                  .Write("while(!inStream.ReachedLimit) {{ {0}.Add(inStream.Read{1}()); }}", fieldName, desc)
                                  .Empty()
                                  .Write("inStream.PopLimit(oldLimit);")
                              .Write("}")
                              .Write("else")
                              .Write("{")
                                  .Write("do")
                                  .Write("{")
                                      .Write("{0}.Add(inStream.Read{1}());", fieldName, desc)
                                  .Write("}")
                                  .Write("while (inStream.MaybeConsumeTag(tag));")
                              .Write("}")
                          .Write("}")
                      .Write("}")
                      .Write("break;");
            }
            else
            {
                writer.Write("case {0}:", idx)
                      .Write("{")
                          .Write("{0} = inStream.Read{1}();", fieldName, desc)
                      .Write("}")
                      .Write("break;");
            }
        }

        static void GenStringFieldDecode(IndentWriter writer, Field field)
        {
            if (field.Repeated)
            {
                writer.Write("case {0}:", field.FieldNumber)
                      .Write("{")
                          .Write("if ({0} == null) {{ {0} = new System.Collections.Generic.List<string>(); }}", field.FieldName)
                          .Empty()
                          .Write("do")
                          .Write("{")
                              .Write("{0}.Add(inStream.ReadString());", field.FieldName)
                          .Write("}")
                          .Write("while (inStream.MaybeConsumeTag(tag));")
                      .Write("}")
                      .Write("break;");
            }
            else
            {
                writer.Write("case {0}:", field.FieldNumber)
                      .Write("{")
                          .Write("{0} = inStream.ReadString();", field.FieldName)
                      .Write("}")
                      .Write("break;");
            }
        }

        static void GenEnumFieldDecode(IndentWriter writer, Field field)
        {
            int idx = field.FieldNumber;
            string desc = field.SimpleType.ToString();
            string fieldName = field.FieldName;

            if (field.Repeated)
            {
                writer.Write("case {0}:", idx)
                      .Write("{")
                          .Write("if ({0} == null){{ {0} = new System.Collections.Generic.List<{1}>(); }}", fieldName, desc)
                          .Empty()
                          .Write("if (Protobuf.WireFormat.IsPacked(tag))")
                          .Write("{")
                              .Write("int len = inStream.ReadLength();")
                              .Write("if(len > 0)")
                              .Write("{")
                                  .Write("int oldLimit = inStream.PushLimit(len);")
                                  .Write("while(!inStream.ReachedLimit) {{ {0}.Add(inStream.ReadEnum()); }}", fieldName)
                                  .Empty()
                                  .Write("inStream.PopLimit(oldLimit);")
                              .Write("}")
                              .Write("else")
                              .Write("{")
                                  .Write("do")
                                  .Write("{")
                                      .Write("{0}.Add(({1})inStream.ReadEnum());", fieldName, field.UserType)
                                  .Write("}")
                                  .Write("while (inStream.MaybeConsumeTag(tag));")
                              .Write("}")
                          .Write("}")
                      .Write("}")
                      .Write("break;");
            }
            else
            {
                writer.Write("case {0}:", idx)
                      .Write("{")
                          .Write("{0} = ({1})inStream.ReadEnum();", fieldName, field.UserType)
                      .Write("}")
                      .Write("break;");
            }
        }

        static void GenCustomFieldDecode(IndentWriter writer, Field field)
        {
            int idx = field.FieldNumber;
            string desc = field.UserType;
            string fieldName = field.FieldName;

            if (field.Repeated)
            {
                writer.Write("case {0}:", idx)
                      .Write("{")
                          .Write("if ({0} == null){{ {0} = new System.Collections.Generic.List<{1}>(); }}", fieldName, desc)
                          .Empty()
                          .Write("do")
                          .Write("{")
                              .Write("int len = inStream.ReadLength();")
                              .Write("int oldLimit = inStream.PushLimit(len);")
                              .Write("var obj = new {0}();", desc)
                              .Write("obj.Decode(inStream);")
                              .Write("inStream.CheckReadEndOfStreamTag();")
                              .Empty()
                              .Write("if (!inStream.ReachedLimit) throw Protobuf.InvalidProtocolBufferException.TruncatedMessage();")
                              .Empty()
                              .Write("{0}.Add(obj);", fieldName)
                              .Write("inStream.PopLimit(oldLimit);")
                          .Write("}")
                          .Write("while(inStream.MaybeConsumeTag(tag));")
                      .Write("}")
                      .Write("break;");
            }
            else
            {
                writer.Write("case {0}:", idx)
                      .Write("{")
                          .Write("if ({0} == null) {{ {0} = new {1}(); }}", fieldName, desc)
                          .Empty()
                          .Write("int len = inStream.ReadLength();")
                          .Write("int oldLimit = inStream.PushLimit(len);")
                          .Write("{0}.Decode(inStream);", fieldName)
                          .Empty()
                          .Write("if (!inStream.ReachedLimit) throw Protobuf.InvalidProtocolBufferException.TruncatedMessage();")
                          .Empty()
                          .Write("inStream.PopLimit(oldLimit);")
                      .Write("}")
                      .Write("break;");
            }
        }
        #endregion

        #region Helper
        static bool IsEnumField(string userType)
        {
            if (s_TmpDescriptor == null)
            {
                throw new ArgumentNullException("s_TmpDescriptor is null.");
            }
            foreach (var em in s_TmpDescriptor.Enumerations)
            {
                if (em.Name == userType)
                {
                    return true;
                }
            }
            return false;
        }

        static uint MakeTag(Field field)
        {
            if (field.Repeated)
            {
                return Protobuf.WireFormat.MakeTag(field.FieldNumber, Protobuf.WireFormat.WireType.LengthDelimited);
            }
            else if (field.IsUsertype)
            {
                if (IsEnumField(field.UserType))
                {
                    return Protobuf.WireFormat.MakeTag(field.FieldNumber, Protobuf.WireFormat.WireType.Varint);
                }
                else
                {
                    return Protobuf.WireFormat.MakeTag(field.FieldNumber, Protobuf.WireFormat.WireType.LengthDelimited);
                }
            }
            else
            {
                switch (field.SimpleType)
                {
                    case SimpleType.Int32:
                    case SimpleType.Int64:
                    case SimpleType.Uint32:
                    case SimpleType.Uint64:
                    case SimpleType.Bool:
                        return Protobuf.WireFormat.MakeTag(field.FieldNumber, Protobuf.WireFormat.WireType.Varint);
                    case SimpleType.Float:
                        return Protobuf.WireFormat.MakeTag(field.FieldNumber, Protobuf.WireFormat.WireType.Fixed32);
                    case SimpleType.Double:
                        return Protobuf.WireFormat.MakeTag(field.FieldNumber, Protobuf.WireFormat.WireType.Fixed64);
                    case SimpleType.String:
                    case SimpleType.Bytes:
                        return Protobuf.WireFormat.MakeTag(field.FieldNumber, Protobuf.WireFormat.WireType.LengthDelimited);
                    default:
                        return 0;
                }
            }
        }
        #endregion
    }
}

