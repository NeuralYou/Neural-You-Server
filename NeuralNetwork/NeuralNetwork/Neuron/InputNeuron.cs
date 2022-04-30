using System;

public class InputNeuron : Neuron
{

	public InputNeuron(int i_SizeOfNextLayer)
	{
		outputWeights = new float[i_SizeOfNextLayer];

		for (int i = 0; i < outputWeights.Length; i++)
		{
			outputWeights[i] = RandomUtils.RandomRange(-1f, 1f);
		}
	}

	public InputNeuron Clone()
	{
		InputNeuron n = new InputNeuron(outputWeights.Length);

		for(int i = 0; i < n.outputWeights.Length; i++)
		{
			n.outputWeights[i] = this.outputWeights[i];
		}

		return n;
	}

	internal void MutateWeight(int index, bool biasedMutation)
	{
		float value = RandomUtils.RandomRange(-3f, 3f);
		outputWeights[index] = biasedMutation ? outputWeights[index] + value : value;
	}
}
