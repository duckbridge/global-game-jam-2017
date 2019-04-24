using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteCropper : MonoBehaviour {

	public enum SplitType { TWO, FOUR, EIGHT };

	public static List<GameObject> SplitSpriteByInPieces(SpriteRenderer spriteToCrop, SplitType splitType) {
		return SplitSpriteByInPieces(spriteToCrop, splitType, -1f, false, Vector3.zero);
	}

	public static List<GameObject> SplitSpriteByInPieces(SpriteRenderer spriteToCrop, SplitType splitType, float intersectPercentage, bool cutsHorizontally, Vector3 extraRotation) {

		List<GameObject> splittedSprites = new List<GameObject>();
		int splitHorizontal = Random.Range (0, 2);

		switch(splitType) {

			case SplitType.TWO:
				
				if(intersectPercentage <= 0 || intersectPercentage >= 1f) {
					intersectPercentage = .5f;
				}

				if(cutsHorizontally) {
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, 1f, 0f, intersectPercentage, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, 1f, intersectPercentage, 1, extraRotation));
				} else {
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, intersectPercentage, 0f, 1f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, intersectPercentage, 1f, 0f, 1f, extraRotation));
				}

			break;

			case SplitType.FOUR:
			
				if(intersectPercentage <= 0 || intersectPercentage >= 1f) {
					intersectPercentage = .5f;
				}
					
				if(cutsHorizontally) {
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .5f, 0f, intersectPercentage, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, 1f, 0f, intersectPercentage, extraRotation));

					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .5f, intersectPercentage, 1f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, 1f, intersectPercentage, 1f, extraRotation));

				} else {

					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, intersectPercentage, 0f, .5f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, intersectPercentage, 1f, 0f, .5f, extraRotation));
							
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, intersectPercentage, .5f, 1f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, intersectPercentage, 1f, .5f, 1f, extraRotation));
				}

			break;

			case SplitType.EIGHT:
				
				if(splitHorizontal == 0) {

					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .25f, 0f, .5f,extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .25f, .5f, 0f, .5f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, .75f, 0f, .5f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .75f, 1f, 0f, .5f, extraRotation));

					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .25f, .5f, 1f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .25f, .5f, .5f, 1f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, .75f, .5f, 1f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .75f, 1f, .5f, 1f, extraRotation));

				} else {
					
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .5f, 0f, .25f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .5f, .25f, .5f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .5f, .5f, .75f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, 0f, .5f, .75f, 1f, extraRotation));
							
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, 1f, 0f, .25f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, 1f, .25f, .5f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, 1f, .5f, .75f, extraRotation));
					splittedSprites.Add (SpriteCropper.CropSprite(spriteToCrop, .5f, 1f, .75f, 1f, extraRotation));
				}

			break;

		}

		return splittedSprites;
	}

	public static GameObject CropSprite(SpriteRenderer spriteToCrop, float startPercentageX, float cropPercentageX, float startPercentageY, float cropPercentageY, Vector3 extraRotation) {
		
		SpriteRenderer spriteRenderer = spriteToCrop;
		Sprite spriteToCropSprite = spriteRenderer.sprite;
		Texture2D spriteTexture = spriteToCropSprite.texture;
		Rect spriteRect = spriteToCropSprite.textureRect;
		
		GameObject croppedSpriteGO = new GameObject(spriteToCrop.name + "_Cropped");
	
		croppedSpriteGO.transform.localRotation = spriteToCrop.transform.localRotation;
		
		Rect croppedSpriteRect = spriteRect;
		
		float pixelsPerUnit = spriteToCrop.sprite.pixelsPerUnit;

		croppedSpriteRect.width = (spriteRect.width * cropPercentageX) - (spriteRect.width * startPercentageX);
		croppedSpriteRect.x = spriteToCropSprite.textureRect.x + (spriteRect.width * startPercentageX);
		croppedSpriteRect.height = (spriteRect.height * cropPercentageY) - (spriteRect.height * startPercentageY);
		croppedSpriteRect.y = spriteToCropSprite.textureRect.y + (spriteRect.height * startPercentageY);

		Sprite croppedSprite = 
			Sprite.Create(spriteTexture, croppedSpriteRect, new Vector3(.5f, .55f), pixelsPerUnit);

		SpriteRenderer cropSpriteRenderer = croppedSpriteGO.AddComponent<SpriteRenderer>();
	
		cropSpriteRenderer.sprite = croppedSprite;
		cropSpriteRenderer.color = spriteToCrop.color;

		Vector3 croppedSpritePosition = CalculateCroppedSpritePosition(spriteToCrop, croppedSpriteRect);

		croppedSpriteGO.transform.Rotate(extraRotation);
		croppedSpriteGO.transform.parent = spriteToCrop.transform.parent;
		croppedSpriteGO.transform.localScale = spriteToCrop.transform.localScale;
	
		croppedSpriteGO.transform.localPosition = croppedSpritePosition;

		return croppedSpriteGO;
	}

	private static Vector3 CalculateCroppedSpritePosition(SpriteRenderer spriteToCrop, Rect croppedSpriteRect) {

		Vector2 centerOfWholeImage = spriteToCrop.sprite.textureRect.center;
		Vector2 croppedSpriteCenter = croppedSpriteRect.center;
		
		Vector2 difference = croppedSpriteCenter - centerOfWholeImage;

		//Logger.Log("center of whole image : " + centerOfWholeImage + " center of cropped " + croppedSpriteCenter + " and diff " + difference);

		return new Vector3(difference.x/spriteToCrop.transform.localScale.x, 0f, difference.y/spriteToCrop.transform.localScale.y);
	}
}
