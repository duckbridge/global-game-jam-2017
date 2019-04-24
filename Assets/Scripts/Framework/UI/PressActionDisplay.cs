using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PressActionDisplay : MonoBehaviour {

	public bool showOnAwake = false;
	public enum ActionNames { Default, DoorEnter, TextBox, YES, NO }

	private bool isInitialized = false;
	private TextMesh interractText;
	private string originalInterractText;

	private List<SpriteRenderer> allIcons;

	private ActionNames actionName = ActionNames.Default;

	public void Awake() {
		Initialize();
		originalInterractText = interractText.text;

		if(showOnAwake) {
			Show ();
		} else {
			Hide ();
		}
	}

	public void Start() {}
	
	public void Update() {}

	public void Show(string newText = "", ActionNames actionName = ActionNames.Default) {

		Initialize();

		if(newText != "") {
			interractText.text = newText;
		} else {
			interractText.text = originalInterractText;
		}
	

		Hide ();

		interractText.gameObject.SetActive(true);

		SpriteRenderer xboxIcon = allIcons.Find(icon => icon.name == ("Xbox" + actionName.ToString()));
		SpriteRenderer keyboardIcon = allIcons.Find(icon => icon.name == ("Keyboard" + actionName.ToString()));

	

		if(ControllerHelper.IsXboxControllerPluggedIn()) {
			xboxIcon.enabled = true;
			keyboardIcon.enabled = false;

			HideAnimationOfIcon(keyboardIcon);
			ShowAnimationOfIcon(xboxIcon);
		} else {

			keyboardIcon.enabled = true;
			xboxIcon.enabled = false;

			HideAnimationOfIcon(xboxIcon);
			ShowAnimationOfIcon(keyboardIcon);
		}
	}
	
	public void Hide() {

		Initialize();

		foreach(SpriteRenderer icon in allIcons) {

			HideAnimationOfIcon(icon);
			icon.enabled = false;
		}

		interractText.gameObject.SetActive(false);

	}

	private void ShowAnimationOfIcon(SpriteRenderer icon) {
		Animation2D iconAnimation = icon.GetComponent<Animation2D>();
		
		if(iconAnimation) {
			iconAnimation.Initialize();

			iconAnimation.Show ();
			iconAnimation.Play();
		}
	}

	private void HideAnimationOfIcon(SpriteRenderer icon) {
		Animation2D iconAnimation = icon.GetComponent<Animation2D>();
		
		if(iconAnimation) {
			iconAnimation.Initialize();

			iconAnimation.Stop ();
			iconAnimation.Hide ();
		}
	}

	private void Initialize() {
		if(!isInitialized) {

			isInitialized = true;

			interractText = GetComponentInChildren<TextMesh>();
			allIcons = new List<SpriteRenderer>(this.transform.Find("Icons").GetComponentsInChildren<SpriteRenderer>());
		}
	}
}
