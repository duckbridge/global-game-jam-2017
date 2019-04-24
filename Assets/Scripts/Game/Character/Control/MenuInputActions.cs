using UnityEngine;
using System.Collections;

namespace InControl {

	public class MenuInputActions : PlayerActionSet {

		public PlayerAction left;
		public PlayerAction right;

		public PlayerAction up;
		public PlayerAction down;

		public PlayerAction one, two, three, four;

		public PlayerOneAxisAction moveHorizontally;
		public PlayerOneAxisAction moveVertically;

		public PlayerOneAxisAction throwHorizontally;
		public PlayerOneAxisAction throwVertically;

		public PlayerAction menuSelect;
	
		public MenuInputActions() {

			one = CreatePlayerAction(" Lan 1");
			two = CreatePlayerAction(" Lan 2");
			three = CreatePlayerAction(" Lan 3");
			four = CreatePlayerAction(" Lan 4");

			left = CreatePlayerAction( "Move Left" );
			right = CreatePlayerAction( "Move Right" );

			up = CreatePlayerAction( "Move Up" );
			down = CreatePlayerAction( "Move Down" );

			moveHorizontally = CreateOneAxisPlayerAction( left, right );
			moveVertically = CreateOneAxisPlayerAction( down, up );

			menuSelect = CreatePlayerAction(" menu select ");
		}
	}
}