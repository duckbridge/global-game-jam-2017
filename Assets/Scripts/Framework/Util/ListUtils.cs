using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListUtils  {

	public static T NextTo<T>(List<T> list, T item) {
		int nextIndex = list.IndexOf(item) + 1;

		if(nextIndex >= list.Count) {
			nextIndex = 0;
		}

		return list[nextIndex];
	}

	public static T PreviousOf<T>(List<T> list, T item) {
		int previousIndex = list.IndexOf(item) - 1;

		if(previousIndex < 0) {
			previousIndex = list.Count - 1;
		}

		return list[previousIndex];
	}
}
