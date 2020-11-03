using System.Linq;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Internal;

namespace VRPersistence.Extensions
{
    public static class ValidationResultExtensions
    {
        public static string GetMessage(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .Select(error => error.ErrorMessage)
                .Join(";");
        }
    }
}