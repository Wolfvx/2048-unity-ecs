using UnityEngine;

using Cubic.Game.Field;
using Cubic.Game.Projectiles;

namespace Cubic.Game {

	public class GameManager : MonoBehaviour {

		[SerializeField] private Player _player = null;
		[SerializeField] private FieldManager _field_manager = null;
		[SerializeField] private FieldGenerator _field_generator = null;
		[SerializeField] private ProjectileFactory _projectile_factory = null;
		[SerializeField] private ControlsInput.InputController[] _input_controllers = null;
		[SerializeField] private ScoreTracker _score_tracker = null;
		[SerializeField] private WinLooseDisplay _win_loose_display = null;

		public void StartGame() {
			_score_tracker.ResetScore();
			_win_loose_display.ResetPanels();
			_projectile_factory.Prepare();
			_field_manager.Prepare();
			_player.Prepare();
			RunGame();
		}

		public void RunGame() {
			_field_manager.Run(true);
			_player.Run(true);
		}

		public void StopGame() {
			_field_manager.Run(false);
			_player.Run(false);
		}

		public void OnWin() {
			StopGame();
			_win_loose_display.OnWin();
		}

		public void OnLoose() {
			StopGame();
			_win_loose_display.OnLoose();
		}

		private void Awake() {
			if (_player == null ||
				_field_manager == null ||
				_field_generator == null ||
				_projectile_factory == null ||
				_input_controllers == null ||
				_score_tracker == null ||
				_win_loose_display == null) {
				Debug.LogError("Not all fields were initialized");
			}
			for (int i = 0; i < _input_controllers.Length; i++) {
				_input_controllers[i].RegisterPlayerControls(_player);
			}
			_player.RegisterGetProjectile(_field_manager);
			_player.RegisterScoreInterface(_score_tracker);
			_field_manager.RegisterProjectileFactory(_projectile_factory);
			_field_manager.RegisterFieldGenerator(_field_generator);
			_field_generator.RegisterProjectileFactory(_projectile_factory);
			_projectile_factory.RegisterInterfaces(_field_manager, _field_manager);
		}

	}

}
