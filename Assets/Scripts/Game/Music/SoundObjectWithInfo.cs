using UnityEngine;
using System.Collections;

public class SoundObjectWithInfo : FadingAudio {

	public Color frontColor = Color.red;
	public TileType tileType = TileType.none;

	public string title;
	public string description;

	public int sphereBlastPower = 10;
	public int tinySphereBlastPower = 3;

	public int diamondBlastPower = 0;
	public int tinyDiamondBlastPower = 0;

	public int crossBlastPower = 10;
	public int tinyCrossBlastPower = 3;

    public int bulletBlastPower = 10;
    public int tinyBulletBlastPower = 3;

	public int onBeatRange = 100;

}
