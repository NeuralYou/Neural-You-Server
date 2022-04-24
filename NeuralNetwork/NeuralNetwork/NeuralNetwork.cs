using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class NeuralNetwork : System.IEquatable<NeuralNetwork>, System.IComparable<NeuralNetwork>
{
	[JsonProperty] InputNeuron[] inputs;
	[JsonProperty] HiddenNeuron[] hidden;
	[JsonProperty] OutputNeuron[] outputs;
	float networkThershold;
	public float Fitness { get; set; }

	
	public NeuralNetwork(int i_NumberOfInputs, int i_NumberOfOutputs)
	{
		inputs = new InputNeuron[i_NumberOfInputs];
		outputs = new OutputNeuron[i_NumberOfOutputs];

		int numberOfHiddenNeurons = (i_NumberOfInputs * 2) + 1;
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
			hid.Activate(networkThershold);
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
			output.Activate(networkThershold);
			output.Reset();
		}
	}
	
	public void MutateNetwork(float i_MutationRate)
	{
		System.Random rand = new System.Random();
		int numberOfWeights = (int) (inputs.Length * hidden.Length * outputs.Length * 0.05f);
		
		List<Neuron> neurons = new List<Neuron>();
		for(int i = 0; i < numberOfWeights/2; i++)
		{
			Neuron n = inputs[rand.Next(inputs.Length)];
			if (!neurons.Contains(n))
				neurons.Add(n);
		}

		for(int i = 0; i < hidden.Length; i++)
		{
			Neuron n = hidden[rand.Next(hidden.Length)];
			if (!neurons.Contains(n))
				neurons.Add(n);
		}

		foreach(Neuron n in neurons)
		{
			n.Mutate();
		}
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as NeuralNetwork);
	}

	public bool Equals(NeuralNetwork other)
	{
		bool res = true;

		if(other == null)
		{
			return false;
		}

		if (!GetType().Equals(other.GetType()))
		{
			res = false;
		}

		for (int i = 0; i < inputs.Length; i++)
		{
			if (!inputs[i].Equals(other.inputs[i]))
			{
				res = false;
			}

			if (!hidden[i].Equals(other.hidden[i]))
			{
				res = false;
			}
		}

		return res;
	}

	public int CompareTo(NeuralNetwork other)
	{
		return (int) (Fitness - other.Fitness);
	}

	public NeuralNetwork Clone()
	{
		NeuralNetwork n = new NeuralNetwork(inputs.Length, outputs.Length);

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
}