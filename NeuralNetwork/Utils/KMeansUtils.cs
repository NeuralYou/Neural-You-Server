using System;
using System.Linq;
using System.Collections.Generic;

public static class KMeansUtils
{
    public static List<NeuralNetwork>[] Cluster(List<NeuralNetwork> elements, int k)
    {
        List<NeuralNetwork>[] clusters = initClusters(elements, k);
     
        clusters = generateClusters(elements, k, clusters);
        return clusters;
    }

    private static List<NeuralNetwork>[] generateClusters(List<NeuralNetwork> elements, int k, List<NeuralNetwork>[] clusters)
    {

        foreach (NeuralNetwork n in elements)
        {
            int closestCenterIndex = 0;
            for (int i = 0; i < clusters.Length; i++)
            {
                if(distanceBehaviour(n, clusters[closestCenterIndex][0]) > distanceBehaviour(n, clusters[i][0]))
                {
                    closestCenterIndex = i;
                }
            }

            clusters[closestCenterIndex].Add(n);
        }

        return clusters;
    }

    private static List<NeuralNetwork>[] initClusters(List<NeuralNetwork> elements, int k)
    {   
        List<NeuralNetwork>[] clusters = new List<NeuralNetwork>[k];

        clusters = clusters.Select(list => new List<NeuralNetwork>()).ToArray();

        foreach(List<NeuralNetwork> list in clusters)
        {
            Console.WriteLine("list is null? " + list == null);
        }

        List<NeuralNetwork> copy = new List<NeuralNetwork>(elements);
        for(int i = 0; i < k; i++)
        {
            NeuralNetwork n = RandomUtils.RandomElement(elements);
            clusters[i].Add(n);
            Console.WriteLine("Remove successful? " + copy.Remove(n));
        }

        return clusters;
    }

    private static float distanceBehaviour(float[] element1, float[] element2)
	{
		float[] diffSquaredSums = new float[element1.Length];
		float diffSquaredSum = 0;
		for(int i = 0; i < element1.Length; i++)
		{
			diffSquaredSum += (float) System.Math.Pow(element1[i] - element2[i], 2);
		}

		return (float) System.Math.Sqrt(diffSquaredSum);
	}

        private static float distanceBehaviour(NeuralNetwork element1, NeuralNetwork element2)
	{
		float[] diffSquaredSums = new float[element1.Genome.Length];
		float diffSquaredSum = 0;

        float[] genome1 = element1.Genome, genome2 = element2.Genome;

		for(int i = 0; i < genome1.Length; i++)
		{
			diffSquaredSum += (float) System.Math.Pow(genome1[i] - genome2[i], 2);
		}

		return (float) System.Math.Sqrt(diffSquaredSum);
	}
}

  //**************************FITNESS-WISE CLUSTERING******************************************//
  //**************************FITNESS-WISE CLUSTERING******************************************//
  //**************************FITNESS-WISE CLUSTERING******************************************//
    
    // public static List<List<NeuralNetwork>> ClusterByFitness(List<NeuralNetwork> elements, int k)
    // {
    //     List<List<NeuralNetwork>> clusters = createInitialClusters(elements, k);
        
    //     foreach(NeuralNetwork n in elements)
    //     {
    //         int closestCenterIndex = 0;
    //         for(int i = 0; i < clusters.Count; i++)
    //         {
    //             if(distanceFitness(clusters[i][0].Fitness, n) < distanceFitness(clusters[closestCenterIndex][0].Fitness, n))
    //                 closestCenterIndex = i;
    //         }
    //         clusters[closestCenterIndex].Add(n);
    //     }

    //     return clusters;
    // }
    // private static List<List<NeuralNetwork>> createInitialClusters(List<NeuralNetwork> elements, int k)
    // {
    //     List<List<NeuralNetwork>> clusters = new List<List<NeuralNetwork>>();
    //     float min = elements[0].Fitness, max = elements[0].Fitness;
    //     foreach(NeuralNetwork element in elements)
    //     {
    //         if(element.Fitness < min)
    //         {
    //             min = element.Fitness;
    //         }

    //         else if(element.Fitness > max)
    //         {
    //             max = element.Fitness;
    //         }
    //     }

    //     List<NeuralNetwork> copy = new List<NeuralNetwork>(elements);
    //     for(int i = 0; i < k; i++)
    //     {
    //         NeuralNetwork chosen = RandomUtils.RandomElement(copy);
    //         clusters.Add(new List<NeuralNetwork>());
    //         clusters[i].Add(chosen);
    //         Console.WriteLine("new found center: " + clusters[i][0].Fitness);
    //         copy.Remove(chosen);
    //     }

    //     return clusters;
    // } 
    
    // private static float distanceFitness(float center, NeuralNetwork element2)
    // {
    //     return Math.Abs(center - element2.Fitness);
    // }







    // private static float[][] createInitialCentersBehaviour(List<NeuralNetwork> elements, int k)
    // {
    //     float[][] centerPoints = new float[k][];
    //     int genomeLength = elements[0].Genome.Length;
        
    //     for(int i = 0; i < centerPoints.Length; i++)
    //     {
    //         centerPoints[i] = new float[genomeLength];
    //     }

    //     float minWeight = elements[0].Genome[0];
    //     float maxWeight = elements[0].Genome[0];

    //     foreach (NeuralNetwork n in elements)
    //     {
    //         foreach (float f in n.Genome)
    //         {
    //             if (f > maxWeight) maxWeight = f;
    //             if (f < minWeight) minWeight = f;
    //         }
    //     }

    //     //Generating random center points, bounded by minWeight and maxWeight
    //     foreach (float[] point in centerPoints)
    //     {
    //         for (int i = 0; i < point.Length; i++)
    //         {
    //             point[i] = RandomUtils.RandomRange(minWeight, maxWeight);
    //         }
    //     }

    //     return centerPoints;
    // }


    // private static float[][] recreateCenterPointsBehaviour(List<List<NeuralNetwork>> clusters)
    // {
    //     float[][] centerPoints = new float[clusters.Count][];
    //     int genomeLength = clusters.FirstOrDefault(list => list.Count != 0)[0].Genome.Length;

    //     for(int i = 0; i < centerPoints.Length; i++)
    //     {
    //         float[] newCenter = new float[genomeLength];
    //         foreach(NeuralNetwork n in clusters[i])
    //         {
    //             float[] genome = n.Genome;
    //             for(int j = 0; j < newCenter.Length; j++)
    //             {
    //                 newCenter[j] += genome[j];
    //             }
    //         }
    //         for(int j = 0; j < newCenter.Length; j++)
    //         {
    //             if(clusters[i].Count != 0)
    //                 newCenter[j] /= clusters[i].Count;
    //             else
    //                 Console.WriteLine("DIVISION BY ZERO!!!!!!!!!!");
    //         }

    //         centerPoints[i] = newCenter;

    //         Console.Write("[");
    //         foreach(float f in centerPoints[i])
    //         {
    //             Console.Write($"{f}, ");
    //         }
    //         Console.WriteLine("]");
    //     }

    //     return centerPoints;
    // }