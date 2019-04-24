using UnityEngine;
using System.Collections;

public class BoomboxMusicSwapUI : UIElement {

	public Cassette currentCassette, newCassette;

	public iTween.EaseType easeInType, easeOutType;

	public float cassetteShowTime = .5f;
	public float cassetteMoveTime = 2f;

	private Transform cassetteShowPosition;
	private Transform cassetteHidePosition;

	void Awake () {
		cassetteShowPosition = this.transform.Find("CassetteShowTransform");
		cassetteHidePosition = this.transform.Find("CassetteHideTransform");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Show (bool force) {

	}

	public override void Hide () {
		base.Hide ();
	}

	public void OnMusicSwap(SoundObjectWithInfo newCassetteInfo, SoundObjectWithInfo currentCassetteInfo) {
		base.Show(true);

        iTween.StopByName(this.gameObject, "ShowingCassette");
        iTween.StopByName(this.gameObject, "HidingCassette");

		currentCassette.transform.localPosition = cassetteShowPosition.localPosition;

		currentCassette.SetForegroundColor(currentCassetteInfo.frontColor);
		currentCassette.SetForegroundText(newCassetteInfo.title);

		currentCassette.gameObject.SetActive(true);

		HideCurrentCassette();

		newCassette.SetForegroundColor(newCassetteInfo.frontColor);
		newCassette.SetForegroundText(newCassetteInfo.title);

		ShowNewCassette();

	}

	public void OnMusicSwap(SoundObjectWithInfo newCassetteInfo) {
		base.Show(true);

        iTween.StopByName(this.gameObject, "ShowingCassette");
        iTween.StopByName(this.gameObject, "HidingCassette");

		newCassette.SetForegroundColor(newCassetteInfo.frontColor);
		newCassette.SetForegroundText(newCassetteInfo.title);
		
		ShowNewCassette();
		
	}

	public void ShowNewCassette() {

		newCassette.transform.localPosition = cassetteHidePosition.localPosition;
		newCassette.gameObject.SetActive(true);

		ShowCassette(newCassette);

	}

	public void OnNewCassetteShown() {
        currentCassette = newCassette;
        //newCassette.StartTextScrolling();
		MusicManager musicManager = SceneUtils.FindObject<MusicManager>();
		if (musicManager) {
			musicManager.PlayNewMusicOnReceive ();
		}
        
        if(cassetteMoveTime > 0) {
            CancelInvoke("HideCurrentCassette");
            Invoke("HideCurrentCassette", cassetteShowTime);
        }
	}

	private void ShowCassette(Cassette cassetteToMove) {
		
		iTween.MoveTo(cassetteToMove.gameObject, 
      		new ITweenBuilder().SetLocal()
              .SetPosition(cassetteShowPosition.transform.localPosition)
              .SetEaseType(easeInType)
              .SetName("ShowingCassette")
              .SetTime(cassetteMoveTime)
              .SetOnCompleteTarget(this.gameObject)
              .SetOnComplete("OnNewCassetteShown")
              .Build());
	}

	private void HideCurrentCassette() {

		iTween.MoveTo(currentCassette.gameObject, 
    		new ITweenBuilder().SetLocal()
             .SetPosition(cassetteHidePosition.transform.localPosition)
             .SetEaseType(easeInType)
             .SetName("HidingCassette")
             .SetTime(cassetteMoveTime)
             .Build());
	}
}
