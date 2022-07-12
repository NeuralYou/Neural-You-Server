using System;
using System.Collections.Generic;
using System.Linq;
public static class EvolutionUtils
{
    public static List<NeuralNetwork> Elitists(List<NeuralNetwork> oldGeneration, float precentage)
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

    public static  List<NeuralNetwork> Select(List<NeuralNetwork> oldBucket, int bucketSize)
	{
		List<NeuralNetwork> newBucket = new List<NeuralNetwork>();

		for(int i = 0; i < bucketSize; i++)
		{
			NeuralNetwork winner = RandomUtils.Tournement(oldBucket, 3);
			newBucket.Add(winner.Clone());
		}

		return newBucket;
	}

    public static List<NeuralNetwork> Crossover(List<NeuralNetwork> newGeneration)
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

  	public static List<NeuralNetwork> Mutate(List<NeuralNetwork> bucket, float bucketMutationChance)
	{

		for(int i = 0; i < bucket.Count; i++)
		{
			bucket[i].MutateNetwork(1/bucketMutationChance);
		}

		return bucket;
	}

    public static List<NeuralNetwork> ResetFitness(List<NeuralNetwork> newGeneration)
	{
		foreach(NeuralNetwork n in newGeneration)
		{
			n.Fitness = 0;
		}

		return newGeneration;
	}


    //*******************************SELECTION**********************************//
    //*******************************SELECTION**********************************//
    //*******************************SELECTION**********************************//
    public static List<List<NeuralNetwork>> BucketSelect(List<List<NeuralNetwork>> oldGeneration, List<NeuralNetwork>[] elitists, int populationSize)
	{
		int bucketSize = populationSize / oldGeneration.Count;
		List<List<NeuralNetwork>> newGeneration = new List<List<NeuralNetwork>>();
		for(int i = 0; i < oldGeneration.Count; i++)
		{
			Console.WriteLine($"bucket number: {i}\t elements: {oldGeneration[i].Count}");
		}

		for(int i = 0; i < oldGeneration.Count; i++)
		{
			int eliteReductionSize = elitists[i].Count;
			newGeneration.Add(Select(oldGeneration[i], bucketSize - eliteReductionSize));
		}

		return newGeneration;
	}
}