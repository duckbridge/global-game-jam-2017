using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiLanguageText : MonoBehaviour {

	public bool hasEnglishAsPlaceholder = true;

	private Dictionary<LanguageCode, string> textByLanguageCode;
	private TextMesh textMesh;

	private LanguageCode currentLanguageCode = LanguageCode.eng;

	private bool isInitialized = false;

	// Use this for initialization
	void Awake () {
		Initialize();
	}

	public void Initialize() {
		if(!isInitialized) {
			isInitialized = true;

			textMesh = GetComponent<TextMesh>();
			
			textByLanguageCode = new Dictionary<LanguageCode, string>();
			
			if(hasEnglishAsPlaceholder) {
				textByLanguageCode.Add (LanguageCode.eng, textMesh.text);
			}
			
			MultiLanguageTextPair[] languageTextPairs = GetComponentsInChildren<MultiLanguageTextPair>();
			foreach(MultiLanguageTextPair languageTextPair in languageTextPairs) {
				textByLanguageCode.Add (languageTextPair.languageCode, languageTextPair.text);
			}

			LanguageManager languageManager = SceneUtils.FindObject<LanguageManager>();
			ChangeLanguageTo(languageManager.languageToUse);
		}
	}

	public void ChangeLanguageTo(LanguageCode languageToChangeTo) {

		if(currentLanguageCode != languageToChangeTo) {
			string foundText = "";

			textByLanguageCode.TryGetValue(languageToChangeTo, out foundText);

			if(foundText != "") {
				textMesh.text = foundText;	
			}

			currentLanguageCode = languageToChangeTo;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
