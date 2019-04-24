using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyUtil  {
	private static float baseBlinkTime = .35f;
	private static float blinkTimeIncrement = .15f;

	public static float CalculateBlinkTime(Room room) {
		List<Enemy> enemiesDeathBlinking = new List<Enemy> (room.GetComponentsInChildren<Enemy> ());
		int amountOfEnemiesDeathBlinking = enemiesDeathBlinking.FindAll(enemy => enemy.IsDeathBlinking()).Count;

		return baseBlinkTime + (amountOfEnemiesDeathBlinking * blinkTimeIncrement);
	}
}
