using System.IO;
using System.Text;

namespace PGGenerator
{
    class IndentWriter
    {
        private StreamWriter writer;
        private int indent;
    
        string NewLine
        {
            get
            {
                return System.Environment.NewLine;
            }
        }

        public IndentWriter(string fileNameWithPath)
        {
            writer = new StreamWriter(fileNameWithPath, false, Encoding.UTF8);
            writer.NewLine = NewLine;
            indent = 0;
        }

        public IndentWriter Write(string fmt, params object[] args)
        {
            fmt = System.Text.RegularExpressions.Regex.Replace(fmt, @"\r\n?|\n|\r", NewLine);

            if (fmt.StartsWith("}")) indent--;

            for (int n = 0; n < indent; n++)
                writer.Write("\t");

            if (args.Length == 0)
                writer.WriteLine(fmt);
            else
            {
                string line = string.Format(fmt, args);
                writer.WriteLine(line);
            }

            if (fmt.EndsWith("{")) indent++;
            return this;
        }

        public IndentWriter WriteNative(string fmt, params object[] args)
        {
            fmt = System.Text.RegularExpressions.Regex.Replace(fmt, @"\r\n?|\n|\r", NewLine);

            for (int n = 0; n < indent; n++)
                writer.Write("\t");

            if (args.Length == 0)
                writer.WriteLine(fmt);
            else
            {
                string line = string.Format(fmt, args);
                writer.WriteLine(line);
            }

            return this;
        }

        public IndentWriter Empty()
        {
            writer.WriteLine();
            return this;
        }

        public IndentWriter AddIndent()
        {
            indent++;
            return this;
        }

        public IndentWriter DescIndent()
        {
            indent--;
            return this;
        }
        public void Finish()
        {
            writer.Flush();
            writer.Close();
        }
    }
}
