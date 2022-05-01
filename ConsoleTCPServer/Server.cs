using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace ConsoleTCPServer
{
	class Server
	{
		float mutationRate;
		string path;

		public Server()
		{
			mutationRate = 0.05f;
			path = FolderUtils.GetPathToCurrentFolder();
			Console.WriteLine(path);

		}

		private void initPopulation(NetworkStream stream)
		{
			Population pop = new Population(mutationRate, 100, 5, 1);
			sendInitialResponse(pop, stream);
		}

		public void Run()
		{
			IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
			TcpListener listener = new TcpListener(ipAddress, 1234);
			listener.Start();
			bool done = false;
			
			while (!done)
			{
				try
				{
					Console.WriteLine("Recieving..");
					TcpClient client = listener.AcceptTcpClient();
					NetworkStream stream = client.GetStream();

					RequestType type = (RequestType)Enum.Parse(typeof(RequestType), NetworkUtils.ReadInt(stream).ToString());

					Console.WriteLine($"Got Request: {type}");
					if (type.Equals(RequestType.INIT))
					{
						initPopulation(stream);
						continue;
					}

					Population population = ParsePopulation(stream);
					Console.WriteLine($"Recived {population.Size()} networks");
					Console.WriteLine($"Processed population");

					FolderUtils.StorePopulation(population, path);

					ApplyGeneticOperators(population);

					sendResponse(population, stream);
					Console.WriteLine("Sent Response\n\n");
				}
				catch(Exception e)
				{
					Console.WriteLine(e.Message);
					Console.WriteLine(e.StackTrace);
					Console.WriteLine("Exception caught. better be more careful!");
				}
			}
		}

		private Population ApplyGeneticOperators(Population population)
		{
			population.ApplyGeneticOperators();
			return population;
		}

		private Population ParsePopulation(NetworkStream stream)
		{
				List<NeuralNetwork> list = new List<NeuralNetwork>();
				int numberOfElements = NetworkUtils.ReadInt(stream);

				for (int i = 0; i < numberOfElements; i++)
				{
					NeuralNetwork n = ProcessIndividual(stream);
					list.Add(n);
				}
				return new Population(list.ToArray(), mutationRate);
		}

		private void sendResponse(Population pop, NetworkStream stream)
		{
			NetworkUtils.WriteInt(stream, pop.Size());

			foreach(NeuralNetwork p in pop.Elements)
			{
				NetworkUtils.WriteNN(stream, p);
			}

			stream.Close();
		}

		public NeuralNetwork ProcessIndividual(NetworkStream stream)
		{
			int length = NetworkUtils.ReadInt(stream);
			NeuralNetwork network = NetworkUtils.ReadNN(stream, length);
			return network;
		}

		private void sendInitialResponse(Population pop, NetworkStream stream)
		{
			NetworkUtils.WriteInt(stream, pop.Size());

			foreach (NeuralNetwork p in pop.Elements)
			{
				NetworkUtils.WriteNN(stream, p);
			}
		}
	}
}
