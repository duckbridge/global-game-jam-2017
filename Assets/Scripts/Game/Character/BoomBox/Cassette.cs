using UnityEngine;
using System.Collections;

public class Cassette : DispatchBehaviour {

	public float rotationSpeed = .5f;
	public float flySpeed = 1f;

	public enum CassetteAction {PLAY, STOP }
	public CassetteAction cassetteAction;

	private Transform target;
	private Transform targetAfterFirst;

	private bool isFlying = false;
	private bool isRotating = false;
	private TextMesh textOutput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(isFlying) {
			Vector3 direction = MathUtils.CalculateDirection(target.position, this.transform.position);
			this.transform.position += direction * flySpeed;

			if(Vector3.Distance(target.position, this.transform.position) < .5f) {
				if(targetAfterFirst) {
					target = targetAfterFirst;
					targetAfterFirst = null;
				} else {
					isFlying = false;
					OnFlyingDone();
				}
			}

		}
	}

	public void FlyToBoomBox(Transform inbetweenTarget) {
		BoomBox boombox = SceneUtils.FindObject<BoomBox>();

		this.AddEventListener(boombox.gameObject);
		this.cassetteAction = CassetteAction.PLAY;
		this.target = boombox.gameObject.transform;

		if(inbetweenTarget) {
			target = inbetweenTarget;
			this.targetAfterFirst = boombox.gameObject.transform;
		}

		StartFlying();
	}

	public void FlyToMusicManager(Transform inbetweenTarget) {
		MusicManager musicManager = SceneUtils.FindObject<MusicManager>();

		this.AddEventListener(musicManager.gameObject);
		this.cassetteAction = CassetteAction.STOP;
		this.target = musicManager.transform;
	
		if(inbetweenTarget) {
			target = inbetweenTarget;
			this.targetAfterFirst = musicManager.transform;
		}

		StartFlying();
	}

	private void StartFlying() {
		isFlying = true;
	}

	public void ToggleRotating(bool isRotating) {
		this.isRotating = isRotating;
	}

	private void OnFlyingDone() {
		DispatchMessage("OnArrivedAtDestination", this);
	}

	public void SetForegroundText(string newText) {
		this.transform.Find("CassetteSprites/CassetteText").GetComponent<TextMesh>().text = newText;
	}

    public void StartTextScrolling() {
        if(this.transform.Find("CassetteSprites/CassetteText").GetComponent<HorizontalScrollingText>()) {
            this.transform.Find("CassetteSprites/CassetteText").GetComponent<HorizontalScrollingText>().ScrollText();
        }
    }

	public void SetForegroundColor(Color color) {
		this.transform.Find("CassetteSprites/Foreground").GetComponent<SpriteRenderer>().color = color;
	}
}
