using System;
using System.Collections.Generic;
using System.IO;

namespace Protobuf
{
    class LengthRecord
    {
        public int StartPos { get;}
        public int Length { get; private set; }
        public int Depth { get; }
       
        public LengthRecord(int Pos, int Depth)
        {
            StartPos = Pos;
            this.Depth = Depth;
            Length = -1;
        }

        public void Close(int Pos, int ExtLength)
        {
            Length = Pos - StartPos + ExtLength;
        }
    }

    public sealed partial class CodedOutputStream
    {
        private List<LengthRecord> records;
        private int CurIndex = -1;

        private byte[] lenghtBuffer;

        public void BeginLengthRecord()
        {
            if (records == null)
            {
                records = new List<LengthRecord>();                
            }
            
            if (CurIndex == -1)
            {
                // 创建1级父序列
                var add = new LengthRecord(position, 1);
                records.Add(add);
                CurIndex = records.Count - 1;
            }else
            {
                // 创建子序列
                var add = new LengthRecord(position, records[CurIndex].Depth + 1);
                records.Add(add);
                CurIndex = records.Count - 1;
            }
        }

        public void EndLengthRecord()
        {
            if (records == null || CurIndex == -1 || records[CurIndex] == null)
            {
                throw new Exception("code error. not init");
            }

            var Cur = records[CurIndex];
            if (Cur.Length >= 0)
            {
                throw new Exception("record already close!");
            }

            int extlen = 0;
            // 向后计算子集
            for (int index = CurIndex + 1; 
                // 没到最后 且 都是我的后代节点
                index < records.Count && records[index].Depth > Cur.Depth; 
                index++)
            {
                //我的直系子节点
                {
                    if (records[index].Length < 0)
                    {
                        throw new Exception("lenght error");
                    }
                    extlen += ComputeLengthSize(records[index].Length);
                }
            }
            // 关闭当前节点
            Cur.Close(position, extlen);

            // 当前节点已关闭 向前定位到父节点
            CurIndex--;
            while (CurIndex > 0 && records[CurIndex].Depth >= Cur.Depth) CurIndex--;
        }

        public void FlushToStream(Stream stream)
        {
            if (records == null || records.Count == 0)
            {
                stream.Write(buffer, 0, position);
                position = 0;
                return;
            }

            int last = 0;
            foreach (var record in records)
            {
                if (record.StartPos > last)
                {
                    stream.Write(buffer, last, record.StartPos - last);
                    last = record.StartPos;
                }
                _OptWriteLength(stream, record.Length);
            }

            if (last < position)
            {
                stream.Write(buffer, last, position - last);
            }

            records.Clear();
            CurIndex = -1;
            position = 0;   
        }

        private void _OptWriteLength(Stream stream, int length)
        {  
            if (length < 0)
            {
                throw new Exception("length error");
            }

            if (lenghtBuffer == null)
            {
                lenghtBuffer = new byte[30];
            }
            int pos = 0;
            if (length < 128)
            {
                lenghtBuffer[pos++] = (byte)length;
            }else
            {
                while(length > 127)
                {
                    lenghtBuffer[pos++] = (byte) ((length & 0x7F) | 0x80);
                    length >>= 7;
                }
                lenghtBuffer[pos++] = (byte)length;
            }

            stream.Write(lenghtBuffer, 0, pos);
        }
    }
}
