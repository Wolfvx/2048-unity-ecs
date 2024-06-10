using UnityEngine;

namespace Cubic.Game.ControlsInput {

	public interface IPlayerControls {

		void OnLaunch();
		void OnMoveProjectile(Vector2 direction);

	}

}
