using Newtonsoft.Json;
using System;

public abstract class Neuron
{
	protected float currentValue;
	[JsonProperty] internal float[] outputWeights;
	protected bool active;

	public virtual void RecieveInput(float i_InputVal)
	{
		currentValue += i_InputVal;
	}

	public virtual void Activate(float i_Threshold)
	{
		currentValue = (float)Math.Tanh(currentValue);

		active = currentValue > i_Threshold;
		if(currentValue > i_Threshold) { active = true; }
		else { active = false; }
	}

	public void Reset()
	{
		currentValue = 0;
	}

	public void FeedForward(Neuron[] i_NextLayerNeurons)
	{
		for(int i = 0; i < i_NextLayerNeurons.Length; i++)
		{
			i_NextLayerNeurons[i].RecieveInput(currentValue * outputWeights[i]);
		}
	}

	public void Mutate(bool biasedMutation)
	{
		for (int i = 0; i < outputWeights.Length; i++)
		{
			float value = RandomUtils.RandomRange(-3f, 3f);
			outputWeights[i] = biasedMutation ? outputWeights[i] + value : value;
		}
	}
	//public void UnbiasedMutate()
	//{
	//	for (int i = 0; i < outputWeights.Length; i++)
	//	{
	//		outputWeights[i] += RandomUtils.RandomRange(-3f, 3f);
	//	}
	//}
}
