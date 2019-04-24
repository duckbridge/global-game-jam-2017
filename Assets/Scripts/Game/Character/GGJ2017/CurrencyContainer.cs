using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyContainer : MonoBehaviour {

	private int currencyAmount = 0;
	public Color positiveColor, negativeColor, neutralColor;
	private TextMesh amountOutput;

	// Use this for initialization
	void Start () {
		amountOutput = this.transform.Find ("Amount").GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncreaseCurrency(int incrementAmount) {
		currencyAmount += incrementAmount;
		amountOutput.text = (currencyAmount + "");;

		if (currencyAmount > 0) {
			amountOutput.color = positiveColor;

		} else if (currencyAmount < 0) {
			amountOutput.color = negativeColor;

		} else {
			amountOutput.color = neutralColor;
		}
	}

	public int GetCurrencyAmount() {
		return currencyAmount;
	}
}
