// <file>
//	 <copyright see="prj:///doc/copyright.txt"/>
//	 <license see="prj:///doc/license.txt"/>
//	 <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//	 <version>$Revision: 2555 $</version>
// </file>

using System;
using NUnit.Core;

namespace NUnit.ConsoleRunner
{
	/// <summary>
	/// Filters tests based on the namespace they are in.
	/// </summary>
	[Serializable]
	public class NamespaceFilter : TestFilter
	{
		string testCaseNamespacePrefix;
		string ns;
		
		/// <summary>
		/// Creates a new instance of the NamespaceFilter.
		/// </summary>
		/// <param name="ns">The root namespace that a test
		/// has to belong to in order to be run.</param>
		public NamespaceFilter(string ns)
		{
			this.ns = ns;
			testCaseNamespacePrefix = String.Concat(ns, ".");
		}
		
		public override bool Pass(ITest test)
		{
			return Match(test);
		}
		
		/// <summary>
		/// Matches only the specified test against the namespace filter.
		/// </summary>
		public override bool Match(ITest test)
		{
			TestCase testCase = test as TestCase;
			TestFixture testFixture = test as TestFixture;
			TestSuite testSuite = test as TestSuite;
			if (testCase != null) {
				return Pass(testCase);
			} else if (testFixture != null) {
				return Pass(testFixture);
			} else if (testSuite != null) {
				// Always allow test suites.
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Only those tests that are in the namespace defined by 
		/// this filter are passed.
		/// </summary>
		bool Pass(Test test)
		{
			TestName testName = test.TestName;
			if (testName.FullName != null) {
				return testName.FullName.StartsWith(testCaseNamespacePrefix);
			}
			return false;
		}		
	}
}
