using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class Population
{
	List<NeuralNetwork> pop;
	public float m_MutationRate;
	int m_PopulationSize;

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
		List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();
		int elitistsAmount = Elitists(newGeneration);
		Select(newGeneration);
		Mutate(elitistsAmount);
		ResetFitness();
	}
	private int Elitists(List<NeuralNetwork> newGeneration)
	{
		Elements.Sort();
		int fivePrecent = (int) (pop.Count * 0.05f);
		newGeneration.AddRange(Elements.GetRange(Elements.Count - fivePrecent, fivePrecent));
		Console.WriteLine("Added " + fivePrecent);
		return fivePrecent;
	}

	public void Select(List<NeuralNetwork> newGeneration)
	{
		for(int i = newGeneration.Count; i < m_PopulationSize; i++)
		{
			newGeneration.Add(ThreeWayTournement());
		}


		Elements = newGeneration;

		for(int i = 0; i < Elements.Count; i++)
		{
			Elements[i] = Elements[i].Clone();
		}
	}

	public void Mutate(int elitistsAmount)
	{
		Random rand = new Random();
		for(int i = elitistsAmount; i < Elements.Count; i++)
		{
			if (rand.NextDouble() < m_MutationRate)
			{
				Elements[i].MutateNetwork(m_MutationRate);
			}
		}
	}
	private void ResetFitness()
	{
		foreach(NeuralNetwork n in Elements)
		{
			n.Fitness = 0;
		}
	}

	public string[] SerializeAll()
	{ 
		List<string> list = new List<string>();

		foreach(NeuralNetwork n in Elements)
		{
			string json = JsonConvert.SerializeObject(n, Formatting.Indented);
			list.Add(json);
		}

		return list.ToArray();
	}

	private NeuralNetwork TwoWayTournement()
	{
		NeuralNetwork p1 = Elements[Utils.RandomRange(0, Elements.Count)];
		NeuralNetwork p2 = Elements[Utils.RandomRange(0, Elements.Count)];
		return Fitter(p1, p2);
	}
	private NeuralNetwork ThreeWayTournement()
	{
		NeuralNetwork p1 = Elements[Utils.RandomRange(0, Elements.Count)];
		NeuralNetwork p2 = Elements[Utils.RandomRange(0, Elements.Count)];
		NeuralNetwork p3 = Elements[Utils.RandomRange(0, Elements.Count)];

		return Fitter(Fitter(p1, p2), p3);
	}

	private NeuralNetwork Fitter(NeuralNetwork i_NeuralNetwork1, NeuralNetwork i_NeuralNetwork2)
	{
		return (i_NeuralNetwork1.Fitness > i_NeuralNetwork2.Fitness ? i_NeuralNetwork1 : i_NeuralNetwork2);
	}

	public NeuralNetwork GetFittest()
	{
		NeuralNetwork max = Elements[0];
		foreach(NeuralNetwork p in Elements)
		{
			if (p.Fitness > max.Fitness)
			{
				max = p;
			}
		}

		return max;
	}
}