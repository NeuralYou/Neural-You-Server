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
			outputWeights[i] = Utils.RandomRange(-1f, 1f);
		}

	}
	public void FeedForward(OutputNeuron[] i_OutputLayer)
	{
		for(int i = 0; i < i_OutputLayer.Length; i++)
		{
			i_OutputLayer[i].RecieveInput(outputWeights[i] * CurrentValue);
		}
	}
}
