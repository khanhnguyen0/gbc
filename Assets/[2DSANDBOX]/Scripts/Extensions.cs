using System;

public static class ArrayExtensions
{
	public static void ForEach<T>(this T[] array, Action<T> callback)
	{
		for (int i = 0, len = array.Length; i < len; ++i)
		{
			callback(array[i]);
		}
	}
}