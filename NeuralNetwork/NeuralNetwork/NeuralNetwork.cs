using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[System.Serializable]
public class NeuralNetwork : System.IComparable<NeuralNetwork>
{
	[JsonProperty] InputNeuron[] inputs;
	[JsonProperty] HiddenNeuron[] hidden;
	[JsonProperty] OutputNeuron[] outputs;
	float networkThreshold;

	public float Fitness { get; set; }
	[JsonIgnore] public float[] Genome
	{
		get
		{
			List<float> genome = new List<float>();
			
			foreach(InputNeuron input in inputs)
			{
				genome.AddRange(input.outputWeights);
			}

			foreach(HiddenNeuron hid in hidden)
			{
				genome.AddRange(hid.outputWeights);
			}

			return genome.ToArray();
		}

		set
		{
			List<float> genome = new List<float>(Genome);
			if (value.Length != genome.Count)
			{
				Console.WriteLine($"Expected length: {genome.Count}\t recieved count: {value.Length}");
				throw new ArgumentException();
			}

			foreach(InputNeuron input in inputs)
			{
				for(int i = 0; i < input.outputWeights.Length; i++)
				{
					input.outputWeights[i] = genome[0];
					genome.RemoveAt(0);
				}
			}

			foreach(HiddenNeuron hid in hidden)
			{
				for(int i = 0; i < hid.outputWeights.Length; i++)
				{
					hid.outputWeights[i] = genome[0];
					genome.RemoveAt(0);
				}
			}
		}
	}

	public NeuralNetwork(int i_NumberOfInputs, int i_NumberOfOutputs)
	{
		networkThreshold = 0.3f;
		inputs = new InputNeuron[i_NumberOfInputs];
		outputs = new OutputNeuron[i_NumberOfOutputs];

		int numberOfHiddenNeurons = 4;
		// int numberOfHiddenNeurons = (i_NumberOfInputs * 2) + 1;
		hidden = new HiddenNeuron[numberOfHiddenNeurons];
		InitNeurons();
	}


	private void InitNeurons()
	{
		for (int i = 0; i < inputs.Length; i++)
		{
			inputs[i] = new InputNeuron(hidden.Length);
		}

		for (int i = 0; i < hidden.Length; i++)
		{
			hidden[i] = new HiddenNeuron(outputs.Length);
		}

		for(int i = 0; i < outputs.Length; i++)
		{
			outputs[i] = new OutputNeuron();
		}
	}

	public void  FeedForward(float[] i_Inputs)
	{
		FeedInput(i_Inputs);
		FeedInputToHiddens();
		FeedHiddenToOutput();
	}

	public void FeedInput(params float[] i_Inputs)
	{
		for(int i = 0; i < inputs.Length; i++)
		{
			inputs[i].RecieveInput(i_Inputs[i]);
		}
	}


	public void FeedInputToHiddens()
	{
		foreach(InputNeuron input in inputs)
		{
			input.FeedForward(hidden);
			input.Reset();
		}

		foreach(HiddenNeuron hid in hidden)
		{
			hid.Activate(networkThreshold);
		}
	}

	public void FeedHiddenToOutput()
	{
		foreach(HiddenNeuron hid in hidden)
		{
			hid.FeedForward(outputs);
			hid.Reset();
		}

		foreach(OutputNeuron output in outputs)
		{
			output.Activate(networkThreshold);
			output.Reset();
		}
	}
	
	public void MutateNetwork(float mutationRate)
	{
		for(int i = 0; i < hidden.Length; i++)
		{
			if(RandomUtils.RollOdds(mutationRate))
			mutateNode(i);
		}
	}
	
	private void mutateNode(int nodeIndex)
	{
		foreach(InputNeuron n in inputs)
		{
			n.MutateWeight(nodeIndex);
		}
	}

	// public void MutateNode(bool biasedMutation)
	// {
	// 	int index = RandomUtils.RandomRange(0, hidden.Length);
	// 	foreach(InputNeuron n in inputs)
	// 	{
	// 		n.MutateWeight(index, biasedMutation);
	// 	}
	// }

	private List<Neuron> ChooseRandomNodesForMutation()
	{
		Random rand = new Random();
		int numberOfWeights = (int)(inputs.Length * hidden.Length * outputs.Length * 0.05f);

		List<Neuron> neurons = new List<Neuron>();
		for (int i = 0; i < numberOfWeights / 2; i++)
		{
			Neuron n = inputs[rand.Next(inputs.Length)];
			if (!neurons.Contains(n))
				neurons.Add(n);
		}

		for (int i = 0; i < hidden.Length; i++)
		{
			Neuron n = hidden[rand.Next(hidden.Length)];
			if (!neurons.Contains(n))
				neurons.Add(n);
		}

		return neurons;
	}

	public int CompareTo(NeuralNetwork other)
	{
		return (int) (Fitness - other.Fitness);
	}

	public NeuralNetwork Clone()
	{
		NeuralNetwork n = new NeuralNetwork(inputs.Length, outputs.Length);
		n.Fitness = Fitness;
		
		for(int i = 0; i < n.inputs.Length; i++)
		{
			n.inputs[i] = inputs[i].Clone();
		}

		for(int i = 0; i < n.hidden.Length; i++)
		{
			n.hidden[i] = hidden[i].Clone();
		}

		for(int i = 0; i < n.outputs.Length; i++)
		{
			n.outputs[i] = new OutputNeuron(); // outputs[i].Clone();
		}

		return n;
	}

	public bool Equals(NeuralNetwork other)
	{
		for(int i = 0; i < inputs.Length; i++)
		{
			for(int j = 0; j < inputs[i].outputWeights.Length; j++)
			{
				if (inputs[i].outputWeights[j] != other.inputs[i].outputWeights[j])
				{
					return false;
				}
			}

			for(int j = 0; j < hidden[i].outputWeights.Length; j++)
			{
				if(hidden[i].outputWeights[j] != other.hidden[i].outputWeights[j])
				{
					return false;
				}
			}
		}

		return true;
	}
}