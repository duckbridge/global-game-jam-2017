using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class MultiDimensionalMenu : Menu {

	public MenuButton backButton;
	public List<RebindMenu> rebindMenus;

	private int currentListIndex = 0;
	private PlayerInputActions playerInputActions;

	public override void Start () {
		base.Start ();

		playerInputActions = PlayerInputHelper.LoadData();

		foreach(RebindMenu rebindMenu in rebindMenus) {
			rebindMenu.SetPlayerInputActions(playerInputActions);
			rebindMenu.AddEventListener(this.gameObject);
		}
	}

	protected override void OnActivated () {
		base.OnActivated ();

		rebindMenus[1].SetInactive();
		rebindMenus[0].SetActive();
	}
	
	public override void Update () {

		if(playerInputActions.left.WasPressed || playerInputActions.right.WasPressed) {

			if(rebindMenus[currentIndex].isActive) {
				rebindMenus[currentIndex].SetInactive();
				
				++currentIndex;
				
				if(currentIndex >= rebindMenus.Count) {
					currentIndex = 0;
				}

				rebindMenus[currentIndex].SetActive();
				
				SetInactive();
			}
		}

		if (playerInputActions.back.WasPressed) {
			OnHideControlsMenu ();
		}
	}

	public void OnHideControlsMenu() {
		DispatchMessage("OnQuittingControlsMenu", null);
	}
}
