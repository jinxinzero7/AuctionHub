using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Results
{
    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value) : base(true, string.Empty, ErrorType.None)
        {
            Value = value;
        }

        private Result(string error, ErrorType errorType) : base(false, error, errorType)
        {
            Value = default;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static new Result<T> Failure(string error, ErrorType errorType) => new Result<T>(error, errorType);
    }
}