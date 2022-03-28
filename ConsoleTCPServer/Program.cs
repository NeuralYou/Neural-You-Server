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
