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
		string path;
		float mutationRate;
		public Server()
		{
			mutationRate = 0.2f;
			//path = GetPathToCurrentFolder();
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

					StorePopulation(population);
					Console.WriteLine("Stored population in " + path);

					ApplyGeneticOperators(population);

					sendResponse(population, stream);
					Console.WriteLine("Sent Response\n\n");
				}
				catch(Exception)
				{
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

				//float mutationRate = NetworkUtils.ReadFloat(stream);
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

			string[] stringReps = pop.SerializeAll();

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

		public void StorePopulation(Population pop)
		{
			GenerateMetaData(pop);

			string[] elements = pop.SerializeAll();
			int num = 1;
			foreach (string e in elements)
			{
				string fileName = "network " + (num++) + ".json";
				string fullPath = Path.Combine(path, fileName);
				StreamWriter write = new StreamWriter(fullPath);
				write.Write(e);
				write.Close();
			}

		}

		public void GenerateMetaData(Population pop)
		{
			float highestFitness = pop.GetFittest().Fitness;
			float averageFitness = 0;

			string fileName = "Population data.txt";
			string date = DateTime.Now.ToString();

			foreach (NeuralNetwork n in pop.Elements)
			{
				averageFitness += n.Fitness;
			}

			averageFitness /= pop.Size();
			string fullPath = Path.Combine(path, fileName);
			StreamWriter write = new StreamWriter(fullPath);
			write.Write("Date: " + date + "\n");
			write.Write("Elements in generation: " + pop.Size()+ "\n");
			write.Write("Highest Fitness: " + highestFitness + "\n");
			write.Write("Average Fitness: " + averageFitness + "\n");
			write.Close();
		}

		private static string GetPathToCurrentFolder()
		{
			string path = @"C:\Users\Guy\Documents\NeuralYou Data\";

			string[] files = Directory.GetDirectories(path);
			bool todayExists = false;
			string today = DateTime.Today.ToShortDateString();
			today = today.Replace("/", ".");
			string s = Directory.GetDirectories(path).SingleOrDefault(file => file.Contains(today));

			if (s is null)
				s = Directory.CreateDirectory(path + @"\" + today).FullName;

			int amountOfFiles = Directory.GetDirectories(s).Length;

			string pathToCurrent = Directory.CreateDirectory(s + @"\" + amountOfFiles).FullName;

			return pathToCurrent;
		}

		private void sendInitialResponse(Population pop, NetworkStream stream)
		{
			NetworkUtils.WriteInt(stream, pop.Size());
			string[] stringReps = pop.SerializeAll();
			foreach (NeuralNetwork p in pop.Elements)
			{
				NetworkUtils.WriteNN(stream, p);
			}
		}
	}
}
