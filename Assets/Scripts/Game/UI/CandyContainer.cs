using UnityEngine;
using System.Collections;

public class CandyContainer : MonoBehaviour {
	public int candyAmount = 0;
	
	private TextMesh candyOutput;
	private Player player;
	
	void Awake() {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCandyOutputText() {
		this.transform.Find("CandyAmount").GetComponent<TextMesh>().text = candyAmount+"";
	}

	public void OnCandyPickedUp(CandyDrop candyDrop) {

		++candyAmount;

		candyOutput.text = candyAmount + "";
	}

	public void DecrementCandyAmount(int decrementAmount) {
		this.candyAmount -= decrementAmount;
		candyOutput.text = candyAmount + "";
	}

	public void Initialize() {
		player = SceneUtils.FindObject<Player>();
		player.AddEventListener(this.gameObject);
		player.SetCandyContainer(this);

		candyOutput = this.transform.Find("CandyAmount").GetComponent<TextMesh>();
	}
}
