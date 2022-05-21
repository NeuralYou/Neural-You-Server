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

	public List<NeuralNetwork> ApplyGeneticOperators()
	{
		//Bucket version:
		List<NeuralNetwork>[] clusters = KMeansUtils.Cluster(Elements, 10);
		List<List<NeuralNetwork>> buckets = clusters.ToList();
		
		buckets.Sort((a, b) => 
		{
			float avgA = 0, avgB = 0;
			foreach(NeuralNetwork n in a)
				avgA += n.Fitness;
			avgA /= a.Count;

			foreach(NeuralNetwork n in b)
				avgB += n.Fitness;
			avgB /= b.Count;

			return Math.Sign(avgA - avgB);
		});

		int listCount = 0;
		foreach(List<NeuralNetwork> list in buckets)
		{
			float avg = 0;
			foreach(NeuralNetwork n in list)
			{
				avg += n.Fitness;
			}

			avg /= list.Count;
			Console.WriteLine($"list {listCount++} average fitness: {avg}");
		}

		buckets = BucketSelect(buckets);

		for(int i = 0; i < buckets.Count; i++)
		{
			buckets[i] = Mutate(buckets[i], i + 1);
		}

		for(int i = 0; i < buckets.Count; i++)
		{
			buckets[i] = ResetFitness(buckets[i]);
		}

		List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();
		foreach(List<NeuralNetwork> bucket in buckets)
		{
			newGeneration.AddRange(bucket);
		}

		return newGeneration;




		//No Bucket version:
		// List<NeuralNetwork> elitists = Elitists(Elements, 0.1f);
		// List<NeuralNetwork> newPopulation = Select(Elements);

		// //Temporarily disabled crossover operator
		// //newPopulation = Crossover(newPopulation);
		// newPopulation = Mutate(newPopulation);
		// newPopulation.AddRange(elitists);
		// newPopulation = ResetFitness(newPopulation);
		// Elements = newPopulation;
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


	public List<List<NeuralNetwork>> BucketSelect(List<List<NeuralNetwork>> oldGeneration)
	{
		int bucketSize = m_PopulationSize / oldGeneration.Count;
		List<List<NeuralNetwork>> newGeneration = new List<List<NeuralNetwork>>();
		for(int i = 0; i < oldGeneration.Count; i++)
		{
			Console.WriteLine($"bucket number: {i}\t elements: {oldGeneration[i].Count}");
		}

		for(int i = 0; i < oldGeneration.Count; i++)
		{
			newGeneration.Add(Select(oldGeneration[i], bucketSize));
		}

		return newGeneration;
	}

	public List<NeuralNetwork> Select(List<NeuralNetwork> oldBucket, int bucketSize)
	{
		List<NeuralNetwork> newBucket = new List<NeuralNetwork>();

		for(int i = 0; i < bucketSize; i++)
		{
			NeuralNetwork winner = RandomUtils.Tournement(oldBucket, 3);
			newBucket.Add(winner.Clone());
		}

		return newBucket;
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

	public List<NeuralNetwork> Mutate(List<NeuralNetwork> bucket, float bucketMutationChance)
	{

		for(int i = 0; i < bucket.Count; i++)
		{
			bucket[i].MutateNetwork(m_MutationRate);// * 1/bucketMutationChance);
		}

		return bucket;
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