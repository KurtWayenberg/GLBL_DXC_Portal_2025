using DXC.Technology.UnitTesting.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.UnitTesting.TestStatistic
{
    public class TestContextPropertiesHelper
    {
        private const string UnitTestTypeKey = "UnitTestType";
        private const string TestClassNameKey = "TestClassName";
        private const string MethodNameKey = "MethodName";
        private const string CommentKey = "Comment";
        /// <summary>
        /// Adds the type and class name to the text content properties.
        /// </summary>
        /// <param name="unitTestType">The type of unit test.</param>
        /// <param name="testClassName">The name of the test class.</param>
        /// <param name="testContext">The test context.</param>
        public static void AddTypeAndClassNameToTextContentProperties(TechnologyUnitTestTypeEnum unitTestType, string testClassName, TestContext testContext)
        {
            testContext.Properties[UnitTestTypeKey] = unitTestType.ToString();
            testContext.Properties[TestClassNameKey] = testClassName;
            testContext.Properties[MethodNameKey] = testContext.ManagedMethod;
        }
        public static void AddCommentToTextContentProperties(string comment, TestContext testContext)
        {
            testContext.Properties[CommentKey] = comment;
        }
        public static TechnologyUnitTestTypeEnum GetUnitTestType(TestContext testContext)
        {
            if (testContext.Properties.Contains(UnitTestTypeKey))
            {
                return Enum.Parse<TechnologyUnitTestTypeEnum>(testContext.Properties[UnitTestTypeKey]?.ToString()!);
            }
            return TechnologyUnitTestTypeEnum.Unspecified;
        }

        public static string GetTestClassName(TestContext testContext)
        {
            if (testContext.Properties.Contains(TestClassNameKey))
            {
                return testContext.Properties[TestClassNameKey]?.ToString()!;
            }
            return string.Empty;
        }
        public static string GetComment(TestContext testContext)
        {
            if (testContext.Properties.Contains(CommentKey))
            {
                return testContext.Properties[CommentKey]?.ToString()!;
            }
            return string.Empty;
        }

        public static string GetMethodName(TestContext testContext)
        {
            if (testContext.Properties.Contains(MethodNameKey))
            {
                return testContext.Properties[MethodNameKey]?.ToString()!;
            }
            return string.Empty;
        }
    }
}
