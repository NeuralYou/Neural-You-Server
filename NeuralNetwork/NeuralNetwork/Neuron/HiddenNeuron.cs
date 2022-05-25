using Newtonsoft.Json;


[System.Serializable]
public class HiddenNeuron: Neuron
{
	//[JsonProperty] float[] outputWeights;
	[JsonIgnore] public float CurrentValue { get; private set; }

	//For construction of an input\hidden neuron.
	public HiddenNeuron(int i_NextLayerSize)
	{
		outputWeights = new float[i_NextLayerSize];

		for(int i = 0; i < outputWeights.Length; i++)
		{
			outputWeights[i] = (float) RandomUtils.RandomRangeNormal(0,1);
		}

	}
	public void FeedForward(OutputNeuron[] i_OutputLayer)
	{
		for(int i = 0; i < i_OutputLayer.Length; i++)
		{
			i_OutputLayer[i].RecieveInput(outputWeights[i] * CurrentValue);
		}
	}

	public HiddenNeuron Clone()
	{
		HiddenNeuron n = new HiddenNeuron(outputWeights.Length);
		
		for(int i = 0; i < n.outputWeights.Length; i++)
		{
			n.outputWeights[i] = outputWeights[i];
		}

		return n;
	}
}
