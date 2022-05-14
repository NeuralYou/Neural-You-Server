using System;
using System.Linq;
using System.Collections.Generic;

public static class KMeansUtils
{
    	public static List<List<NeuralNetwork>> KMeansClustering(List<NeuralNetwork> elements, int k)
    {

        //Finding the proper dimensions of the genome space

        float[][] centerPoints = createInitialCenters(elements, k);
     
        int numberOfIterations = 3;
        List<List<NeuralNetwork>> clusters;
        for (int i = 0; i < numberOfIterations; i++)
        {
            clusters = GenerateClusters(elements, k, centerPoints);
        }
    }

    private static float[][] recreateCenterPoints(List<List<NeuralNetwork>> clusters)
    {
        float[][] centerPoints = new float[clusters.Count][];
        int genomeLength = clusters[0][0].Genome.Length;

        for(int i = 0; i < centerPoints.Length; i++)
        {
            float[] newCenter = new float[genomeLength];
            foreach(NeuralNetwork n in clusters[i])
            {
                float[] genome = n.Genome;
                for(int j = 0; j < newCenter.Length; j++)
                {
                    newCenter[j] += genome[j];
                }
            }
            centerPoints[i] = newCenter;
        }

        return centerPoints;
    }

    private static List<List<NeuralNetwork>> GenerateClusters(List<NeuralNetwork> elements, int k, float[][] centerPoints)
    {
        List<List<NeuralNetwork>> clusters = new List<List<NeuralNetwork>>(k);
        foreach (NeuralNetwork n in elements)
        {
            int closestCenterIndex = 0;
            for (int i = 0; i < centerPoints.Length; i++)
            {
                if (distance(n.Genome, centerPoints[closestCenterIndex]) < distance(n.Genome, centerPoints[i]))
                {
                    closestCenterIndex = i;
                }
            }
            clusters[closestCenterIndex].Add(n);
        }

        return clusters;
    }

    private static float[][] createInitialCenters(List<NeuralNetwork> elements, int k)
    {
        float[][] centerPoints = new float[k][];
        int genomeLength = elements[0].Genome.Length;
        centerPoints = (float[][])centerPoints.Select(point => new float[genomeLength]);
        float minWeight = elements[0].Genome[0];
        float maxWeight = elements[0].Genome[0];

        foreach (NeuralNetwork n in elements)
        {
            foreach (float f in n.Genome)
            {
                if (f > maxWeight) maxWeight = f;
                if (f < minWeight) minWeight = f;
            }
        }

        //Generating random center points, bounded by minWeight and maxWeight
        foreach (float[] point in centerPoints)
        {
            for (int i = 0; i < point.Length; i++)
            {
                point[i] = RandomUtils.RandomRange(minWeight, maxWeight);
            }
        }

        return centerPoints;
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
}