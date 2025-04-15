using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DXC.Technology.Utilities;
using DXC.Technology.UnitTesting.Helpers;

namespace DXC.Technology.Utilities.Tests
{
    /// <summary>
    /// Test class for 'Age'
    /// </summary>
    [TestClass]
    public class AgeTests: TestingClassBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgeTests"/> class.
        /// </summary>
        public AgeTests() : base(UnitTesting.Enumerations.TechnologyUnitTestTypeEnum.Core)
        {
            TestClassName = GetType().Name;
        }

        /// <summary>
        /// A test for the constructor with a single parameter (birthday).
        /// </summary>
        [TestMethod()]
        public void Constructor_SingleParameter_ShouldCalculateCorrectAge()
        {
            // Arrange
            DateTime birthday = new DateTime(2000, 1, 1);
            DateTime today = DateTime.Today;

            // Act
            Age age = new Age(birthday);

            // Assert
            this.IsTrue(age.Years == today.Year - birthday.Year, "Years should be correctly calculated.");
            this.IsTrue(age.Months >= 0 && age.Months <= 11, "Months should be within valid range.");
            this.IsTrue(age.Days >= 0 && age.Days <= 31, "Days should be within valid range.");
        }

        /// <summary>
        /// A test for the constructor with two parameters (birthday and comparison date).
        /// </summary>
        [TestMethod()]
        public void Constructor_TwoParameters_ShouldCalculateCorrectAge()
        {
            // Arrange
            DateTime birthday = new DateTime(2000, 1, 1);
            DateTime comparisonDate = new DateTime(2023, 10, 1);

            // Act
            Age age = new Age(birthday, comparisonDate);

            // Assert
            this.IsTrue(age.Years == 23, "Years should be correctly calculated.");
            this.IsTrue(age.Months == 9, "Months should be correctly calculated.");
            this.IsTrue(age.Days == 0, "Days should be correctly calculated.");
        }

        /// <summary>
        /// A test for the Count method with a single parameter (birthday).
        /// </summary>
        [TestMethod()]
        public void Count_SingleParameter_ShouldCalculateCorrectAge()
        {
            // Arrange
            DateTime birthday = new DateTime(2000, 1, 1);
            DateTime today = DateTime.Today;
            Age age = new Age(birthday);

            // Act
            age.Count(birthday);

            // Assert
            this.IsTrue(age.Years == today.Year - birthday.Year, "Years should be correctly calculated.");
            this.IsTrue(age.Months >= 0 && age.Months <= 11, "Months should be within valid range.");
            this.IsTrue(age.Days >= 0 && age.Days <= 31, "Days should be within valid range.");
        }

        /// <summary>
        /// A test for the Count method with two parameters (birthday and comparison date).
        /// </summary>
        [TestMethod()]
        public void Count_TwoParameters_ShouldCalculateCorrectAge()
        {
            // Arrange
            DateTime birthday = new DateTime(2000, 1, 1);
            DateTime comparisonDate = new DateTime(2023, 10, 1);
            Age age = new Age(birthday);

            // Act
            age.Count(birthday, comparisonDate);

            // Assert
            this.IsTrue(age.Years == 23, "Years should be correctly calculated.");
            this.IsTrue(age.Months == 9, "Months should be correctly calculated.");
            this.IsTrue(age.Days == 0, "Days should be correctly calculated.");
        }

        /// <summary>
        /// A test for invalid birthday (future date).
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Count_InvalidBirthday_ShouldThrowException()
        {
            // Arrange
            DateTime futureBirthday = DateTime.Today.AddYears(1);

            // Act
            Age age = new Age(futureBirthday);
        }
    }
}