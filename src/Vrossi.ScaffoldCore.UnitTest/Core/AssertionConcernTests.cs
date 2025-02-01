using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Core.Domain;

namespace Vrossi.ScaffoldCore.UnitTest.Core
{
    public class AssertionConcernTests
    {
        [TestMethod]
        public void AssertValidEmail_ShouldNotThrowException_WhenEmailIsValid()
        {
            // Arrange
            string validEmail = "test@example.com";

            // Act & Assert
            AssertionConcern.AssertValidEmail(validEmail, "Invalid email");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertValidEmail_ShouldThrowException_WhenEmailIsInvalid()
        {
            // Arrange
            string invalidEmail = "invalid-email";

            // Act
            AssertionConcern.AssertValidEmail(invalidEmail, "Invalid email");
        }

        [TestMethod]
        public void AssertArgumentEquals_ShouldNotThrowException_WhenValuesAreEqual()
        {
            // Arrange
            object value1 = "test";
            object value2 = "test";

            // Act & Assert
            AssertionConcern.AssertArgumentEquals(value1, value2, "Values should be equal");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentEquals_ShouldThrowException_WhenValuesAreNotEqual()
        {
            // Arrange
            object value1 = "test1";
            object value2 = "test2";

            // Act
            AssertionConcern.AssertArgumentEquals(value1, value2, "Values are not equal");
        }

        [TestMethod]
        public void AssertArgumentFalse_ShouldNotThrowException_WhenValueIsFalse()
        {
            // Act & Assert
            AssertionConcern.AssertArgumentFalse(false, "Value should be false");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentFalse_ShouldThrowException_WhenValueIsTrue()
        {
            // Act
            AssertionConcern.AssertArgumentFalse(true, "Value should be false");
        }

        [TestMethod]
        public void AssertArgumentNotNull_ShouldNotThrowException_WhenObjectIsNotNull()
        {
            // Act & Assert
            AssertionConcern.AssertArgumentNotNull(new object(), "Object should not be null");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentNotNull_ShouldThrowException_WhenObjectIsNull()
        {
            // Act
            AssertionConcern.AssertArgumentNotNull(null, "Object is null");
        }

        [TestMethod]
        public void AssertArgumentNotEmpty_ShouldNotThrowException_WhenStringIsNotEmpty()
        {
            // Act & Assert
            AssertionConcern.AssertArgumentNotEmpty("test", "String should not be empty");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentNotEmpty_ShouldThrowException_WhenStringIsEmpty()
        {
            // Act
            AssertionConcern.AssertArgumentNotEmpty("", "String is empty");
        }

        [TestMethod]
        public void AssertArgumentRange_ShouldNotThrowException_WhenValueIsInRange()
        {
            // Act & Assert
            AssertionConcern.AssertArgumentRange(5, 1, 10, "Value should be in range");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentRange_ShouldThrowException_WhenValueIsOutOfRange()
        {
            // Act
            AssertionConcern.AssertArgumentRange(15, 1, 10, "Value is out of range");
        }

        [TestMethod]
        public void AssertArgumentMatches_ShouldNotThrowException_WhenStringMatchesPattern()
        {
            // Arrange
            string pattern = @"^\d{3}-\d{3}-\d{4}$"; // Example: 123-456-7890
            string validInput = "123-456-7890";

            // Act & Assert
            AssertionConcern.AssertArgumentMatches(pattern, validInput, "String should match pattern");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentMatches_ShouldThrowException_WhenStringDoesNotMatchPattern()
        {
            // Arrange
            string pattern = @"^\d{3}-\d{3}-\d{4}$"; // Example: 123-456-7890
            string invalidInput = "12345";

            // Act
            AssertionConcern.AssertArgumentMatches(pattern, invalidInput, "String does not match pattern");
        }

        [TestMethod]
        public void AssertArgumentTrue_ShouldNotThrowException_WhenValueIsTrue()
        {
            // Act & Assert
            AssertionConcern.AssertArgumentTrue(true, "Value should be true");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentTrue_ShouldThrowException_WhenValueIsFalse()
        {
            // Act
            AssertionConcern.AssertArgumentTrue(false, "Value should be true");
        }

        [TestMethod]
        public void AssertArgumentLength_ShouldNotThrowException_WhenStringLengthIsValid()
        {
            // Arrange
            string validString = "Test";

            // Act & Assert
            AssertionConcern.AssertArgumentLength(validString, 2, 10, "Invalid string length");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentLength_ShouldThrowException_WhenStringLengthIsTooShort()
        {
            // Arrange
            string shortString = "T";

            // Act
            AssertionConcern.AssertArgumentLength(shortString, 2, 10, "Invalid string length");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentLength_ShouldThrowException_WhenStringLengthIsTooLong()
        {
            // Arrange
            string longString = "This is a long string";

            // Act
            AssertionConcern.AssertArgumentLength(longString, 2, 10, "Invalid string length");
        }

        [TestMethod]
        public void AssertArgumentLessThen_ShouldNotThrowException_WhenValueIsGreaterThanMinimum()
        {
            // Act & Assert
            AssertionConcern.AssertArgumentLessThen(5, 3, "Value should be greater than minimum");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentLessThen_ShouldThrowException_WhenValueIsLessThanMinimum()
        {
            // Act
            AssertionConcern.AssertArgumentLessThen(2, 3, "Value is less than minimum");
        }

        [TestMethod]
        public void AssertArgumentGreaterThen_ShouldNotThrowException_WhenValueIsLessThanMaximum()
        {
            // Act & Assert
            AssertionConcern.AssertArgumentGreaterThen(5, 10, "Value should be less than maximum");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AssertArgumentGreaterThen_ShouldThrowException_WhenValueIsGreaterThanMaximum()
        {
            // Act
            AssertionConcern.AssertArgumentGreaterThen(15, 10, "Value is greater than maximum");
        }
    }
}
