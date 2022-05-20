using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class FolderUtils
{
	public static void StorePopulation(Population population, string path)
	{
		//GenerateMetaData(population, path);

		string[] elements = population.SerializeNetworks();
		int num = 1;
		foreach (string e in elements)
		{
			string fileName = "network " + (num++) + ".json";
			string fullPath = Path.Combine(path, fileName);
			StreamWriter write = new StreamWriter(fullPath);
			write.Write(e);
			write.Close();
		}

	}

	public static void GenerateMetaData(Population pop, string path)
	{
		float highestFitness = pop.GetFittest().Fitness;
		float averageFitness = 0;

		string fileName = "Population data.txt";
		string date = DateTime.Now.ToString();

		foreach (NeuralNetwork n in pop.Elements)
		{
			averageFitness += n.Fitness;
		}

		averageFitness /= pop.Size();
		string fullPath = Path.Combine(path, fileName);
		StreamWriter write = new StreamWriter(fullPath);
		write.Write("Date: " + date + "\n");
		write.Write("Elements in generation: " + pop.Size() + "\n");
		write.Write("Highest Fitness: " + highestFitness + "\n");
		write.Write("Average Fitness: " + averageFitness + "\n");
		write.Close();
	}

	//***************PATH IS HARD CODED!!!!!*******************//
	//***************PATH IS HARD CODED!!!!!*******************//
	public static string GetPathToCurrentFolder(bool createNew = true)
	{
		string workingDirectory = Environment.CurrentDirectory;
		string path = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

		path = Path.Combine(path, "NeuralYou Data");
		Directory.CreateDirectory(path);
		string[] files = Directory.GetDirectories(path);
		string today = DateTime.Today.ToShortDateString();
		today = today.Replace("/", ".");
		string s = Directory.GetDirectories(path).SingleOrDefault(file => file.Contains(today));

		if (s is null)
			s = Directory.CreateDirectory(path + @"\" + today).FullName;

		int amountOfFiles = Directory.GetDirectories(s).Length - 1;
		amountOfFiles = createNew ? amountOfFiles + 1: amountOfFiles;

		string pathToCurrent = Directory.CreateDirectory(s + @"\" + amountOfFiles).FullName;

		return pathToCurrent;
	}

	public static void StoreBest(NeuralNetwork element)
	{
		NeuralNetwork currentBest = LoadBest();
		if(currentBest == null || currentBest.Fitness < element.Fitness)
		{
			string fullPath = GetPathToBest();
			StreamWriter writer = new StreamWriter(fullPath);
			string json = JsonConvert.SerializeObject(element, Formatting.Indented);
			writer.Write(json);
			writer.Close();					
		}
	}

	public static NeuralNetwork LoadBest()
	{
		try
		{
			string path = GetPathToBest();
			StreamReader reader = new StreamReader(path);
			string json = reader.ReadToEnd();
			NeuralNetwork best = JsonConvert.DeserializeObject<NeuralNetwork>(json);
			reader.Close();
			return best;
		}

		catch(FileNotFoundException)
		{
			Console.WriteLine("File not found! returning null..");
			return null;	
		}
	}

	public static string GetPathToBest()
	{
		string path = GetPathToCurrentFolder(false);
		string best = "Best";

		string fullPath = Directory.CreateDirectory(Path.Combine(path, best)).FullName;
		string fileName = "Best.json";
		fullPath = Path.Combine(fullPath, fileName);
		return fullPath;
	}
}
