using InControl;

public class InputPattern {

	public PlayerAction playerAction;

	public double start;
	public double range;

	public int framesRequired = 1;
	private int amountOfFrames = 0;

	private SoundObject sound;
	private bool hasPlayedSound = false;

	public InputPattern(double start, double range, PlayerAction playerAction, SoundObject sound, int framesRequred = 1) {
		this.start = start;
		this.range = range;

		this.framesRequired = framesRequred;
		this.playerAction = playerAction;

		this.sound = sound;
	}

	public void ResetPattern() {
		amountOfFrames = 0;
		hasPlayedSound = false;
	}

	public void UpdateFramesHeldBy(int amount) {
		amountOfFrames += amount;
	}

	public bool IsFinished() {
		return amountOfFrames >= framesRequired;
	}

	public void ResumeSound() {
		this.sound.Play (false);
	}

	public void PlayOnce() {
		if (!hasPlayedSound) {
			this.sound.Play (true);
			hasPlayedSound = true;
		}
	}

	public void PlaySound() {
		this.sound.Play (true);
	}

	public void SetSoundEffect(SoundObject newSound) {
		this.sound = newSound;
	}

	public string GetSoundName() {
		return this.sound.name;
	}
}
