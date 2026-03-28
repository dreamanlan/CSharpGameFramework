// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org/?p=license&r=2.4.
// ****************************************************************

// This version of NUnit-console is modified to support:
// 1) Running single test methods using the "testMethodName" command line argument.
// 2) Running all tests in a particular namespace.
// 3) Writing all tests results to a file as the test results are known.

namespace NUnit.ConsoleRunner
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Resources;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Diagnostics;
	using System.Runtime.InteropServices;
	using NUnit.Core;
	using NUnit.Core.Filters;
	using NUnit.Util;
	
	/// <summary>
	/// Summary description for ConsoleUi.
	/// </summary>
	public class ExtendedConsoleUi
	{
		[STAThread]
		public static int Main(string[] args)
		{
			ExtendedConsoleOptions options = new ExtendedConsoleOptions(args);
			
			if(!options.nologo)
				WriteCopyright();

			if(options.help)
			{
				options.Help();
				return 0;
			}
			
			if(options.NoArgs)
			{
				Console.Error.WriteLine("fatal error: no inputs specified");
				options.Help();
				return 0;
			}
			
			if(!options.Validate())
			{
				Console.Error.WriteLine("fatal error: invalid arguments");
				options.Help();
				return 2;
			}


			// Add Standard Services to ServiceManager
			ServiceManager.Services.AddService( new SettingsService() );
			ServiceManager.Services.AddService( new DomainManager() );
			//ServiceManager.Services.AddService( new RecentFilesService() );
			//ServiceManager.Services.AddService( new TestLoader() );
			ServiceManager.Services.AddService( new AddinRegistry() );
			ServiceManager.Services.AddService( new AddinManager() );

			// Initialize Services
			ServiceManager.Services.InitializeServices();

			try
			{
				ExtendedConsoleUi consoleUi = new ExtendedConsoleUi();
				return consoleUi.Execute( options );
			}
			catch( FileNotFoundException ex )
			{
				Console.WriteLine( ex.Message );
				return 2;
			}
			catch( BadImageFormatException ex )
			{
				Console.WriteLine( ex.Message );
				return 2;
			}
			catch( Exception ex )
			{
				Console.WriteLine( "Unhandled Exception:\n{0}", ex.ToString() );
				return 2;
			}
			finally
			{
				if(options.wait)
				{
					Console.Out.WriteLine("\nHit <enter> key to continue");
					Console.ReadLine();
				}
			}
		}

		private static XmlTextReader GetTransformReader(ConsoleOptions parser)
		{
			return (XmlTextReader)typeof(ConsoleUi).InvokeMember("GetTransformReader",
																 BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
																 null, null, new object[] { parser });
		}

		private static void WriteCopyright()
		{
			typeof(ConsoleUi).InvokeMember("WriteCopyright",
										   BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
										   null, null, null);
		}

		private static TestRunner MakeRunnerFromCommandLine( ConsoleOptions options )
		{
			return (TestRunner)typeof(ConsoleUi).InvokeMember("MakeRunnerFromCommandLine",
															  BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
															  null, null, new object[] { options });

		}
		
		public ExtendedConsoleUi()
		{
		}

		public int Execute( ExtendedConsoleOptions options )
		{
			XmlTextReader transformReader = GetTransformReader(options);
			if(transformReader == null) return 3;

			TextWriter outWriter = Console.Out;
			if ( options.isOut )
			{
				StreamWriter outStreamWriter = new StreamWriter( options.output );
				outStreamWriter.AutoFlush = true;
				outWriter = outStreamWriter;
			}

			TextWriter errorWriter = Console.Error;
			if ( options.isErr )
			{
				StreamWriter errorStreamWriter = new StreamWriter( options.err );
				errorStreamWriter.AutoFlush = true;
				errorWriter = errorStreamWriter;
			}
			
			TextWriter testResultWriter = null;
			if ( options.IsResults )
			{
				testResultWriter = new StreamWriter ( options.results, false, Encoding.UTF8 );
				((StreamWriter)testResultWriter).AutoFlush = true;
			}

			TestRunner testRunner = MakeRunnerFromCommandLine( options );

			try
			{
				if (testRunner.Test == null)
				{
					testRunner.Unload();
					Console.Error.WriteLine("Unable to locate fixture {0}", options.fixture);
					return 2;
				}

				EventCollector collector = new EventCollector( options, outWriter, errorWriter, testResultWriter );

				TestFilter catFilter = TestFilter.Empty;
				
				if (options.HasInclude)
				{
					Console.WriteLine( "Included categories: " + options.include );
					catFilter = new CategoryFilter( options.IncludedCategories );
				}
				
				if ( options.HasExclude )
				{
					Console.WriteLine( "Excluded categories: " + options.exclude );
					TestFilter excludeFilter = new NotFilter( new CategoryFilter( options.ExcludedCategories ) );
					catFilter = AndFilter( catFilter, excludeFilter );
				}
				
				if ( options.HasNamespaceFilter )
				{
					Console.WriteLine( "Namespace filter: " + options.namespaceFilter );
					TestFilter namespaceFilter = GetNamespaceFilter( testRunner, options.namespaceFilter );
					catFilter = AndFilter( catFilter, namespaceFilter );
				}
				
				if ( options.HasTestMethodName || options.IsFixture )
				{
					SimpleNameFilter nameFilter = new SimpleNameFilter();
					if ( options.HasTestMethodName )
						nameFilter.Add( options.testMethodName );
					else
						nameFilter.Add( options.fixture );
					catFilter = AndFilter( catFilter, nameFilter );
				}

				TestResult result = null;
				string savedDirectory = Environment.CurrentDirectory;
				TextWriter savedOut = Console.Out;
				TextWriter savedError = Console.Error;

				try
				{
					result = testRunner.Run( collector, catFilter );
				}
				finally
				{
					outWriter.Flush();
					errorWriter.Flush();

					if ( options.isOut )
						outWriter.Close();
					if ( options.isErr )
						errorWriter.Close();
					if ( options.IsResults )
						testResultWriter.Close();

					Environment.CurrentDirectory = savedDirectory;
					Console.SetOut( savedOut );
					Console.SetError( savedError );
				}

				Console.WriteLine();

				string xmlOutput = CreateXmlOutput( result );
				
				if (options.xmlConsole)
				{
					Console.WriteLine(xmlOutput);
				}
				else
				{
					try
					{
						//CreateSummaryDocument(xmlOutput, transformReader );
						XmlResultTransform xform = new XmlResultTransform( transformReader );
						xform.Transform( new StringReader( xmlOutput ), Console.Out );
					}
					catch( Exception ex )
					{
						Console.WriteLine( "Error: {0}", ex.Message );
						return 3;
					}
				}

				// Write xml output here
				string xmlResultFile = options.IsXml ? options.xml : "TestResult.xml";

				using ( StreamWriter writer = new StreamWriter( xmlResultFile ) )
				{
					writer.Write(xmlOutput);
				}

				//if ( testRunner != null )
				//	testRunner.Unload();

				if ( collector.HasExceptions )
				{
					collector.WriteExceptions();
					return 2;
				}
				
				return result.IsFailure ? 1 : 0;
			}
			finally
			{
				testRunner.Unload();
			}
		}

		private string CreateXmlOutput( TestResult result )
		{
			StringBuilder builder = new StringBuilder();
			XmlResultVisitor resultVisitor = new XmlResultVisitor(new StringWriter( builder ), result);
			result.Accept(resultVisitor);
			resultVisitor.Write();

			return builder.ToString();
		}
		
		private TestFilter GetNamespaceFilter( TestRunner testRunner, string filter )
		{
			TestDomain testDomain = (TestDomain)testRunner;
			AssemblyResolver assemblyResolver = (AssemblyResolver)testDomain.AppDomain.CreateInstanceFromAndUnwrap(
				typeof(AssemblyResolver).Assembly.CodeBase,
				typeof(AssemblyResolver).FullName);

			// Tell resolver to use the nunit-console assembly in the test domain
			assemblyResolver.AddFile( GetType().Assembly.Location );
			return new NamespaceFilter( filter );
		}
		
		private TestFilter AndFilter( TestFilter existingFilter, TestFilter newFilter )
		{
			if ( existingFilter.IsEmpty )
				existingFilter = newFilter;
			else
				existingFilter = new AndFilter( existingFilter, newFilter );
			return existingFilter;
		}
	}
}

