using System.Collections.Generic;
using System.IO;

namespace Protobuf
{
    public sealed class StreamPool
    {
        private static Queue<CodedOutputStream> s_outStreamPool = new Queue<CodedOutputStream>();
        private static Queue<CodedInputStream> s_inStreamPool = new Queue<CodedInputStream>();
        private static Queue<CodedInputStream> s_buffedInPool = new Queue<CodedInputStream>();

        public static CodedInputStream GetInputStream(Stream src)
        {
            if (s_inStreamPool != null && s_inStreamPool.Count > 0)
            {
                CodedInputStream input = s_inStreamPool.Dequeue();
                input.PoolReUse(src);
                return input;
            }
            else
            {
                return new CodedInputStream(src);
            }
        }

        public static CodedInputStream GetInputStream(Stream src, int limit)
        {
            if (s_inStreamPool != null && s_inStreamPool.Count > 0)
            {
                CodedInputStream input = s_inStreamPool.Dequeue();
                input.PoolReUse(src,limit);
                return input;
            }
            else
            {
                var input = new CodedInputStream(src);
                input.SetSizeLimit(limit);
                return input;
            }
        }

        public static CodedInputStream GetBuffedInputStream(byte[] buff, int offset, int length)
        {
            if (s_buffedInPool != null && s_buffedInPool.Count > 0)
            {
                var input = s_buffedInPool.Dequeue();
                input.PoolReUse(buff, offset, length);
                return input;
            }else
            {
                return new CodedInputStream(buff, offset, length);
            }
        }

        public static void PutBackBuffedInputStram(CodedInputStream input)
        {
            s_buffedInPool.Enqueue(input);
        }


        public static void PutBackInputStream(CodedInputStream input)
        {
            s_inStreamPool.Enqueue(input);
        }

        public static CodedOutputStream GetOutputStream()
        {
            if (s_outStreamPool != null && s_outStreamPool.Count > 0)
            {
                CodedOutputStream output = s_outStreamPool.Dequeue();
                output.PoolReUse(null);
                return output;
            }
            else
            {
                //TODO:数据包缓存最大500K
                return new CodedOutputStream(new byte[1024 * 500]);
            }
        }

        public static void PutBackOutputStream(CodedOutputStream output)
        {
            s_outStreamPool.Enqueue(output);
        }
    }
}