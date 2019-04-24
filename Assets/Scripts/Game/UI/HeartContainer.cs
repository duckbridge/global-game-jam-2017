using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartContainer : MonoBehaviour {
	
	public int timesThatHearthsBlink = 3;
	public float healthBlinkTime = 0.15f;
	public float oneHealthLeftBlinkTime = .3f;

	public List<Heart> hearts;
	public Heart heartPrefab;
	public float hearthOffset = 3f;

	private int currentHealth = GameSettings.MAX_HEARTS;
	private int extraHearts = 0;

	private Player player;
	private MinimapBuilder minimapBuilder;

	public void Start () {
	}

	public void Initialize() {
		minimapBuilder = SceneUtils.FindObject<MinimapBuilder>();

		player = SceneUtils.FindObject<Player>();
		player.AddEventListener(this.gameObject);

		foreach(Heart heart in hearts) {
			heart.SetColor(minimapBuilder.blockColor);
		}
	}

	public void OnHeartPickedUp() {
		if(currentHealth < GameSettings.MAX_HEARTS) {
			
			if(currentHealth == 1) {
				BlinkAllHearts(0, healthBlinkTime);
			}
			
			++currentHealth;
			ActivateHeartOrSpawnNew(currentHealth - 1, true);
		}
		
		player.OnHealthChanged(currentHealth);
		
		player.PlayHPPickupSound();
	}

	public void OnHeartPickedUp(HeartDrop heartDrop) {
		OnHeartPickedUp();
		if(heartDrop) {
			Destroy(heartDrop.gameObject);
		}
	}

	public void ShrinkHeartsTo(int newAmount) {
		currentHealth = newAmount;
		
		for(int i = hearts.Count-1 ; i > newAmount-1 ; i--) {
			hearts[i].Awake();
			hearts[i].HideAll();
			hearts.RemoveAt(i);
		}
	}

	public void OnFullyRegenerateHearts() {
		for(int i = 0 ; i < GameSettings.MAX_HEARTS ; i++) {
			ActivateHeartOrSpawnNew(i, false);
			hearts[i].ShowOverlay(true);
		}

		currentHealth = GameSettings.MAX_HEARTS;
	}

	public void ExpandHeartsBy(int incrementAmount) {
		ExpandHeartsTo(GameSettings.MAX_HEARTS + incrementAmount);
	}

	public void ExpandHeartsTo(int newMaxAmount) {
		if(newMaxAmount > GameSettings.MAX_HEARTS) {
			GameSettings.MAX_HEARTS = newMaxAmount;

			for(int i = 0 ; i < newMaxAmount + 1 ; i++) {
				ActivateHeartOrSpawnNew(i, false);
			}

			currentHealth = newMaxAmount;
		
		}
	}

	public void OnPlayerHit(int damage) {
		if(currentHealth - damage > 0) {
			for(int i = 0; i < damage ;i++) {
				currentHealth--;
				DeactivateHeart();
			}

			if(currentHealth > 1) {
				BlinkAllHearts(timesThatHearthsBlink, healthBlinkTime);
			} else {
				BlinkAllHearts(-1, oneHealthLeftBlinkTime);
			}

		} else {
			for(int i = 0; i < damage ;i++) {
				if(currentHealth > 0) {
					currentHealth--;
					DeactivateHeart();
				}
			}
			player.OnDie();
		}

		player.OnHealthChanged(currentHealth);
	}

	private void BlinkAllHearts(int blinkTimes, float blinkTimeout) {
		foreach(Heart heart in hearts) {
			heart.Blink(blinkTimes, blinkTimeout);
		}
	}
	
	private void ActivateHeartOrSpawnNew(int currentHealthIndex, bool doBlink) {
		if(currentHealthIndex > hearts.Count) {
		
			Vector3 lastHeartPosition = hearts[hearts.Count - 1].transform.position;
			Vector3 spawnPosition = new Vector3(lastHeartPosition.x + hearthOffset, lastHeartPosition.y, lastHeartPosition.z);

			Heart heart = (Heart) GameObject.Instantiate(heartPrefab, spawnPosition, Quaternion.identity);

			heart.transform.parent = this.transform;
			heart.transform.localScale = new Vector3(.5f, .5f, 1f);
			heart.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

			heart.SetColor(minimapBuilder.blockColor);

			hearts.Add (heart);
			
		} else {
			if(currentHealthIndex > 0 && currentHealthIndex < hearts.Count) {
				hearts[currentHealthIndex].SetColor(minimapBuilder.blockColor);
				hearts[currentHealthIndex].ShowOverlay(doBlink);
			}
		}
	}

	public void DeactivateAllHearts() {
		hearts.ForEach(heart => heart.HideOverlay());
	}
	
	private void DeactivateHeart() {
		if(currentHealth > -1) {
			hearts[currentHealth].HideOverlay();
		}
	}
}

