using PGCompiler.Compiler;
using System.Collections.Generic;

namespace PGCompiler.Interfaces
{
    /// <summary>
    /// Defines an implementation that can analyze a <see cref="Source"/> into a stream of <see cref="Token"/>s
    /// </summary>
    internal interface ILexicalAnalyzer
    {
        /// <summary>
        /// Perform Lexical Analysis on the <see cref="Source"/> input to the constructor
        /// </summary>
        void Tokenize();

        /// <summary>
        /// Get the results of <see cref="Tokenize"/> as <see cref="System.Collections.Generic.Queue{T}"/> where T is a <see cref="Token"/>
        /// </summary>
        Queue<Token> TokenStream { get; }
    }
}