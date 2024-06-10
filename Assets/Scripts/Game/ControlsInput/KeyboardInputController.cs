using UnityEngine;

namespace Cubic.Game.ControlsInput {

	public class KeyboardInputController : InputController {

		[SerializeField] private float _horizontal_authority = 0.2f;

		protected override void Start() {
			base.Start();
		}

		private void Update() {
			processKeyboard();
		}

		private void processKeyboard() {
			float horizontal = Input.GetAxis("Horizontal");
			if (horizontal > 0.1f || horizontal < -0.1f) {
				horizontal *= _horizontal_authority;
				_player_controls.OnMoveProjectile(new Vector2(horizontal, 0));
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				_player_controls.OnLaunch();
			}
		}

	}

}
