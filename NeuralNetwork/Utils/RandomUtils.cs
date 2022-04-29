using System.Collections.Generic;

class RandomUtils
{
	public static System.Random rand = new System.Random();
	public static float RandomRange(float min, float max)
	{
		float delta = max - min;
		float val = (float)((rand.NextDouble() * delta) + min);
		return val;
	}

	public static int RandomRange(int min, int max)
	{
		int delta = max - min;
		int  val = (int)((rand.NextDouble() * delta) + min);
		return val;
	}

	public static bool RollOdds(float chance)
	{
		float val = (float)rand.NextDouble();
		return chance <= val;
	}

	public static T RandomElement<T>(IList<T> array)
	{
		return array[RandomRange(0, array.Count)];
	}
}