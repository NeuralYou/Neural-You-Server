using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class NeuralNetwork : System.IEquatable<NeuralNetwork>
{
	[JsonProperty] Neuron[] inputs;
	[JsonProperty] Neuron[] hidden;
	[JsonProperty] Neuron[] outputs;
	float networkThershold;
	public float Fitness { get; set; }

	
	public NeuralNetwork(int i_NumberOfInputs, int i_NumberOfHidden, int i_NumberOfOutputs)
	{
		inputs = new Neuron[i_NumberOfInputs];
		hidden = new Neuron[i_NumberOfHidden];
		outputs = new Neuron[i_NumberOfOutputs];
		InitNeurons();
	}

	private void InitNeurons()
	{
		for (int i = 0; i < outputs.Length; i++)
		{
			outputs[i] = new Neuron();
		}

		for (int i = 0; i < hidden.Length; i++)
		{
			hidden[i] = new Neuron(outputs.Length);
		}

		for (int i = 0; i < inputs.Length; i++)
		{
			inputs[i] = new Neuron(hidden.Length);
		}
	}

	public float[] FeedForward(float[] i_Inputs)
	{
		FeedInput(i_Inputs);
		FeedInputToHidden();
		FeedHiddenToOutput();

		return GenerateOutput();
	}

	public void FeedInput(float[] i_Inputs)
	{
		for(int i = 0; i < inputs.Length; i++)
		{
			inputs[i].RecieveInput(i_Inputs[i]);
		}
	}


	public void FeedInputToHidden()
	{
		foreach(Neuron input in inputs)
		{
			input.FeedForward(hidden);
		}

		foreach(Neuron hid in hidden)
		{
			hid.ActivateSelf();
		}
	}

	public void FeedHiddenToOutput()
	{
		foreach(Neuron hid in hidden)
		{
			hid.FeedForward(outputs);
		}

		foreach(Neuron output in outputs)
		{
			output.ActivateSelf();
		}
	}

	public float[] GenerateOutput()
	{
		float[] networkOutputs = new float[outputs.Length];
		for(int i = 0; i < networkOutputs.Length; i++)
		{
			networkOutputs[i] = outputs[i].CurrentValue;
		}

		return networkOutputs;
	}

	public void ResetNetwork()
	{
		foreach(Neuron n in inputs)
		{
			n.Reset();
		}

		foreach (Neuron n in hidden)
		{
			n.Reset();
		}

		foreach (Neuron n in outputs)
		{
			n.Reset();
		}
	}
	
	public void MutateNetwork(float i_MutationRate)
	{
		foreach(Neuron n in inputs)
		{
			n.Mutate(i_MutationRate);
		}

		foreach(Neuron n in hidden)
		{
			n.Mutate(i_MutationRate);
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
}