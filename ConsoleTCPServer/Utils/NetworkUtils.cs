using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkUtils
{
	public static string BytesToString(byte[] i_Array)
	{
		return Encoding.ASCII.GetString(i_Array);
	}

	public static byte[] StringToBytes(string i_Str)
	{
		return Encoding.ASCII.GetBytes(i_Str);
	}

	public static int ReadInt(NetworkStream i_Stream)
	{
		byte[] bytes = ReadBytes(i_Stream, 4);
		int val = BitConverter.ToInt32(bytes, 0);
		return val;
	}

	public static float ReadFloat(NetworkStream i_stream)
	{
		byte[] bytes = ReadBytes(i_stream, 4);
		float val = BitConverter.ToSingle(bytes, 0);
		return val;
	}

	public static string ReadString(NetworkStream i_Stream, int i_Length)
	{
		byte[] bytes = ReadBytes(i_Stream, i_Length);
		string str = Encoding.ASCII.GetString(bytes);
		return str;
	}

	public static byte[] ReadBytes(NetworkStream i_Stream, int i_NumberOfBytes)
	{
		byte[] bytes = new byte[i_NumberOfBytes];
		i_Stream.Read(bytes, 0, i_NumberOfBytes);
		return bytes;

	}

	public static NeuralNetwork ReadNN(NetworkStream i_Stream, int i_LengthInBytes)
	{
		string rep = ReadString(i_Stream, i_LengthInBytes);
		NeuralNetwork net = JsonConvert.DeserializeObject<NeuralNetwork>(rep);
		// Task.Run(() => rep = ReadString(i_Stream, i_LengthInBytes)).ContinueWith((a) => net = JsonConvert.DeserializeObject<NeuralNetwork>(rep));
		return net;
	}

	public static void WriteInt(NetworkStream i_Stream, int i_Val)
	{
		byte[] bytes = BitConverter.GetBytes(i_Val);
		i_Stream.Write(bytes, 0, bytes.Length);
	}

	public static void WriteFloat(NetworkStream i_Stream, float i_Val)
	{
		byte[] bytes = BitConverter.GetBytes(i_Val);
		i_Stream.Write(bytes, 0, bytes.Length);
	}

	public static void WriteString(NetworkStream i_Stream, string i_Val)
	{
		byte[] bytes = StringToBytes(i_Val);
		i_Stream.Write(bytes, 0, bytes.Length);
	}

	public static void WriteNN(NetworkStream i_Stream, NeuralNetwork i_Val, bool i_WriteLengthFirst = true)
	{
		string rep = JsonConvert.SerializeObject(i_Val);

		if (i_WriteLengthFirst)
		{
			byte[] bytes = StringToBytes(rep);
			WriteInt(i_Stream, bytes.Length);
		}

		WriteString(i_Stream, rep);
	}
}
