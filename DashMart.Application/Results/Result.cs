
namespace DashMart.Application.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public List<string>? ErrorMessages { get; }
        public T? Value { get; } 
        public StatusCodeEnum StatusCode { get; }
       
        private Result(bool isSuccess, T value, string? message, StatusCodeEnum statusCode)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Value = value;

            if(message != null)
                ErrorMessages = new List<string> { message};
            else
                ErrorMessages = null;
        }

        private Result(List<string> message, StatusCodeEnum statusCode)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Value = default;

            if (message != null)
                ErrorMessages = new List<string>(message);
            else
                ErrorMessages = null;
        }

        public static Result<T> ValidationFailure(List<string> messages,  StatusCodeEnum statusCode = StatusCodeEnum.BadRequest)
        {
            return new Result<T>(messages, statusCode);
        }

        public static Result<T> Success(T value) => new Result<T>(true,value, null, StatusCodeEnum.Success);

        public static Result<T> Failure(string message, StatusCodeEnum statusCode)
        {
            return new Result<T>(new List<string> { message }, statusCode);
        }

        public static Result<T> Failure(List<string> message, StatusCodeEnum statusCode)
        {
            return new Result<T>(message, statusCode);
        }

        public static implicit operator Result<T>(T value) => Success(value);

    }
}
