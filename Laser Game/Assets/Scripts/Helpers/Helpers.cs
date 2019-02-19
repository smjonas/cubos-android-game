using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class Helpers {

    public static T[] ShiftByOne<T>(this T[] array) {

        var newArray = array.Clone() as T[];
        var last = newArray[newArray.Length - 1];

        for (int i = newArray.Length - 1; i > 0; i--) {
            newArray[i] = newArray[i - 1];
        }
        newArray[0] = last;
        return newArray;
    }

    public static void ShuffleList<T> (this List<T> list) {

		int n = list.Count;

		while (n > 1) {
			int k = (UnityEngine.Random.Range(0, n) % n);
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static List<int> ToOrderList(this int[] distribution) {

		List<int> orderList = new List<int>();
		// E.g. 121 --> 0112, because 121[1] is 2, therefore 11
		for (int i = 0; i < distribution.Length; i++) {
			for (int j = 0; j < distribution [i]; j++) {
				orderList.Add (i);
			}
		}
		return orderList;
	}

	public static bool ContainsSequence<T>(this T[] array, params T[] sequence) {

		if (sequence.Count () == 0 || sequence.Count () > array.Count () ||
			(array.Count () == sequence.Count ()) && !array[0].Equals (sequence[0]))
			Debug.LogError ("Sequence length is zero or longer than array!");

		var count = 0;
		for (int i = 0; i < array.Length - sequence.Length + 1; i++) {

			if (array [i].Equals (sequence [0])) {

				count++;
				if (count == sequence.Length)
					return true;

				for (int j = 1; j < sequence.Length; j++)
					if (array[i + 1].Equals (sequence[j]))
						count++;
			}
		}
		return count == sequence.Length;
	}

    public static bool HasAnyDuplicates<T>(this IEnumerable<T> list) {

        var hashset = new HashSet<T>();
        return list.Any(e => !hashset.Add(e));
    }

    public static void SetAllValues<T>(this T[] array, T value) {

		for (int i = 0; i < array.Length; i++)
			array [i] = value;
	}

	public static int GetItemCount<T>(this List<T> list, T item) {

		var count = 0;

		foreach (T element in list)
			if (element.Equals (item))
				count++;

		return count;
	}

	public static int[] RandomDistribution(this int total, int divisions) {

		if (divisions > total)
			Debug.LogError ("RandomDistribution: Number of divisions cannot be higher than total!");

		int[] distribution = new int[divisions];
		// Set all elements to 1
		for (int i = 0; i < distribution.Length; i++) {
			distribution [i] = 1;
			total--;
		}

		while (total > 0) {
			distribution [UnityEngine.Random.Range (0, distribution.Length)]++;
			total--;
		}
		return distribution;
	}

	public static T[,] Shuffle<T> (this T[,] array) { // #Unoptimized!

		T[,] shuffledArray = new T[array.GetLength (0), array.GetLength (1)];

		// Shuffle rows
		for (int row = 0; row < array.GetLength (0); row++) {

			var shuffledRow = array.GetRow (row).ShuffleArray ();
			shuffledArray.SetRow (row, shuffledRow);
		}

		// Shuffle columns
		for (int col = 0; col < array.GetLength (1); col++) {

			var shuffleCol = shuffledArray.GetCol (col).ShuffleArray ();
			shuffledArray.SetCol (col, shuffleCol);

		}
		return shuffledArray;
	}


	static T[] ShuffleArray<T> (this T[] array) {

		int n = array.Length;

		while (n > 1) {
			int k = (UnityEngine.Random.Range(0, n) % n);
			n--;
			T value = array[k];
			array[k] = array[n];
			array[n] = value;
		}
		return array;
	}
		
	public static T[,] SetRow<T>(this T[,] array, int row, T[] rowArray) {

		int rowLength = array.GetLength(1);

		for (int i = 0; i < rowLength; i++)
			array[row, i] = rowArray[i];
		
		return array;
	}

	public static T[] GetRow<T> (this T[,] array, int row) {

		int cols = array.GetLength (1);
		T[] newArray = new T[cols];

		for (int i = 0; i < cols; i++)
			newArray [i] = array [row, i];

		return newArray;
	}

	public static T[,] SetCol<T>(this T[,] array, int col, T[] colArray) {

		int colLength = array.GetLength(0);

		for (int i = 0; i < colLength; i++)
			array[i, col] = colArray[i];

		return array;
	}

	public static T[] GetCol<T> (this T[,] array, int col) {

		int rows = array.GetLength (0);
		T[] newArray = new T[rows];

		for (int i = 0; i < rows; i++)
			newArray [i] = array [i, col];

		return newArray;
	}
		
	public static T[,] Mirror<T>(this T[,] array) {

		T[,] newArray = new T[array.GetLength (0), array.GetLength (1)];

		int rows = array.GetLength (0);
		int cols = array.GetLength (1);

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				
				newArray [row, col] = array [(rows - 1) - row, col];
            }
		}
		return newArray;
	}

	static int width;

	public static void ShiftByOne<T>(this T[,] array, int pos, Swipes.Direction direction) {

		T[] row, col;
		T first, last;

		width = array.GetLength (1);

		switch (direction) {

		case Swipes.Direction.LEFT:

			row = array.GetRow (GetRowByPos (pos));
			first = row [0];

			for (int j = 0; j < row.Length - 1; j++)
				row [j] = row [j + 1];

			row [row.Length - 1] = first;
			array.SetRow (GetRowByPos (pos), row);
			break;

		case Swipes.Direction.RIGHT:

			row = array.GetRow (GetRowByPos (pos));
			last = row [row.Length - 1];

			for (int l = row.Length - 1; l > 0; l--)
				row [l] = row [l - 1];

			row [0] = last;
			array.SetRow (GetRowByPos (pos), row);
			break;

		case Swipes.Direction.UP:

			col = array.GetCol (GetColByPos (pos));
			last = col [col.Length - 1];

			for (int k = col.Length - 1; k > 0; k--)
				col [k] = col [k - 1];

			col [0] = last;
			array.SetCol (GetColByPos (pos), col);
			break;

		case Swipes.Direction.DOWN:

			col = array.GetCol (GetColByPos (pos));
			first = col [0];

			for (int m = 0; m < col.Length - 1; m++)
				col [m] = col [m + 1];

			col [col.Length - 1] = first;
			array.SetCol (GetColByPos (pos), col);
			break;

		}
	}

	private static int GetRowByPos(int pos) {
		return pos / width;
	}

	private static int GetColByPos(int pos) {
		return pos % width;
	}
		
    public static int GetRow(this int pos) {
        return pos / LevelManager.instance.boardWidth;
    }

    public static int GetCol(this int pos) {
        return pos % LevelManager.instance.boardWidth;
    }

	public static class WeightedRandomness {

		public static int GetElement (params float[] elements) {

			if (elements.Sum () != 100f) Debug.LogError ("RandomElement: Sum of probabilities must be 100%!");

			float diceRoll = UnityEngine.Random.value * 100f;
			float cumulative = 0f;

			for (int i = 0; i < elements.Length; i++) {

				cumulative += elements [i];
				if (diceRoll < cumulative) {
					return i;
				}
			}
			Debug.LogError ("RandomElement: should not return -1!");
			return -1;
		}
	}

	public static Coroutine FadeTo<T>(this GameObject gameObject, float alpha, float time) {

		T component = gameObject.GetComponent<T> ();
		var property = component.GetType ().GetProperty ("color");
		var color = (Color)property.GetValue (component, null);

		return HelpersExtension.instance.StartCoroutine (HelpersExtension.instance.FadeToCoroutine (component, property, color, alpha, time));
	}
}