using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Website.Domain.Shared.Extensions;

namespace Website.Domain.Tests.Shared.Extensions
{
    [TestFixture]
    public class HtmlExtensionsTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        private static IEnumerable ToDateTestCases()
        {
            yield return new TestCaseData(new DateTime(1981, 5, 7)).Returns("7th May 1981").SetName("My birthday all nicely formatted with ordinal");
            yield return new TestCaseData(null).Returns(null).SetName("No date return null");
        }

        [TestCaseSource(typeof(HtmlExtensionsTests), "ToDateTestCases")]
        public string ToDate(DateTime? date)
        {
            return HtmlExtensions.ToDate(null, date);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
