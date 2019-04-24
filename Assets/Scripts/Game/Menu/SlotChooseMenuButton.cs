using UnityEngine;
using System.Collections;

public class SlotChooseMenuButton : MenuButtonWithId {

	public SpriteRenderer[] allBackgroundSpritesInOrder;
	private SpriteRenderer backgroundRenderer, border;
	private TextMesh townName, slotText, dataTextMesh;

	private bool hasInitialized = false;

	public virtual void Awake() {
		Initialize ();
	}

	public void Initialize() {

		if (!this.hasInitialized) {
			backgroundRenderer = this.transform.Find ("Background").GetComponent<SpriteRenderer> ();
			townName = this.transform.Find ("TownName").GetComponent<TextMesh> ();
			slotText = this.transform.Find ("SlotText").GetComponent<TextMesh> ();
			dataTextMesh = this.transform.Find ("ContainsData").GetComponent<TextMesh> ();

			border = this.transform.Find ("Border").GetComponent<SpriteRenderer> ();

			this.originalColor = border.color;
			this.originalText = slotText.text;

			hasSetOriginalText = true;
			this.hasInitialized = true;
		}
	}

	public void ShowData(SerializablePlayerDataSummary dataToShow, string dataText, Color dataTextColor) {

		dataTextMesh.text = dataText;
		dataTextMesh.color = dataTextColor;

		if (dataToShow != null) {
			Initialize ();
			string townNameByData = GetTownNameByLastVisitedVillage (dataToShow.lastVillageVisited);

			backgroundRenderer.sprite = allBackgroundSpritesInOrder [dataToShow.lastVillageVisited].sprite;
			townName.text = townNameByData.ToUpper();

		} else {
			ResetData (dataText);
		}
	}

	public override void SetText(string text) {
		slotText.text = text;
	}

	private void ResetData(string dataText) {
		townName.text = "";
		dataTextMesh.text = dataText;
		backgroundRenderer.sprite = allBackgroundSpritesInOrder [allBackgroundSpritesInOrder.Length - 1].sprite;
	}

	private string GetTownNameByLastVisitedVillage(int lastVisitedVillage) {
		switch (lastVisitedVillage) {
			case 0:
				return "Junkyard";
			break;

			case 1:
				return "Omega";
			break;

			case 2:
				return "CosmoCrater";
			break;

			case 3:
				return "LA2019";
			break;

			case 4:
				return "Matrix2199";
			break;

			case 5:
				return "RoboVillage";
			break;

			default:
				return "Junkyard";
			break;
		}
	}

	public override void OnSelected() {
		isSelected = true;
		SetColor (selectColor);
	}

	public override void OnUnSelected() {
		isSelected = false;
		SetColor (originalColor);
	}

	public override void SetOriginalColor (Color color) {
		base.SetOriginalColor (color);
		if (!isSelected) {
			SetColor (color);
		}
	}

	private void SetColor(Color color) {
		slotText.color = color;
		border.color = color;
		townName.color = color;
	}
}
