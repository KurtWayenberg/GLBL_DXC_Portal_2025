    using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DXC.Technology.UnitTesting.Helpers;
using DXC.Technology.UnitTesting.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXC.Technology.UnitTesting.TestStatistic;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace DXC.Technology.UnitTesting.Helpers
{
    [TestClass]
    public class TestingClassBase
    {
        #region Instance Fields

        /// <summary>
        /// Gets or sets the type of unit test.
        /// </summary>
        public TechnologyUnitTestTypeEnum UnitTestType { get; set; }

        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Gets or sets the name of the test class.
        /// </summary>
        public string TestClassName { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestingClassBase"/> class.
        /// </summary>
        /// <param name="unitTestType">The type of unit test.</param>
        public TestingClassBase(TechnologyUnitTestTypeEnum unitTestType)
        {
            UnitTestType = unitTestType;
            TestClassName = GetType().Name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the test.
        /// </summary>
        /// <param name="">No parameters are required for this method.</param>
        [TestInitialize]
        public void TestInitialize()
        {
            TestContextPropertiesHelper.AddTypeAndClassNameToTextContentProperties(UnitTestType, TestClassName, TestContext);
        }

        /// <summary>
        /// Cleans up after the test.
        /// </summary>
        /// <param name="">No parameters are required for this method.</param>
        [TestCleanup]
        public void TestCleanup()
        {
            TestStatisticsHelper.Current.ReportTestOutCome(TestContext);
        }

        public void AreEqual(object expected, object actual, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, message);
        }
        //
        // Summary:
        //     Verifies that two specified strings are equal, ignoring case or not as specified.
        //     The assertion fails if they are not equal.
        //
        // Parameters:
        //   expected:
        //     The first string to compare. This is the string the unit test expects.
        //
        //   actual:
        //     The second string to compare. This is the string the unit test produced.
        //
        //   ignoreCase:
        //     A Boolean value that indicates a case-sensitive or insensitive comparison.
        //     true indicates a case-insensitive comparison.
        //
        // Exceptions:
        //   Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException:
        //     expected is not equal to actual.
        public void AreEqual<T>(T expected, T actual, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual<T>(expected, actual, message);
        }
        //
        // Summary:
        //     Verifies that two specified doubles are equal, or within the specified accuracy
        //     of each other. The assertion fails if they are not within the specified accuracy
        //     of each other. Displays a message if the assertion fails.
        //
        // Parameters:
        //   expected:
        //     The first double to compare. This is the double the unit test expects.
        //
        //   actual:
        //     The second double to compare. This is the double the unit test produced.
        //
        //   delta:
        //     The required accuracy. The assertion will fail only if expected is different
        //     from actual by more than delta.
        //
        //   message:
        //     A message to display if the assertion fails. This message can be seen in
        //     the unit test results.
        //
        // Exceptions:
        //   Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException:
        //     expected is different from actual by more than delta.
        public void AreEqual(double expected, double actual, double delta, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, delta, message);
        }
        //
        // Summary:
        //     Verifies that two specified singles are equal, or within the specified accuracy
        //     of each other. The assertion fails if they are not within the specified accuracy
        //     of each other. Displays a message if the assertion fails.
        //
        // Parameters:
        //   expected:
        //     The first single to compare. This is the single the unit test expects.
        //
        //   actual:
        //     The second single to compare. This is the single the unit test produced.
        //
        //   delta:
        //     The required accuracy. The assertion will fail only if expected is different
        //     from actual by more than delta.
        //
        //   message:
        //     A message to display if the assertion fails. This message can be seen in
        //     the unit test results.
        //
        // Exceptions:
        //   Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException:
        //     expected is not equal to actual.
        public void AreEqual(float expected, float actual, float delta, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, delta, message);
        }
        //
        // Summary:
        //     Verifies that two specified objects are equal. The assertion fails if the
        //     objects are not equal. Displays a message if the assertion fails, and applies
        //     the specified formatting to it.
        //
        // Parameters:
        //   expected:
        //     The first object to compare. This is the object the unit test expects.
        //
        //   actual:
        //     The second object to compare. This is the object the unit test produced.
        //
        //   message:
        //     A message to display if the assertion fails. This message can be seen in
        //     the unit test results.
        //
        //   parameters:
        //     An array of parameters to use when formatting message.
        //
        // Exceptions:
        //   Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException:
        //     expected is not equal to actual.
        public void AreEqual(object expected, object actual, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, message, parameters);
        }
        //
        // Summary:
        //     Verifies that two specified strings are equal, ignoring case or not as specified.
        //     The assertion fails if they are not equal. Displays a message if the assertion
        //     fails.
        //
        // Parameters:
        //   expected:
        //     The first string to compare. This is the string the unit test expects.
        //
        //   actual:
        //     The second string to compare. This is the string the unit test produced.
        //
        //   ignoreCase:
        //     A Boolean value that indicates a case-sensitive or insensitive comparison.
        //     true indicates a case-insensitive comparison.
        //
        //   message:
        //     A message to display if the assertion fails. This message can be seen in
        //     the unit test results.
        //
        // Exceptions:
        //   Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException:
        //     expected is not equal to actual.
        public void AreEqual(string expected, string actual, bool ignoreCase, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, ignoreCase, message);
        }
        //
        // Summary:
        //     Verifies that two specified generic type data are equal. The assertion fails
        //     if they are not equal. Displays a message if the assertion fails, and applies
        //     the specified formatting to it.
        //
        // Parameters:
        //   expected:
        //     The first generic type data to compare. This is the generic type data the
        //     unit test expects.
        //
        //   actual:
        //     The second generic type data to compare. This is the generic type data the
        //     unit test produced.
        //
        //   message:
        //     A message to display if the assertion fails. This message can be seen in
        //     the unit test results.
        //
        //   parameters:
        //     An array of parameters to use when formatting message.
        //
        // Type parameters:
        //   T:
        //
        // Exceptions:
        //   Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException:
        //     expected is not equal to actual.
        public void AreEqual<T>(T expected, T actual, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual<T>(expected, actual, message, parameters);
        }

        // Verifies that two specified doubles are equal, or within the specified accuracy
        public void AreEqual(double expected, double actual, double delta, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, delta, message, parameters);
        }

        // Verifies that two specified singles are equal, or within the specified accuracy
        public void AreEqual(float expected, float actual, float delta, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, delta, message, parameters);
        }

        // Verifies that two specified strings are equal, ignoring case and using the culture info specified
        public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, ignoreCase, culture, message);
        }

        // Verifies that two specified strings are equal, ignoring case
        public void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, ignoreCase, message, parameters);
        }

        // Verifies that two specified strings are equal, ignoring case and using culture info, with formatted message
        public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreEqual(expected, actual, ignoreCase, culture, message, parameters);
        }

        // Verifies that two specified objects are not equal with a message
        public void AreNotEqual(object notExpected, object actual, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, message);
        }

        // Verifies that two specified generic values are not equal with a message
        public void AreNotEqual<T>(T notExpected, T actual, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual<T>(notExpected, actual, message);
        }

        // Verifies that two doubles are not equal within a given delta
        public void AreNotEqual(double notExpected, double actual, double delta, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, delta, message);
        }

        // Verifies that two floats are not equal within a given delta
        public void AreNotEqual(float notExpected, float actual, float delta, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, delta, message);
        }

        // Verifies that two objects are not equal with formatted message
        public void AreNotEqual(object notExpected, object actual, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, message, parameters);
        }

        // Verifies that two strings are not equal, ignoring case
        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, ignoreCase, message);
        }

        // Verifies that two generic values are not equal with formatted message
        public void AreNotEqual<T>(T notExpected, T actual, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual<T>(notExpected, actual, message, parameters);
        }

        // Verifies that two doubles are not equal within a given delta with formatted message
        public void AreNotEqual(double notExpected, double actual, double delta, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, delta, message, parameters);
        }

        // Verifies that two floats are not equal within a given delta with formatted message
        public void AreNotEqual(float notExpected, float actual, float delta, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, delta, message, parameters);
        }

        // Verifies that two strings are not equal, ignoring case and using culture
        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, ignoreCase, culture, message);
        }

        // Verifies that two strings are not equal, ignoring case and using culture, with formatted message
        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotEqual(notExpected, actual, ignoreCase, culture, message, parameters);
        }

        // Verifies that two objects refer to different instances
        public void AreNotSame(object notExpected, object actual, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotSame(notExpected, actual, message);
        }

        // Verifies that two objects refer to different instances with formatted message
        public void AreNotSame(object notExpected, object actual, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreNotSame(notExpected, actual, message, parameters);
        }

        // Verifies that two objects refer to the same instance
        public void AreSame(object expected, object actual, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreSame(expected, actual, message);
        }

        // Verifies that two objects refer to the same instance with formatted message
        public void AreSame(object expected, object actual, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.AreSame(expected, actual, message, parameters);
        }

        // Forces a failed test with a message
        public void Fail(string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.Fail(message);
        }

        // Forces a failed test with formatted message
        public void Fail(string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.Fail(message, parameters);
        }

        // Passes a test with message (used as Success marker)
        public void Success(string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsTrue(true, message);
        }

        // Passes a test with formatted message (used as Success marker)
        public void Success(string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsTrue(true, message, parameters);
        }

        // Marks test as inconclusive with a message
        public void Inconclusive(string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.Inconclusive(message);
        }

        // Marks test as inconclusive with formatted message
        public void Inconclusive(string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.Inconclusive(message, parameters);
        }

        // Verifies that a condition is false with a message
        public void IsFalse(bool condition, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsFalse(condition, message);
        }

        // Verifies that a condition is false with formatted message
        public void IsFalse(bool condition, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsFalse(condition, message, parameters);
        }

        // Verifies that an object is of a given type
        public void IsInstanceOfType(object value, Type expectedType, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsInstanceOfType(value, expectedType, message);
        }

        public void IsInstanceOfType(object value, Type expectedType, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsInstanceOfType(value, expectedType, message, parameters);
        }

        // Verifies that an object is not of a given type
        public void IsNotInstanceOfType(object value, Type wrongType, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsNotInstanceOfType(value, wrongType, message);
        }

        public void IsNotInstanceOfType(object value, Type wrongType, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsNotInstanceOfType(value, wrongType, message, parameters);
        }

        // Verifies that an object is not null
        public void IsNotNull(object value, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsNotNull(value, message);
        }

        public void IsNotNull(object value, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsNotNull(value, message, parameters);
        }

        // Verifies that an object is null
        public void IsNull(object value, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsNull(value, message);
        }

        public void IsNull(object value, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsNull(value, message, parameters);
        }

        // Verifies that a condition is true
        public void IsTrue(bool condition, string message)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsTrue(condition, message);
        }

        public void IsTrue(bool condition, string message, params object[] parameters)
        {
            this.AddCommentToTextContentProperties(message);
            Assert.IsTrue(condition, message, parameters);
        }

        // Verifies that an exception of type T is thrown

        public void ThrowsException<T>(Action action, string message) where T : Exception
        {
            this.AddCommentToTextContentProperties(message);
            ExceptionAssert.Throws<T>(action);
        }


        #endregion

        // Replaces null characters in a string with "\\0"
        private void AddCommentToTextContentProperties(string message)
        {
            TestContextPropertiesHelper.AddCommentToTextContentProperties(message, TestContext);
        }



    }
}
