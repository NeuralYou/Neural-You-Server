using System;
using System.Collections.Generic;
namespace ConsoleTCPServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Population pop = new Population(0.01f, 100, 3, 5);
			List<List<NeuralNetwork>> clusters = KMeansUtils.KMeansClustering(pop.Elements, 10);

			for(int i = 0; i < clusters.Count; i++)
			{
				Console.WriteLine("Cluster " + i +"*******************");
				Console.WriteLine($"members : {clusters[i].Count}");	
			}
			// Server server = new Server();
			// server.Run();
		}
	}
}
