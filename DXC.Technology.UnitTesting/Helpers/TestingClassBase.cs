using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DXC.Technology.UnitTesting.Helpers;
using DXC.Technology.UnitTesting.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            TechnologyUnitTestHelper.HelpTestClassInitialization(UnitTestType, TestClassName, TestContext);
            TechnologyUnitTestHelper.HelpTestInitialization(UnitTestType, TestClassName, TestContext);
        }

        /// <summary>
        /// Cleans up after the test.
        /// </summary>
        /// <param name="">No parameters are required for this method.</param>
        [TestCleanup]
        public void TestCleanup()
        {
            TechnologyUnitTestHelper.HelpTestCleanup(UnitTestType, TestClassName, TestContext);
            TechnologyUnitTestHelper.HelpTestClassCleanup(UnitTestType, TestClassName);
        }

        #endregion
    }
}