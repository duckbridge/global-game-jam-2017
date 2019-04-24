using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInputHelper {

	private static PlayerInputActions playerInputActions;

	public static PlayerInputActions LoadData() {

		if (playerInputActions != null) {
			return playerInputActions;
		}

		playerInputActions = new PlayerInputActions();
		PlayerPrefs.DeleteKey(GameSettings.INPUT_SAVE_NAME);

		//string savedInputData = PlayerPrefs.GetString(GameSettings.INPUT_SAVE_NAME, "");
		string savedInputData = "";

		if(savedInputData != "") {
			playerInputActions.Load (savedInputData);
		} else {

			playerInputActions.left.AddDefaultBinding(Key.LeftArrow);
			playerInputActions.left.AddDefaultBinding(InputControlType.LeftStickLeft);
			playerInputActions.left.AddDefaultBinding(InputControlType.DPadLeft);

			playerInputActions.right.AddDefaultBinding(Key.RightArrow);
			playerInputActions.right.AddDefaultBinding(InputControlType.LeftStickRight);
			playerInputActions.right.AddDefaultBinding(InputControlType.DPadRight);

			playerInputActions.up.AddDefaultBinding(Key.UpArrow);
			playerInputActions.up.AddDefaultBinding(InputControlType.LeftStickUp);
			playerInputActions.up.AddDefaultBinding(InputControlType.DPadUp);

			playerInputActions.down.AddDefaultBinding(Key.DownArrow);
			playerInputActions.down.AddDefaultBinding(InputControlType.LeftStickDown);
			playerInputActions.down.AddDefaultBinding(InputControlType.DPadDown);

			playerInputActions.rStickLeft.AddDefaultBinding(Key.A);
			playerInputActions.rStickLeft.AddDefaultBinding(InputControlType.RightStickLeft);

			playerInputActions.rStickRight.AddDefaultBinding(Key.D);
			playerInputActions.rStickRight.AddDefaultBinding(InputControlType.RightStickRight);

			playerInputActions.rStickUp.AddDefaultBinding(Key.W);
			playerInputActions.rStickUp.AddDefaultBinding(InputControlType.RightStickUp);

			playerInputActions.rStickDown.AddDefaultBinding(Key.S);
			playerInputActions.rStickDown.AddDefaultBinding(InputControlType.RightStickDown);

			playerInputActions.menuSelect.AddDefaultBinding(Key.D);
			playerInputActions.menuSelect.AddDefaultBinding(InputControlType.Action1);

			playerInputActions.interact.AddDefaultBinding(Key.D);
			playerInputActions.interact.AddDefaultBinding (InputControlType.Action1);
			playerInputActions.interact.AddDefaultBinding(InputControlType.Button1);

			playerInputActions.pause.AddDefaultBinding(Key.Escape);
			playerInputActions.pause.AddDefaultBinding(Key.Tab);
			playerInputActions.pause.AddDefaultBinding(InputControlType.Command);

			playerInputActions.back.AddDefaultBinding(Key.S);
			playerInputActions.back.AddDefaultBinding(InputControlType.Action2);

			playerInputActions.secondAttack.AddDefaultBinding(Key.S);
			playerInputActions.secondAttack.AddDefaultBinding(InputControlType.Action2);

			playerInputActions.thirdAttack.AddDefaultBinding(Key.Q);
			playerInputActions.thirdAttack.AddDefaultBinding(InputControlType.Action3);

			playerInputActions.roll.AddDefaultBinding(Key.LeftShift);
			playerInputActions.roll.AddDefaultBinding(InputControlType.LeftTrigger);

			playerInputActions.previousTrack.AddDefaultBinding(Key.Q);
			playerInputActions.previousTrack.AddDefaultBinding(InputControlType.LeftTrigger);

			playerInputActions.nextTrack.AddDefaultBinding(Key.E);
			playerInputActions.nextTrack.AddDefaultBinding(InputControlType.RightTrigger);

			playerInputActions.dance.AddDefaultBinding(Key.R);
			playerInputActions.dance.AddDefaultBinding (InputControlType.LeftBumper);

			PlayerPrefs.SetString(GameSettings.INPUT_SAVE_NAME, playerInputActions.Save ());
			PlayerPrefs.Save ();
		}

		return playerInputActions;
	}

	public static string[] DecideDeviceNameAndInputNameForInteract() {
		string[] deviceNameAndInputName = new string[2];

		InControl.PlayerInputActions playerInputActions = PlayerInputHelper.LoadData();

		InControl.InputDevice device = InControl.InputManager.ActiveDevice;

		//Logger.Log (device.Name);

		if(device.Name == "None") { //keyboard?
			for(int i = 0 ; i < playerInputActions.interact.Bindings.Count ; i++) {
				if(playerInputActions.interact.Bindings[i].BindingSourceType == InControl.BindingSourceType.KeyBindingSource) {
					deviceNameAndInputName[0] = playerInputActions.interact.Bindings[i].DeviceName;
					deviceNameAndInputName[1] = playerInputActions.interact.Bindings[i].Name;

					//Logger.Log ("Keyboard " + deviceNameAndInputName[1]);
				}
			}
		} else {
			for(int i = 0 ; i < playerInputActions.interact.Bindings.Count ; i++) {
				if(playerInputActions.interact.Bindings[i].BindingSourceType == InControl.BindingSourceType.DeviceBindingSource) {
					deviceNameAndInputName[0]= playerInputActions.interact.Bindings[i].DeviceName;
					deviceNameAndInputName[1] = playerInputActions.interact.Bindings[i].Name;

					//Logger.Log ("Controller " + deviceNameAndInputName[1]);
				}
			}
		}

		return deviceNameAndInputName;
	}

	public static void ResetInputHelper() {
		playerInputActions = null;
	}
}
