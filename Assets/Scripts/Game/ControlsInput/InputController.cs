using UnityEngine;

namespace Cubic.Game.ControlsInput {

	public abstract class InputController : MonoBehaviour {

		protected IPlayerControls _player_controls;

		public void RegisterPlayerControls(IPlayerControls controls) {
			_player_controls = controls;
		}

		protected virtual void Start() {
			if (_player_controls == null) {
				Debug.LogError("_player_controlls wasn't assigned");
			}
		}

	}

}
