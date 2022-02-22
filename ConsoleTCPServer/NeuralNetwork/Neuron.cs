using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[System.Serializable]
public class Neuron: System.IEquatable<Neuron>
{
	[JsonProperty] float[] outputWeights;
	[JsonIgnore] public float CurrentValue { get; private set; }

	//For construction of an output neuron.
	public Neuron()
	{

	}

	//For construction of an input\hidden neuron.
	public Neuron(int i_NextLayerSize)
	{
		outputWeights = new float[i_NextLayerSize];

		for(int i = 0; i < outputWeights.Length; i++)
		{
			outputWeights[i] = Utils.RandomRange(-1f, 1f);
		}

	}

	//public void PassInputTo(Neuron i_Neuron)
	//{
	//	List<Neuron> list = new List<Neuron>(nextLayerNeurons);
	//	int index =list.IndexOf(i_Neuron);
	//	i_Neuron.RecieveInput(CurrentValue * outputWeights[index]);
	
	//}

	public void RecieveInput(float val)
	{
		CurrentValue += val;
	}

	public void ActivateSelf()
	{
		CurrentValue = (float) System.Math.Tanh(CurrentValue);
	}

	//Can't feed forward before this neuron was fed all of its inputs.
	public void FeedForward(Neuron[] i_NextLayer)
	{
		for(int i = 0; i < i_NextLayer.Length; i++)
		{
			i_NextLayer[i].RecieveInput(outputWeights[i] * CurrentValue);
		}
	}

	public void Reset()
	{
		CurrentValue = 0;
	}

	public void Mutate(float i_MutationRate)
	{
		for (int i = 0; i < outputWeights.Length; i++)
		{
			if (Utils.RandomRange(0, 1f) <= i_MutationRate)
			{
				outputWeights[i] += Utils.RandomRange(-3f, 3f);
			}

		}
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as Neuron);
	}

	public bool Equals(Neuron other)
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

		if (outputWeights.Length != other.outputWeights.Length)
		{
			res = false;
		}

		for (int i = 0; i < outputWeights.Length; i++)
		{
			if (outputWeights[i] != other.outputWeights[i])
			{
				res = false;
				break;
			}
		}

		return res;
	}
}
