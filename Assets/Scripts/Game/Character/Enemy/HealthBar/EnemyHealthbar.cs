using UnityEngine;
using System.Collections;

public class EnemyHealthbar : MonoBehaviour {

	public Color colorOne = Color.yellow;
	public Color colorTwo = Color.red;

	public int maxHealthbarSize = 5;

	protected GameObject healthbarContainer;
	protected SpriteRenderer healthbarSprite;

	protected Vector4 differenceBetweenColors;

	// Use this for initialization
	public virtual void Awake () {
		healthbarContainer = this.transform.Find("HealthbarContainer").gameObject;
		healthbarSprite = healthbarContainer.transform.Find("HealthbarContent").GetComponent<SpriteRenderer>();
		healthbarSprite.color = colorOne;

		differenceBetweenColors = new Vector4(colorTwo.r - colorOne.r, colorTwo.g - colorOne.g, colorTwo.b - colorOne.b, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void UpdateHealthbar(float currentAmount, int maxAmount) {

        if(!healthbarContainer) {
            Awake();
        }

		if(currentAmount >= maxAmount) {
			healthbarContainer.transform.localScale = new Vector3(maxHealthbarSize, healthbarContainer.transform.localScale.y, healthbarContainer.transform.localScale.z);
		} else if(currentAmount > 0) {
			healthbarContainer.transform.localScale = new Vector3((currentAmount / (float)maxAmount) * maxHealthbarSize, healthbarContainer.transform.localScale.y, healthbarContainer.transform.localScale.z);
		} else {
			healthbarContainer.transform.localScale = new Vector3(0f, healthbarContainer.transform.localScale.y, healthbarContainer.transform.localScale.z);
		}

		healthbarSprite.color = GetColorInBetween(currentAmount, (float)maxAmount);
	}

	protected Color GetColorInBetween(float currentAmount, float maxAmount) {
		Color colorToReturn = colorOne;

		float damageInPercentage = (currentAmount / maxAmount);

		colorToReturn = new Color(colorOne.r + (differenceBetweenColors.x * damageInPercentage),
		                          colorOne.g + (differenceBetweenColors.y * damageInPercentage),
		                          colorOne.b + (differenceBetweenColors.z * damageInPercentage),
		                          255);
		return colorToReturn;
	}
}
