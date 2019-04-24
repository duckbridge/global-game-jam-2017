using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutSceneManager : DispatchBehaviour {

	public bool hideUIOnCutsceneStart = true;
	public bool showUIOnCutsceneDone = true;
	public bool disablePausing = true;

	public bool disablePlayerInputOnStart = true;
	public bool enablePlayerInputOnDone = false;

	public List<CutScene> cutScenes;

	private List<UIElement> uiElements;

	private int currentCutSceneIndex = 0;
	private bool isInitialized = false;

	private CutsceneEnableCollider[] cutsceneEnableColliders;

	public virtual void Awake() {
		Initialize();
	}
	
	public void Update() {}

	public void StartCutScene(bool disablePlayerInput) {

		currentCutSceneIndex = 0;
		
		if(disablePlayerInput) {
			SceneUtils.FindObject<PlayerInputComponent>().enabled = false;
		}

		Initialize();

		cutScenes.ForEach(cutScene => cutScene.ResetIndex());

		if(hideUIOnCutsceneStart) {
			uiElements.ForEach(uiElement => uiElement.Hide ());
		}

		OnActivateCutScene();
		
		if(cutsceneEnableColliders != null) {
			for(int i = 0 ; i < cutsceneEnableColliders.Length ; i++) {
				cutsceneEnableColliders[i].GetComponent<Collider>().enabled = false;
				cutsceneEnableColliders[i].RemoveEventListener(this.gameObject);
			}
		}
	}

	public virtual void OnListenerTrigger(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			StartCutScene(false);
		}
	}

	public void OnActivateCutScene() {
		if(currentCutSceneIndex > 0) {
			cutScenes[currentCutSceneIndex - 1].OnDeActivate();
		}

		cutScenes[currentCutSceneIndex].OnActivate();
	}
	
	public void OnCutSceneDone(CutScene cutScene) {
		currentCutSceneIndex++;

		if(cutScenes.Count == currentCutSceneIndex) {

			DispatchMessage("OnCutSceneManagerDone", this);

			if(showUIOnCutsceneDone) {
				uiElements.ForEach(uiElement => uiElement.Show ());
			}

			if(enablePlayerInputOnDone) {
				SceneUtils.FindObject<PlayerInputComponent>().enabled = true;
			}

			this.gameObject.SetActive(false);

		} else {
			OnActivateCutScene();
		}
	}

	private void Initialize() {
		if(!isInitialized) {
			isInitialized = true;
			cutScenes.ForEach(cutScene => cutScene.AddEventListener(this.gameObject));
			
			cutsceneEnableColliders = GetComponentsInChildren<CutsceneEnableCollider>();

			if(cutsceneEnableColliders != null) {
				for(int i = 0; i < cutsceneEnableColliders.Length ; i++) {
					cutsceneEnableColliders[i].AddEventListener(this.gameObject);
				}
			}
			
			uiElements = new List<UIElement>(SceneUtils.FindObjectsOfType<UIElement>());
		}
	}
}
