using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextContainer  {
	public TextMesh textMesh;
	public TextContainerWord[] words;
	private int currentWordIndex = 0;

	private bool hasSplitLetters = false;

	public TextContainer(TextMesh textMesh, bool splitLetters = false) {

		this.hasSplitLetters = splitLetters;

		currentWordIndex = 0;

		this.textMesh = textMesh;
		if(splitLetters) {
			this.words = SplitWordsAndLetters();
		} else {
			this.words = SplitWords();
		}

		textMesh.text = "";
	}

	public void AppendNextWord() {

		TextContainerWord nextWord = words[currentWordIndex];

		if(hasSplitLetters) {
			textMesh.text += nextWord.text + (nextWord.isEndOfWord ? " " : "");
		} else {
			textMesh.text += nextWord.text + " ";
		}

		currentWordIndex++;
	}

	public bool CanDisplayNextWord() {
		return (currentWordIndex < words.Length);
	}

	private TextContainerWord[] SplitWords() {

		string[] wordsAsStrings = textMesh.text.Trim().Split(' ');
		TextContainerWord[] allWords = new TextContainerWord[wordsAsStrings.Length];

		for(int i = 0 ; i < wordsAsStrings.Length ; i++) {
			TextContainerWord tcw = new TextContainerWord();
			tcw.text = wordsAsStrings[i];
			allWords[i] = tcw;
		}

		return allWords;
	}

	private TextContainerWord[] SplitWordsAndLetters() {
		List<TextContainerWord> allWordsOfSentenses = new List<TextContainerWord>();
		string[] localWords = textMesh.text.Trim().Split(' ');
		
		for(int i = 0 ; i < localWords.Length ; i++) {

			string currentWord = localWords[i];

			for(int j = 0 ; j < currentWord.Length ; j++) {

				TextContainerWord tcw = new TextContainerWord();
				tcw.text = currentWord[j].ToString();
				if(j == currentWord.Length - 1) {
					tcw.isEndOfWord = true;
				} 

				allWordsOfSentenses.Add (tcw);
			}
		}
		
		return allWordsOfSentenses.ToArray();
		
	}
}
