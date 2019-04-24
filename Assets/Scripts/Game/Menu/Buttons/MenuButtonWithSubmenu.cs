using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Menu))]
public class MenuButtonWithSubmenu : MenuButtonWithColors {

	private Menu menu;

	public override void Start () {
		base.Start();
		menu = GetComponent<Menu>();
	}

	public override void OnSelected () {
		base.OnSelected ();
		ShowSubmenu();
	}

	public override void OnUnSelected () {
		base.OnUnSelected ();
		HideSubmenu();
	}

	public void ShowSubmenu() {
        if(!menu) {
            menu = GetComponent<Menu>();
        }
		menu.menuButtons.ForEach(menuButton => menuButton.gameObject.SetActive(true));
	}

	public void HideSubmenu() {
        if(!menu) {
            menu = GetComponent<Menu>();
        }
		menu.menuButtons.ForEach(menuButton => menuButton.gameObject.SetActive(false));
	}

	public Menu GetMenu() {
		return menu;
	}
}
