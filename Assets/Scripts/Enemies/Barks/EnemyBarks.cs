using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public static class EnemyBarks
{
	static Dictionary<int, List<string>> barks;

	public static void InitBarks()
	{
		string temp = Resources.Load<TextAsset>("Barks/Barks").ToString();
		string[] tempArr = temp.Split("\r\n");
		//Debug.Log("temp : " + temp);
		//Debug.Log("tempArr size : " + tempArr.Length);
		string[] tempBarks = new string[tempArr.Length - 1];
		barks = new Dictionary<int, List<string>>();
		//Debug.Log("barks size : " + barks.Length);
		for (int i = 0; i < tempBarks.Length; i++)
		{
			// Copy the temporary array in the temporary barks array, capitalizing the first letter and lowering the rest
			tempBarks[i] = CaptializeFirstLetter(tempArr[i + 1]);

			// Get the size of the current string
			int size = tempBarks[i].Length;
			
			// Allocate the string list for that key of it doesn't exist and add the string to that key, otherwise just add the value to that key
			if (!barks.ContainsKey(size))
			{
				barks[size] = new List<string>();
				barks[size].Add(tempBarks[i]);
			}
			else
			{
				barks[size].Add(tempBarks[i]);
			}
			//Debug.Log($"{i} (size : {tempBarks[i].Length}): [{tempBarks[i]}]");
		}
	}

	static string CaptializeFirstLetter(string str)
	{
		if (string.IsNullOrEmpty(str))
			return str;

		// Capitalize the first letter and make the rest lowercase
		return char.ToUpper(str[0]) + str.Substring(1).ToLower();
	}


	// Return the list of barks of a set size
	static List<string> GetBarksOfSize(int size)
	{
		if (!barks.ContainsKey(size))
		{
			Debug.LogWarning($"No barks of size {size} exist !");
			return new List<string>();
		}

		return barks[size];
	}

	public static void WriteBarksOfSize(int size)
	{
		List<string> temp = GetBarksOfSize(size);
		for (int i = 0; i < temp.Count; i++)
		{
			Debug.Log($"Bark ({i}) : {temp[i]}");
		}
	}

	public static string GetRandomBark(int size)
	{
		// Get all the barks that have a set size
		List<string> temp = GetBarksOfSize(size);

		string ret;
		if (temp.Count == 0)
			ret = "ERROR !";
		else
			ret = temp[Random.Range(0, temp.Count)];

		return ret;
	}
}
