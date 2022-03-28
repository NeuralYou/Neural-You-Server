using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ConsoleTCPServer
{
	class Server
	{

		public void Run()
		{
			IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			TcpListener listener = new TcpListener(ipAddress, 1234);
			listener.Start();
			bool done = false;
			
			while (!done)
			{
				Console.WriteLine("Recieving..");
				TcpClient client = listener.AcceptTcpClient();
				NetworkStream stream = client.GetStream();
				NeuralNetwork[] networks = processInput(stream, out float mutationRate);
				Console.WriteLine($"Recived {networks.Length} networks");
				Population pop = processPopulation(mutationRate, networks);
				Console.WriteLine($"Processed population");
				sendResponse(pop, stream);
				Console.WriteLine("Sent Response\n\n");
			}
		}

		private NeuralNetwork[] processInput(NetworkStream stream, out float mutationRate)
		{
			try
			{
				List<NeuralNetwork> list = new List<NeuralNetwork>();

				mutationRate = NetworkUtils.ReadFloat(stream);
				int numberOfElements = NetworkUtils.ReadInt(stream);

				for (int i = 0; i < numberOfElements; i++)
				{
					NeuralNetwork n = ProcessIndividual(stream);
					list.Add(n);
				}
				return list.ToArray();
			}

			catch (Exception e)
			{
				Console.WriteLine(e);
				mutationRate = 0;
				return null;
			}
		}


		private void sendResponse(Population pop, NetworkStream stream)
		{
			NetworkUtils.WriteFloat(stream, pop.m_MutationRate);
			NetworkUtils.WriteInt(stream, pop.Size());

			string[] stringReps = pop.SerializeAll();

			foreach(NeuralNetwork p in pop.Pop)
			{
				NetworkUtils.WriteNN(stream, p);
			}

			stream.Close();
		}

		private Population processPopulation(float mutationRate, NeuralNetwork[] networks)
		{
			Population pop = new Population(networks, mutationRate);
			pop.ApplyGeneticOperators();
			Console.WriteLine("Applied genetic operators");
			return pop;
		}

		public NeuralNetwork ProcessIndividual(NetworkStream stream)
		{
			int length = NetworkUtils.ReadInt(stream);
			NeuralNetwork network = NetworkUtils.ReadNN(stream, length);

			return network;

		}
	}
}
