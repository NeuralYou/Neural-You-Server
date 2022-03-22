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

	public void Mutate()
	{

		foreach(NeuralNetwork p in pop)
		{
			p.MutateNetwork(0.01f);
		}
	}

	public void ApplyGeneticOperators()
	{
		Select();
		Mutate();
	}

	public void Select()
	{
		List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();
		for(int i = 0; i < m_PopulationSize; i++)
		{
			newGeneration.Add(ThreeWayTournement());
		}

		pop = newGeneration;
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

	public string[] SerializeAll()
	{
		int num = 1;
		string path = @"C:\Users\Guy\Desktop\jsons\";

		List<string> list = new List<string>();
		foreach(NeuralNetwork n in pop)
		{
			string json = JsonConvert.SerializeObject(n, Formatting.Indented);
			list.Add(json);
			string fileName = "network " + num++ +".txt";
			StreamWriter write = new StreamWriter(path + fileName);
			write.Write(json);
			write.Close();
			//string json = JsonConvert.SerializeObject(n, Formatting.Indented);
			//list.Add(JsonUtility.ToJson(n));
		}

		

		return list.ToArray();
	}
}