using System;
using CSharpFunctionalExtensions;

namespace VRPersistence.Extensions
{
    public static class ResultExtensions
    {
        public static SerializableResult<TValue> AsSerializableResult<TValue>(this Result<TValue> result, string redirectUrl)
        {
            return new SerializableResult<TValue>
            {
                IsSuccess = result.IsSuccess,
                Error = result.IsFailure ? result.Error : "",
                Value = result.IsSuccess ? result.Value : default,
                RedirectUrl = redirectUrl
            };
        }
        
        public static SerializableResult<TValue, TError> AsSerializableResult<TValue, TError>(this Result<TValue, TError> result, string redirectUrl)
        {
            return new SerializableResult<TValue, TError>
            {
                IsSuccess = result.IsSuccess,
                ErrorObject = result.IsFailure ? result.Error : default,
                Value = result.IsSuccess ? result.Value : default,
                RedirectUrl = redirectUrl
            };
        }
        
        public static SerializableResult<TValue> AsSerializableResult<TValue>(this Result<TValue> result)
        { 
            return AsSerializableResult(result, String.Empty);
        }
        
        public static SerializableResult<TValue, TError> AsSerializableResult<TValue, TError>(this Result<TValue, TError> result)
        { 
            return AsSerializableResult(result, String.Empty);
        }

        public static SerializableResult AsSerializableResult(this Result result, string redirectUrl)
        {
            return new SerializableResult
            {
                IsSuccess = result.IsSuccess,
                Error = result.IsFailure ? result.Error : "",
                RedirectUrl = redirectUrl
            };
        }
        
        public static SerializableResult AsSerializableResult(this Result result)
        {
            return AsSerializableResult(result, String.Empty);
        }
    }
    
    public class SerializableResult
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class SerializableResult<TValue> : SerializableResult
    {
        public TValue Value { get; set; }
    }
    
    public class SerializableResult<TValue, TError> : SerializableResult
    {
        public TValue Value { get; set; }
        public TError ErrorObject { get; set; }
    }
}
