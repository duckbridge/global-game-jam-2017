using UnityEngine;
using System.Collections;

public class PowerUpComponent : MonoBehaviour {

	public float tinySoundBlastFrameResetTimeout = 1.5f;
	public MusicAura sphereAura, crossAura, diamondAura, bulletAura;

	private bool canThrowWeapon;
	private MusicAuraTypes currentMusicAuraType = MusicAuraTypes.Sphere;

	private MusicAura currentMusicAura;

	private SoundObject resetTinyBlastFramesSound;
    
    private bool hasUnlockedRolling = false;
	private bool hasUnlockedSecondAttack = false;
	private bool hasUnlockedThirdAttack = false;

	// Use this for initialization
	void Start () {
		resetTinyBlastFramesSound = this.transform.Find("Sounds/ResetFramesSound").GetComponent<SoundObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPickedUp(ShopItem shopItem, bool destroyAfter) {

		switch(shopItem.shopItemType) {
			case ShopItem.ShopItemType.Heart:
				GetComponent<Player>().OnHealthPickedUp(null);
			break;

			case ShopItem.ShopItemType.ExtraHeart:
				GetComponent<Player>().OnExtraHealthPickedUp();
			break;

		}

		if(destroyAfter) {
			Destroy(shopItem.gameObject);
		}
	}

	public void SwapMusicAuraType(MusicAuraTypes newAuraType) {

		this.currentMusicAuraType = newAuraType;

		switch(newAuraType) {
			case MusicAuraTypes.Cross:
				this.currentMusicAura = crossAura;

			break;

			case MusicAuraTypes.Diamond:
				this.currentMusicAura = diamondAura;
			break;

			case MusicAuraTypes.Sphere:
				this.currentMusicAura = sphereAura;
			break;

            case MusicAuraTypes.Bullet:
                this.currentMusicAura = bulletAura;
            break;

		}
	}

	public void DoSoundBlast(MusicAuraTypes newMusicAuraType) {
		SwapMusicAuraType(newMusicAuraType);
		currentMusicAura.DoSoundBlast();
	}

	public void DoTinySoundBlast(MusicAuraTypes newMusicAuraType) {
		SwapMusicAuraType(newMusicAuraType);
		currentMusicAura.DoTinySoundBlast();

		CancelInvoke("ResetFrameOverride");
		Invoke ("ResetFrameOverride", tinySoundBlastFrameResetTimeout);
	}

	private void ResetFrameOverride() {
		resetTinyBlastFramesSound.Play();

		sphereAura.GetTinyBlastAnimation().ResetLastFrameOverride();
		diamondAura.GetTinyBlastAnimation().ResetLastFrameOverride();
		crossAura.GetTinyBlastAnimation().ResetLastFrameOverride();
        bulletAura.GetTinyBlastAnimation().ResetLastFrameOverride();
	}

    public void SetBlastDamageIncrementAmount(float sphereDamageIncrementAmount, float crossDamageIncrementAmount, float diamondDamageIncrementAmount, float bulletDamageIncrementAmount) {
		sphereAura.SetBlastDamageIncrementAmount(sphereDamageIncrementAmount);
		crossAura.SetBlastDamageIncrementAmount(crossDamageIncrementAmount);
		diamondAura.SetBlastDamageIncrementAmount(diamondDamageIncrementAmount);
        bulletAura.SetBlastDamageIncrementAmount(bulletDamageIncrementAmount);
	}
	
    public void SetTinyBlastDamageIncrementAmount(float tinySphereDamageIncrementAmount, float tinyCrossDamageIncrementAmount, float tinyDiamondDamageIncrementAmount, float tinyBulletDamageIncrementAmount) {
		sphereAura.SetTinyBlastDamageIncrementAmount(tinySphereDamageIncrementAmount);
		crossAura.SetTinyBlastDamageIncrementAmount(tinyCrossDamageIncrementAmount);
		diamondAura.SetTinyBlastDamageIncrementAmount(tinyDiamondDamageIncrementAmount);
        bulletAura.SetTinyBlastDamageIncrementAmount(tinyBulletDamageIncrementAmount);
	}

	public MusicAuraTypes GetCurrentAuraType() {
		return this.currentMusicAuraType;
	}

	public void EnableWeaponThrowing() {
		canThrowWeapon = true;
	}

	public bool CanThrowWeapon() {
		return canThrowWeapon;
	}

    public bool HasUnlockedRolling() {
        return hasUnlockedRolling;
    }
    
    public void UnlockRolling() {
        hasUnlockedRolling = true;
    }      

	public bool HasUnlockedSecondAttack() {
		return hasUnlockedSecondAttack;
	}

	public void UnlockSecondAttack() {
		hasUnlockedSecondAttack = true;
	}      

	public bool HasUnlockedThirdAttack() {
		return hasUnlockedThirdAttack;
	}

	public void UnlockThirdAttack() {
		hasUnlockedThirdAttack = true;
	}      
}
