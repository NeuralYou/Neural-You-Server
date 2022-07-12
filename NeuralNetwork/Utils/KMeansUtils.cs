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
                if(distance(n, clusters[closestCenterIndex][0]) > distance(n, clusters[i][0]))
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

        List<NeuralNetwork> copy = new List<NeuralNetwork>(elements);
        for(int i = 0; i < k; i++)
        {
            NeuralNetwork n = RandomUtils.RandomElement(elements);
            clusters[i].Add(n);
        }

        return clusters;
    }

    private static float[] variance(List<NeuralNetwork>[] clusters)
    {
        float[] variances = new float[clusters.Length];
        
        foreach(List<NeuralNetwork> list in clusters)
        {
            //The point of this function is to find the overall variance of clusters.
            //this is done by calculating the variance of each cluster, then averaging these
            //averages.

            //inside this foreach loop, the variance of a single cluster 
            //will be calculated.

            //each cluster is basically a 2D array of floating point numbers.


            //Sum all values to the the mean of each position.
            float[] currentClusterMeans = new float[variances.Length];
            for(int i = 0; i < list.Count; i++)
            {
                float[] genome = list[i].Genome;
                for(int j = 0 ; j < currentClusterMeans.Length; j++)
                {
                    currentClusterMeans[j] += genome[j];
                }
            }

            //Produce the actual mean values.
            for(int i = 0; i < currentClusterMeans.Length; i++)
            {
                currentClusterMeans[i] /= list.Count;
            }

            //Calculate the overall variance of this cluster

            float clusterVariance = 1;
            foreach(float f in currentClusterMeans)
            {

            }

        }
            return new float[]{};
    }

    private static float distance(float[] element1, float[] element2)
	{
		float[] diffSquaredSums = new float[element1.Length];
		float diffSquaredSum = 0;
		for(int i = 0; i < element1.Length; i++)
		{
			diffSquaredSum += (float) System.Math.Pow(element1[i] - element2[i], 2);
		}

		return (float) System.Math.Sqrt(diffSquaredSum);
	}

    private static float distance(NeuralNetwork element1, NeuralNetwork element2)
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

    public static int SortBuckets(List<NeuralNetwork> a, List<NeuralNetwork> b)
    {
        float avgA = 0, avgB = 0;
        foreach(NeuralNetwork n in a)
            avgA += n.Fitness;
        avgA /= a.Count;

        foreach(NeuralNetwork n in b)
            avgB += n.Fitness;
        avgB /= b.Count;

        return Math.Sign(avgA - avgB);
    }
}