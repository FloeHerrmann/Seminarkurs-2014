using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator( ConfigFile = "Log4Net.config" , Watch = true )]

namespace Seminarkurs2014Console {
	class Program {

		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		static void Main ( string[] args ) {

			logger.Info( "- - - - - - - - - - - - - - - - " );
			logger.Info( "Seminarkurs 2014 Konsolenanwendung" );

			Console.ReadKey();

		}
	}
}
