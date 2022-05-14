using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Population
{
	private List<NeuralNetwork> pop;
	public float m_MutationRate;
	int m_PopulationSize;
	public float AverageFitness
	{
		get
		{
			float sum = 0;
			foreach(NeuralNetwork n in Elements)
			{
				sum += n.Fitness;
			}

			return sum / Elements.Count;
		}
	}

	public List<NeuralNetwork> Elements
	{
		get { return pop; }
		private set { pop = value; }
	}

	public Population(float i_MutationRate, int i_PopulationSize, int i_Inputs, int i_Outputs)
	{
		m_MutationRate = i_MutationRate;
		m_PopulationSize = i_PopulationSize;

		initPopulation(i_Inputs, i_Outputs);
	}

	public Population(NeuralNetwork[] networks, float i_MutationRate)
	{
		pop = new List<NeuralNetwork>(networks);
		m_PopulationSize = pop.Count;
		m_MutationRate = i_MutationRate;
	}

	private void initPopulation(int i_Inputs, int i_Outputs)
	{
		pop = new List<NeuralNetwork>();
		for(int i = 0; i < m_PopulationSize; i++)
		{
			NeuralNetwork n = new NeuralNetwork(i_Inputs, i_Outputs);
			pop.Add(n);
		}
	}

	public int Size()
	{
		return pop.Count;
	}

	public void ApplyGeneticOperators()
	{
		List<NeuralNetwork> elitists = Elitists(Elements, 0.1f);

		List<NeuralNetwork> newPopulation = Select(Elements);

		//Temporarily disabled crossover operator
		//newPopulation = Crossover(newPopulation);
		newPopulation = Mutate(newPopulation);

		newPopulation.AddRange(elitists);

		newPopulation = ResetFitness(newPopulation);
		Elements = newPopulation;
	}

	private List<NeuralNetwork> Elitists(List<NeuralNetwork> oldGeneration, float precentage)
	{
		int tenPercent = (int) (oldGeneration.Count * precentage);

		List<NeuralNetwork> temp = new List<NeuralNetwork>(oldGeneration);
		temp.Sort();

		List<NeuralNetwork> elitists = temp.GetRange(temp.Count - tenPercent, tenPercent);

		for(int i = 0; i < elitists.Count; i++)
		{
			elitists[i] = elitists[i].Clone();
		}

		return elitists;
	}

	public List<NeuralNetwork> Select(List<NeuralNetwork> oldGeneration)
	{
		int cloneLimit = 3;
		List<int> cloneCounters = new List<int>(new int[oldGeneration.Count]);
		List<NeuralNetwork> tempPop = new List<NeuralNetwork>(oldGeneration);
		List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();

		for(int i = 10; i < oldGeneration.Count; i++)
		{
			NeuralNetwork winner = RandomUtils.Tournement(tempPop, 3);
			int winnerIndex = tempPop.FindIndex(net => winner.Equals(net));
			cloneCounters[winnerIndex]++;
			if(cloneCounters[winnerIndex] >= cloneLimit)
			{
				tempPop.RemoveAt(winnerIndex);
				cloneCounters.RemoveAt(winnerIndex);
			}

			newGeneration.Add(winner);
		}

		return newGeneration;
	}

	//Crossover type is 1-point crossover
	public List<NeuralNetwork> Crossover(List<NeuralNetwork> newGeneration)
	{
		for(int i = 0; i < newGeneration.Count; i++)
		{
			if(RandomUtils.RollOdds(0.4f))
			{
				NeuralNetwork other = newGeneration[RandomUtils.RandomRange(0, newGeneration.Count)];

				float[] genome1 = newGeneration[i].Genome;
				float[] genome2 = other.Genome;

				int middle = genome1.Length / 2;
				List<float> newGenome1 = new List<float>(genome1[0..middle]);
				newGenome1.AddRange(genome2[middle..]);

				List<float> newGenome2 = new List<float>(genome2[0..middle]);
				newGenome2.AddRange(genome1[middle..]);

				newGeneration[i].Genome = newGenome1.ToArray();
				other.Genome = newGenome2.ToArray();
			}
		}

		return newGeneration;
	}

	public List<NeuralNetwork> Mutate(List<NeuralNetwork> newGeneration)
	{
		float average = AverageFitness;

		for(int i = 0; i < newGeneration.Count; i++)
		{
			if (RandomUtils.RollOdds(m_MutationRate))
			{
				bool aboveAverage = newGeneration[i].Fitness >= average;
				newGeneration[i].MutateNetwork(aboveAverage);
			}
		}

		return newGeneration;
	}
	private List<NeuralNetwork> ResetFitness(List<NeuralNetwork> newGeneration)
	{
		foreach(NeuralNetwork n in newGeneration)
		{
			n.Fitness = 0;
		}

		return newGeneration;
	}

	public string[] SerializeNetworks()
	{ 
		List<string> list = new List<string>();

		foreach(NeuralNetwork n in Elements)
		{
			string json = JsonConvert.SerializeObject(n);
			list.Add(json);
		}

		return list.ToArray();
	}

	private NeuralNetwork Fitter(NeuralNetwork network1, NeuralNetwork network2)
	{
		return (network1.CompareTo(network2) > 0 ? network1 : network2);
	}

	public NeuralNetwork GetFittest()
	{
		List<NeuralNetwork> sorted = new List<NeuralNetwork>(Elements);
		sorted.Sort();
		return sorted.Last();
	}
}