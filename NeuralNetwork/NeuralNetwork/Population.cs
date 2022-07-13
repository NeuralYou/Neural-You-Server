using System;
using System.Linq;
using Newtonsoft.Json;
using static EvolutionUtils;
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
		init { pop = value; }
	}

	public Population(float i_MutationRate, int i_PopulationSize, int i_Inputs, int i_Outputs)
	{
		m_MutationRate = i_MutationRate;
		m_PopulationSize = i_PopulationSize;

		Elements = new List<NeuralNetwork>();
		for(int i = 0; i < m_PopulationSize; i++)
		{
			NeuralNetwork n = new NeuralNetwork(i_Inputs, i_Outputs);
			Elements.Add(n);
		}
	}

	public Population(NeuralNetwork[] networks, float i_MutationRate)
	{
		Elements = new List<NeuralNetwork>(networks);
		m_PopulationSize = Elements.Count;
		m_MutationRate = i_MutationRate;
	}

	public int Size()
	{
		return Elements.Count;
	}

	public List<NeuralNetwork> ApplyGeneticOperators()
    {
        //Clustering
        List<NeuralNetwork> newGeneration = BucketOperators(Elements);

        return newGeneration;
    }

    public string[] SerializeNetworks()
	{ 
		List<string> list = new List<string>();

		foreach(NeuralNetwork n in Elements)
		{
			string json = JsonConvert.SerializeObject(n, Formatting.Indented);
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
		NeuralNetwork best = Elements[0];
		foreach(NeuralNetwork element in Elements)
		{
			if(element.Fitness > best.Fitness)
			{
				best = element;
			}
		}

		return best;
	}

	public static List<NeuralNetwork> GenerateNetworks(int amount, int numberOfInputs, int numberOfOutputs)
	{
		List<NeuralNetwork> networks = new List<NeuralNetwork>();
		for(int i = 0; i < amount; i++)
		{
			NeuralNetwork n = new NeuralNetwork(numberOfInputs, numberOfOutputs);
			networks.Add(n);
		}

		return networks;
	}
}