public class OutputNeuron
{
	private float currentValue;

	public OutputNeuron()
	{
	}

	public void RecieveInput(float i_InputVal)
	{
		currentValue += i_InputVal;
	}

	public void Activate(float i_Threshold)
	{

	}

	public void Reset()
	{
		currentValue = 0;
	}
}
