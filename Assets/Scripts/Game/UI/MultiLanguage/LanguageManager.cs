using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour {

	public LanguageCode languageToUse;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeLanguageGlobally(LanguageCode languageToChangeTo) {

		List<MultiLanguageText> multiLanguageTexts = SceneUtils.FindObjects<MultiLanguageText>();
		foreach(MultiLanguageText multiLanguageText in multiLanguageTexts) {
			multiLanguageText.Initialize();
			multiLanguageText.ChangeLanguageTo(languageToUse);
		}

		languageToUse = languageToChangeTo;
	}
}
