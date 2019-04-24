using UnityEngine;
using System.Collections;

namespace InControl {

	public class PlayerInputActions : PlayerActionSet {

		public PlayerAction left;
		public PlayerAction right;

		public PlayerAction up;
		public PlayerAction down;

		public PlayerAction rStickLeft;
		public PlayerAction rStickRight;
		
		public PlayerAction rStickUp;
		public PlayerAction rStickDown;

		public PlayerAction pause;
		public PlayerAction back;

		public PlayerOneAxisAction moveHorizontally;
		public PlayerOneAxisAction moveVertically;

		public PlayerOneAxisAction throwHorizontally;
		public PlayerOneAxisAction throwVertically;

		public PlayerAction interact;
		public PlayerAction secondAttack, thirdAttack;
		public PlayerAction roll;

		public PlayerAction menuSelect, nextTrack, previousTrack;
		public PlayerAction dance;

		public PlayerInputActions() {

			left = CreatePlayerAction( "left" );
			right = CreatePlayerAction( "right" );

			up = CreatePlayerAction( "up" );
			down = CreatePlayerAction( "down" );

			rStickLeft = CreatePlayerAction( "rStickLeft" );
			rStickRight = CreatePlayerAction( "rStickRight" );
			
			rStickUp = CreatePlayerAction( "rStickUp" );
			rStickDown = CreatePlayerAction( "rStickDown" );

			moveHorizontally = CreateOneAxisPlayerAction( left, right );
			moveVertically = CreateOneAxisPlayerAction( down, up );

			throwHorizontally = CreateOneAxisPlayerAction( rStickLeft, rStickRight );
			throwVertically = CreateOneAxisPlayerAction( rStickDown, rStickUp );

			interact = CreatePlayerAction("interact");
			secondAttack = CreatePlayerAction("secondAttack");
			thirdAttack = CreatePlayerAction("thirdAttack");

			nextTrack = CreatePlayerAction("nextTrack");
			previousTrack = CreatePlayerAction("previousTrack");

			roll = CreatePlayerAction("roll");

			pause = CreatePlayerAction("pause");
			back = CreatePlayerAction ("back");
			menuSelect = CreatePlayerAction("menuSelect");

			dance = CreatePlayerAction("select");
		}
	}
}