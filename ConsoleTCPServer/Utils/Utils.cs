using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Utils
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
}