using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace ConsoleTCPServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Server server = new Server();
			server.Run();
		}


	}
}
