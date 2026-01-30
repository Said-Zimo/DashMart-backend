

namespace DashMart.Application.Exceptions
{
    public sealed class AppValidationException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }
        public AppValidationException(IReadOnlyDictionary<string, string[]> errors) 
            : base("One or more validations occurred")
        => Errors = errors;
        
    }
}
