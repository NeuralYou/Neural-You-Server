using System;
using System.Linq;
using System.Collections.Generic;

public static class KMeansUtils
{
    //**************************FITNESS-WISE CLUSTERING******************************************//
    
    public static List<List<NeuralNetwork>> ClusterByFitness(List<NeuralNetwork> elements, int k)
    {
        List<List<NeuralNetwork>> clusters = createInitialClusters(elements, k);

        
        foreach(NeuralNetwork n in elements)
        {
            int closestCenterIndex = 0;
            for(int i = 0; i < clusters.Count; i++)
            {
                if(distanceFitness(clusters[i][0].Fitness, n) < distanceFitness(clusters[closestCenterIndex][0].Fitness, n))
                    closestCenterIndex = i;
            }
            clusters[closestCenterIndex].Add(n);
        }

        return clusters;
    }
    private static List<List<NeuralNetwork>> createInitialClusters(List<NeuralNetwork> elements, int k)
    {
        List<List<NeuralNetwork>> clusters = new List<List<NeuralNetwork>>();
        float min = elements[0].Fitness, max = elements[0].Fitness;
        foreach(NeuralNetwork element in elements)
        {
            if(element.Fitness < min)
            {
                min = element.Fitness;
            }

            else if(element.Fitness > max)
            {
                max = element.Fitness;
            }
        }

        List<NeuralNetwork> copy = new List<NeuralNetwork>(elements);
        for(int i = 0; i < k; i++)
        {
            NeuralNetwork chosen = RandomUtils.RandomElement(copy);
            clusters.Add(new List<NeuralNetwork>());
            clusters[i].Add(chosen);
            copy.Remove(chosen);
        }

        return clusters;
    } 
    
    private static float distanceFitness(float center, NeuralNetwork element2)
    {
        return Math.Abs(center - element2.Fitness);
    }






    //**************************BEHAVIOURAL CLUSTERING******************************************//
    //**************************BEHAVIOURAL CLUSTERING******************************************//
    //**************************BEHAVIOURAL CLUSTERING******************************************//
    // public static List<List<NeuralNetwork>> ClusterByBehaviour(List<NeuralNetwork> elements, int k)
    // {

    //     //Finding the proper dimensions of the genome space

    //     float[][] centerPoints = altCreateInitialCentersBehaviour(elements, k);
     
    //     // int numberOfIterations = 1;
    //     List<List<NeuralNetwork>> clusters = null;

    //     // for (int i = 0; i < numberOfIterations; i++)
    //     {
    //         clusters = GenerateClustersBehaviour(elements, k, centerPoints);
    //         // centerPoints = recreateCenterPoints(clusters);           
    //     }

    //     return clusters;
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

    // private static List<List<NeuralNetwork>> GenerateClustersBehaviour(List<NeuralNetwork> elements, int k, float[][] centerPoints)
    // {
    //     List<List<NeuralNetwork>> clusters = new List<List<NeuralNetwork>>();
    //     for(int i = 0; i < k; i++)
    //     {
    //         clusters.Add(new List<NeuralNetwork>());
    //     }


    //     foreach (NeuralNetwork n in elements)
    //     {
    //         int closestCenterIndex = 0;
    //         for (int i = 0; i < centerPoints.Length; i++)
    //         {
    //             if (distanceBehaviour(n.Genome, centerPoints[closestCenterIndex]) < distanceBehaviour(n.Genome, centerPoints[i]))
    //             {
    //                 closestCenterIndex = i;
    //             }
    //         }

    //         clusters[closestCenterIndex].Add(n);
    //     }

    //     return clusters;
    // }

    // private static float[][] altCreateInitialCentersBehaviour(List<NeuralNetwork> elements, int k)
    // {   
    //     float[][] centers = new float[k][];

    //     List<NeuralNetwork> copy = new List<NeuralNetwork>(elements);
    //     for(int i = 0; i < k; i++)
    //     {
    //         NeuralNetwork n = RandomUtils.RandomElement(elements);
    //         centers[i] = (float[]) n.Genome.Clone();
    //         copy.Remove(n);
    //     }

    //     return centers.ToArray();
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
    // private static float distanceBehaviour(float[] element1, float[] element2)
	// {
	// 	float[] diffSquaredSums = new float[element1.Length];
	// 	float diffSquaredSum = 0;
	// 	for(int i = 0; i < element1.Length; i++)
	// 	{
	// 		diffSquaredSum += (float) System.Math.Pow(element1[i] - element2[i], 2);
	// 	}

	// 	return (float) System.Math.Sqrt(diffSquaredSum);
	// }
}