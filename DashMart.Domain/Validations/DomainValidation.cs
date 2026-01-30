

using DashMart.Domain.Abstraction;
using System.Text.RegularExpressions;

namespace DashMart.Domain.Validations
{
    public sealed class DomainValidation
    {
        public static void EnsureValidString(string value, int maxLength, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{fieldName} cannot be empty.");
            if (value.Length > maxLength)
                throw new DomainException($"{fieldName} cannot exceed {maxLength} characters.");
        }
        public static bool IsValidEmailFormat(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, pattern);
        }
        public static bool IsValidPhoneNumberFormat(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            string pattern = @"^\d{11}$";

            return Regex.IsMatch(phoneNumber, pattern);
        }


       

    }
}
