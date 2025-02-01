using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Core.Domain
{
    public class AssertionConcern
    {
        public static void AssertValidEmail(string email, string message)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new(pattern);
            if(!regex.IsMatch(email))
                throw new DomainException(message);
        }
        public static void AssertArgumentEquals(object object1, object object2, string message)
        {
            if (!object1.Equals(object2))
                throw new DomainException(message);
        }
        public static void AssertArgumentFalse(bool boolValue, string message)
        {
            if (boolValue)
                throw new DomainException(message);
        }
        public static void AssertArgumentLength(string stringValue, int maximum, string message)
        {
            var length = stringValue.Trim().Length;

            if (length > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLength(string stringValue, int minimum, int maximum, string message)
        {
            var length = stringValue.Trim().Length;

            if (length < minimum || length > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentMatches(string pattern, string stringValue, string message)
        {
            var regex = new Regex(pattern);

            if (!regex.IsMatch(stringValue))
                throw new DomainException(message);
        }
        public static void AssertArgumentNotEmpty(string stringValue, string message)
        {
            if (stringValue == null || stringValue.Trim().Length == 0)
                throw new DomainException(message);
        }
        public static void AssertArgumentNotEquals(object object1, object object2, string message)
        {
            if (object1.Equals(object2))
                throw new DomainException(message);
        }
        public static void AssertArgumentNotNull(object object1, string message)
        {
            if (object1 == null)
                throw new DomainException(message);
        }
        public static void AssertArgumentRange(double value, double minimum, double maximum, string message)
        {
            if (value < minimum || value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentRange(float value, float minimum, float maximum, string message)
        {
            if (value < minimum || value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentRange(int value, int minimum, int maximum, string message)
        {
            if (value < minimum || value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentRange(long value, long minimum, long maximum, string message)
        {
            if (value < minimum || value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentTrue(bool boolValue, string message)
        {
            if (!boolValue)
                throw new DomainException(message);
        }
        public static void AssertStateFalse(bool boolValue, string message)
        {
            if (boolValue)
                throw new DomainException(message);
        }
        public static void AssertStateTrue(bool boolValue, string message)
        {
            if (!boolValue)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessThen(int value, int minimum, string message)
        {
            if (value < minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessThen(double value, double minimum, string message)
        {
            if (value < minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessThen(float value, float minimum, string message)
        {
            if (value < minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessThen(decimal value, decimal minimum, string message)
        {
            if (value < minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessThen(TimeOnly value, TimeOnly minimum, string message)
        {
            if (value < minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentGreaterThen(int value, int maximum, string message)
        {
            if (value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentGreaterThen(double value, double maximum, string message)
        {
            if (value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentGreaterThen(float value, float maximum, string message)
        {
            if (value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentGreaterThen(decimal value, decimal maximum, string message)
        {
            if (value > maximum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessOrEqualThen(decimal value, decimal minimum, string message)
        {
            if (value <= minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessOrEqualThen(float value, float minimum, string message)
        {
            if (value <= minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessOrEqualThen(double value, double minimum, string message)
        {
            if (value <= minimum)
                throw new DomainException(message);
        }
        public static void AssertArgumentLessOrEqualThen(long value, long minimum, string message)
        {
            if (value <= minimum)
                throw new DomainException(message);
        }
    }
}
