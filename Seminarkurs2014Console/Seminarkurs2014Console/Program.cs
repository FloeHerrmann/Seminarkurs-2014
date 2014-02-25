using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seminarkurs2014Console.database;
using Seminarkurs2014Console.model;

[assembly: log4net.Config.XmlConfigurator( ConfigFile = "Log4Net.config" , Watch = true )]

namespace Seminarkurs2014Console {
	class Program {

		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		static void Main ( string[] args ) {

			logger.Info( "- - - - - - - - - - - - - - - - " );
			logger.Info( "Seminarkurs 2014 Konsolenanwendung" );

			DatabaseFacade database = new DatabaseFacade();
			database.SetDatabaseConnector( new MySQLConnector( "SERVER=localhost;DATABASE=seminarkurs2014;UID=root;PASSWORD=root;" ) );

			ExampleData exampleData = new ExampleData();
			exampleData.InsertExampleData( database );

			Console.ReadKey();

		}
	}
}
