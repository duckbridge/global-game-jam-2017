using UnityEngine;
using System.Collections;

public class MenuPopupBox : TimeScaleIndependentUpdate {

	public int showTimeout = 10;
	private Menu oldMenu;

	private int totalTime = 0;

	private bool isActive = false;

	public override void OnUpdate () {
		if(isActive) {
			
			elapsedTime += deltaTime;
			
			if(elapsedTime > timeRequiredForUpdateTick) {
				++totalTime;
				elapsedTime = 0f;

				if(totalTime >= showTimeout) {
					Hide ();
				}
			}
		}
	}

	public void Show(Menu oldMenu) {

		ResetPopupBox();

		isActive = true;

		this.gameObject.SetActive(true);
		this.oldMenu = oldMenu;
		oldMenu.isActive = false;
	}

	public void Hide() {

		ResetPopupBox();

		isActive = false;

		oldMenu.isActive = true;

		this.gameObject.SetActive(false);
	}

	private void ResetPopupBox() {
		totalTime = 0;
		elapsedTime = 0f;
	}
}
