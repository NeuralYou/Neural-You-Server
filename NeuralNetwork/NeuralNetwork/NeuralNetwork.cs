using Newtonsoft.Json;

[System.Serializable]
public class NeuralNetwork : System.IEquatable<NeuralNetwork>
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
		foreach(InputNeuron n in inputs)
		{
			n.Mutate(i_MutationRate);
		}

		foreach(HiddenNeuron n in hidden)
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