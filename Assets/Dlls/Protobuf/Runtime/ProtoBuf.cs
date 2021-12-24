using System;
using System.IO;

/// <summary>
/// Protobuf API
/// </summary>
namespace Protobuf
{
    public sealed class ProtoBuf
    {
        public static byte[] Serialize<T>(T obj) where T : IMessage
        {
            IMessage o = (IMessage)obj;
            return Serialize(o);
        }

        public static byte[] Serialize(IMessage obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }

            byte[] bytes;
            CodedOutputStream outputStream = StreamPool.GetOutputStream();
            obj.Encode(outputStream);

            using (MemoryStream st = new MemoryStream())
            {
                outputStream.FlushToStream(st);
                bytes = st.ToArray();
            }
            StreamPool.PutBackOutputStream(outputStream);
            return bytes;
        }


        public static T Deserialize<T>(byte[] bytes) where T : IMessage
        {
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentNullException();
            }

            T obj = Activator.CreateInstance<T>();
            CodedInputStream inputStream = StreamPool.GetBuffedInputStream(bytes, 0, bytes.Length);
            obj.Decode(inputStream);
            StreamPool.PutBackBuffedInputStram(inputStream);
            return obj;
        }

        public static IMessage Deserialize(byte[] bytes,Type t)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentNullException();
            }

            object obj = Activator.CreateInstance(t);
            IMessage message = obj as IMessage;
            if (message == null)
            {
                throw new ArgumentNullException();
            }

            CodedInputStream inputStream = StreamPool.GetBuffedInputStream(bytes, 0, bytes.Length);
            message.Decode(inputStream);
            StreamPool.PutBackBuffedInputStram(inputStream);
            return message;
        }

        public static T Deserialize<T>(Stream stream) where T : IMessage
        {
            if (stream == null)
            {
                throw new ArgumentNullException();
            }

            T obj = Activator.CreateInstance<T>();
            CodedInputStream inputStream = StreamPool.GetInputStream(stream);
            obj.Decode(inputStream);
            StreamPool.PutBackInputStream(inputStream);
            return obj;
        }
    }
}


