using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }
        public ErrorType ErrorType { get; }

        protected Result(bool isSuccess, string error, ErrorType errorType)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
        }

        public static Result Success() => new Result(true, string.Empty, ErrorType.None);
        public static Result Failure(string error, ErrorType errorType = ErrorType.Validation) 
            => new Result(false, error, errorType);
    
        public static Result<T> Success<T>(T value) => Result<T>.Success(value);
        public static Result<T> Failure<T>(string error, ErrorType errorType = ErrorType.Validation) 
            => Result<T>.Failure(error, errorType);
    }
}