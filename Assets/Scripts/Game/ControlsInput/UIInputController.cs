using UnityEngine;
using UnityEngine.EventSystems;

namespace Cubic.Game.ControlsInput {

	public class UIInputController : InputController, IDragHandler {

		[SerializeField] private float _launch_threshhold = 100f;
		[SerializeField] private float _horizontal_authority = 0.1f;

		protected override void Start() {
			base.Start();
		}

		private void Update() {
			
		}

		void IDragHandler.OnDrag(PointerEventData eventData) {
			_player_controls.OnMoveProjectile(new Vector2(eventData.delta.x * _horizontal_authority, 0));
			if (eventData.delta.y > _launch_threshhold) {
				_player_controls.OnLaunch();
			}
		}

	}

}
