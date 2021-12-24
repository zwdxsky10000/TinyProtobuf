namespace PGCompiler.Compiler.Errors
{

    public abstract class CompilerError
    {
        internal string Message { get; }

        internal CompilerError(string message)
        {
            Message = message;
        }
    }
}