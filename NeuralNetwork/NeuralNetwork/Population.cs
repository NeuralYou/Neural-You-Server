using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class Population
{
	List<NeuralNetwork> pop;
	public float m_MutationRate;
	int m_PopulationSize;

	public List<NeuralNetwork> Pop
	{
		get { return pop; }
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
		List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();
		int elitistsAmount = Elitists(newGeneration);
		Select(newGeneration);
		Mutate(elitistsAmount);
		ResetFitness();
	}
	private int Elitists(List<NeuralNetwork> newGeneration)
	{
		pop.Sort();
		int fivePrecent = (int) (pop.Count * 0.05f);
		newGeneration.AddRange(pop.GetRange(pop.Count - fivePrecent, fivePrecent));
		Console.WriteLine("Added " + fivePrecent);
		return fivePrecent;
	}

	public void Select(List<NeuralNetwork> newGeneration)
	{
		for(int i = newGeneration.Count; i < m_PopulationSize; i++)
		{
			newGeneration.Add(ThreeWayTournement());
		}

		pop = newGeneration;
	}

	public void Mutate(int elitistsAmount)
	{
		Random rand = new Random();
		for(int i = elitistsAmount; i < pop.Count; i++)
		{
			if (rand.NextDouble() < m_MutationRate)
			{
				pop[i].MutateNetwork(m_MutationRate);
			}
		}

		for(int i = elitistsAmount; i < pop.Count; i++ )
		{
			pop[i].MutateNetwork(m_MutationRate);
		}
	}
	private void ResetFitness()
	{
		foreach(NeuralNetwork n in Pop)
		{
			n.Fitness = 0;
		}
	}

	public string[] SerializeAll()
	{ 
		List<string> list = new List<string>();

		foreach(NeuralNetwork n in pop)
		{
			string json = JsonConvert.SerializeObject(n, Formatting.Indented);
			list.Add(json);
		}

		return list.ToArray();
	}

	private NeuralNetwork TwoWayTournement()
	{
		NeuralNetwork p1 = pop[Utils.RandomRange(0, pop.Count)];
		NeuralNetwork p2 = pop[Utils.RandomRange(0, pop.Count)];
		return Fitter(p1, p2);
	}
	private NeuralNetwork ThreeWayTournement()
	{
		NeuralNetwork p1 = pop[Utils.RandomRange(0, pop.Count)];
		NeuralNetwork p2 = pop[Utils.RandomRange(0, pop.Count)];
		NeuralNetwork p3 = pop[Utils.RandomRange(0, pop.Count)];

		return Fitter(Fitter(p1, p2), p3);
	}

	private NeuralNetwork Fitter(NeuralNetwork i_NeuralNetwork1, NeuralNetwork i_NeuralNetwork2)
	{
		return (i_NeuralNetwork1.Fitness > i_NeuralNetwork2.Fitness ? i_NeuralNetwork1 : i_NeuralNetwork2);
	}

	public NeuralNetwork GetFittest()
	{
		NeuralNetwork max = pop[0];
		foreach(NeuralNetwork p in pop)
		{
			if (p.Fitness > max.Fitness)
			{
				max = p;
			}
		}

		return max;
	}
}