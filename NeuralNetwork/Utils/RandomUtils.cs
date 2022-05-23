using System.Linq;
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

	public static T Tournement<T>(List<T> array, int competitorAmount) where T : System.IComparable<T>
	{
		List<T> competitors = new List<T>();
		for(int i = 0; i < competitorAmount; i++)
		{
			competitors.Add(RandomElement(array));
		}

		T best = competitors[0];
		foreach(T competitor in competitors)
		{
			best = best.CompareTo(competitor) > 0 ? best : competitor;
		}

		return best;
	}

}