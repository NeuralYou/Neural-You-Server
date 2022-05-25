using System;

public class InputNeuron : Neuron
{

	public InputNeuron(int i_SizeOfNextLayer)
	{
		outputWeights = new float[i_SizeOfNextLayer];

		for (int i = 0; i < outputWeights.Length; i++)
		{
			outputWeights[i] = (float) RandomUtils.RandomRangeNormal(0,1);
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

	internal void MutateWeight(int index)
	{
		float value = (float) RandomUtils.RandomRangeNormal(0, 0.5d);
		outputWeights[index] += value;
	}
}
