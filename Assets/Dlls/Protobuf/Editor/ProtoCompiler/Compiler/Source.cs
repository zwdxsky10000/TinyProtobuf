using PGCompiler.Extensions;
using PGCompiler.Interfaces;
using System;
using System.IO;

namespace PGCompiler.Compiler
{
    internal class Source : ISource
    {
        private readonly StreamReader _streamReader = null;

        public int Column { get; private set; }
        public int Line { get; private set; }

        public bool EndStream => _streamReader.EndOfStream;

        private const int PeekEndFile = -1;

        internal Source(StreamReader streamReader)
        {
            _streamReader = Check.NotNull(streamReader, nameof(streamReader));
            Line = 1;
            Column = 0;
        }

        public char Peek()
        {
            if (EndStream) throw new EndOfStreamException("End of Stream Reached.");
            return Convert.ToChar(_streamReader.Peek());
        }

        public char Next()
        {
            if (EndStream) throw new EndOfStreamException("End of Stream Reached.");

            var singleBuffer = new char[1];
            _streamReader.Read(singleBuffer, 0, 1);

            var next = singleBuffer[0];

            // Peek returns -1 at EoF, and we can't Convert or cast this to char.
            var peek = _streamReader.Peek();
            var lookahead = (peek.Equals(PeekEndFile)) ? ' ' : Convert.ToChar(peek);

            Column++;

            // Windows - Dump the extraneous LineFeed
            if (next.IsCarriageReturn() && lookahead.IsLineFeed()) _streamReader.Read();

            // All Platforms -> Increase the Line Count and reset Column to 1
            if (next.IsCarriageReturn() /* Mac or Win */ || next.IsLineFeed() /* Unix */)
            {
                Line++;
                // Next call to 'Next' will have first character in the line returned as Column 1
                Column = 0;
            }

            return next;
        }

        public void Dispose()
        {
            if(_streamReader != null)
            {
                _streamReader.Close();
            }
        }
    }
}