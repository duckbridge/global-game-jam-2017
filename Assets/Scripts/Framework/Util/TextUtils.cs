using UnityEngine;
using System.Collections;

public class TextUtils : MonoBehaviour {

	public static string ReplaceLastInSentence(string sourceString, string wordToReplace, string newWord, char sentenceSplitter = ' ') {

		string[] splittedSouceString = sourceString.Split(sentenceSplitter);
		int lastIndexOfWord = 0;

		for(int i = 0 ; i < splittedSouceString.Length ; i++) {

			string originalWord = splittedSouceString[i];
			if(originalWord == wordToReplace) {
				lastIndexOfWord = i;
			}
		}

		splittedSouceString[lastIndexOfWord] = newWord;

		string newSentence = "";

		for(int i = 0 ; i < splittedSouceString.Length ; i++) {
			if(newSentence.Length > 0) {
				newSentence += " ";
			}

			newSentence += splittedSouceString[i];
		}

		return newSentence;
	}
}
