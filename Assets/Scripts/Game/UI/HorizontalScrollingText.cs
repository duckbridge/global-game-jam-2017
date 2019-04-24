using UnityEngine;
using System.Collections;

public class HorizontalScrollingText : MonoBehaviour {

    public bool showFullTextOnDone = true;
	public float letterSwapTimeout = .25f;

	public int endOfTextIndex = 10;

	private string fullText;
	private TextMesh textOutput;

	private Vector3 oldShowPosition;

	private int originalStartOfTextIndex = 0;
	private int originalEndOfTextIndex = 10;

	private MeshRenderer meshRenderer;
	private int startOfTextIndex = 0;

	void Awake () {
		textOutput = GetComponent<TextMesh>();
		meshRenderer = GetComponent<MeshRenderer>();

		originalStartOfTextIndex = startOfTextIndex;
		originalEndOfTextIndex = endOfTextIndex;
	}

	void Start() { }

    public void ScrollText() {
        startOfTextIndex = originalStartOfTextIndex;
        endOfTextIndex = originalEndOfTextIndex;
        
        fullText = GetComponent<TextMesh>().text;

        if(endOfTextIndex > fullText.Length) {
            endOfTextIndex = fullText.Length;
        }
        
        meshRenderer.enabled = false;
        
        string textToShow = fullText.Substring(startOfTextIndex, endOfTextIndex);

        textOutput.text = textToShow;
        
        meshRenderer.enabled = true;

        Invoke ("SwapLetters", letterSwapTimeout);
    }

	private void SwapLetters() {
		++startOfTextIndex;

		if(startOfTextIndex > fullText.Length) {
		
            if(showFullTextOnDone) {
                textOutput.text = fullText;
            } else {
			    meshRenderer.enabled = false;
            }

		} else {

			if(startOfTextIndex + endOfTextIndex > fullText.Length) {
				endOfTextIndex = fullText.Length - startOfTextIndex;
			}

			string textToShow = fullText.Substring(startOfTextIndex, endOfTextIndex);
			textOutput.text = textToShow;

			Invoke ("SwapLetters", letterSwapTimeout);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
