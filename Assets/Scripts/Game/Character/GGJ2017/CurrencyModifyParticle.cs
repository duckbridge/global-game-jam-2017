using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyModifyParticle : MonoBehaviour {

	public Vector3 particleTravelDistance = new Vector3 (0f, 0f, 30f);
	public float particleTravelTime = 2.5f;
	public float particleExpandTime = .75f;

	// Use this for initialization
	void Start () {
	}

	public void Show(int amount) {

		this.transform.Find ("Amount").GetComponent<TextMesh> ().text =  amount >= 0 ? "+" : "-";

		CurrencyContainer currencyContainer = SceneUtils.FindObject<CurrencyContainer> ();

		iTween.ScaleFrom (this.gameObject, 
			new ITweenBuilder ()
			.SetScale(new Vector3 (0f, 0f, 0f))
			.SetTime (particleExpandTime)
			.Build ());

		iTween.MoveBy (this.gameObject, new ITweenBuilder()
			.SetAmount(particleTravelDistance)
			.SetTime(particleTravelTime)
			.SetOnComplete("OnDoneWithParticle")
			.SetOnCompleteTarget(this.gameObject).Build()
		);

		if (amount > 0) {
			this.transform.Find("OnCurrencyGainedSound").GetComponent<SoundObject> ().PlayIndependent (true);
			this.transform.Find ("Amount").GetComponent<TextMesh> ().color = currencyContainer.positiveColor;

		} else {
			this.transform.Find("OnCurrencyLostSound").GetComponent<SoundObject> ().PlayIndependent (true);
			this.transform.Find ("Amount").GetComponent<TextMesh> ().color = currencyContainer.negativeColor;
		}
	}

	private void OnDoneWithParticle() {
		Destroy (this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
