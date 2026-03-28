/*
 * Created by SharpDevelop.
 * User: Daniel Grunwald
 * Date: 03.02.2006
 * Time: 23:13
 */

using System;
using Codeblast;

namespace NUnit.ConsoleRunner
{
	public class ExtendedConsoleOptions : ConsoleOptions
	{
		public ExtendedConsoleOptions(string[] args) : base(args) {}
		
		[Option(Description="Test method to run")]
		public string testMethodName;
		
		public bool HasTestMethodName
		{
			get
			{
				return (testMethodName != null) && (testMethodName.Length != 0);
			}
		}
		
		[Option(Description="File to receive test results as each test is run")]
		public string results;
		
		public bool IsResults
		{
			get 
			{
				return (results != null) && (results.Length != 0);
			}
		}
				
		[Option(Description="Namespace that tests must belong to in order to be run")]
		public string namespaceFilter;
		
		public bool HasNamespaceFilter
		{
			get 
			{
				return (namespaceFilter != null) && (namespaceFilter.Length != 0);
			}
		}
	}
}
