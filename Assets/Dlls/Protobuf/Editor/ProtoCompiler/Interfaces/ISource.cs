namespace PGCompiler.Interfaces
{
    /// <summary>
    /// Represents the source of a character stream.
    /// </summary>
    internal interface ISource
    {
        /// <summary>
        /// Check if the stream is at the end of input
        /// Simply wraps the underlying stream property EndOfStream
        /// </summary>
        bool EndStream { get; }

        /// <summary>
        /// Get the next character in the stream.
        /// </summary>
        char Next();

        /// <summary>
        /// Look at the next character in the stream without consuming it.
        /// </summary>
        /// <returns></returns>
        char Peek();


        void Dispose();

        /// <summary>
        /// Get the current line of the input. Increments when a
        /// newline character is found in the stream.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Get the current string position of the input. Increments on Next.
        /// </summary>
        int Column { get; }
    }
}