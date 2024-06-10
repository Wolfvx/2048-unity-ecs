using UnityEngine;

using Cubic.Game.Projectiles;
using Cubic.Game.Field;

namespace Cubic.Game {

	public class Player : MonoBehaviour, ControlsInput.IPlayerControls, ILoadNextProjectile {

		[SerializeField] private Transform _spawn_location = null;
		[SerializeField] private float _launch_strength = 200f;
		[SerializeField] private float _max_side_displacement = 2.4f;

		private bool _run = false;

		private ILaunchableProjectile _current_projectile = null;
		private IScoreIncrement _score_tracker = null;
		private INextProjectile _next_projectile = null;
		private Vector3 _launch_position = Vector3.zero;

		public void RegisterGetProjectile(INextProjectile getter) {
			_next_projectile = getter;
		}

		public void RegisterScoreInterface(IScoreIncrement score) {
			_score_tracker = score;
		}

		public void Prepare() {
			_run = false;
			_next_projectile.GetNextProjectile(this, _spawn_location.position, true);
		}

		public void Run(bool value) {
			_run = value;
		}

		public void LoadProjectile(ILaunchableProjectile projectile) {
			_current_projectile = projectile;
		}

		public void OnLaunch() {
			if (!_run) return;
			if (_current_projectile == null) return; // Waiting for next projectile
			_current_projectile.Launch(_launch_strength);
			_current_projectile = null;
			_launch_position = _spawn_location.position;
			_next_projectile.GetNextProjectile(this, _spawn_location.position);
			_score_tracker.IncrementScore();
		}

		public void OnMoveProjectile(Vector2 direction) {
			if (!_run) return;
			if (_current_projectile == null) return; // Waiting for next projectile
			_launch_position.x += direction.x;
			_launch_position.x = Mathf.Clamp(_launch_position.x, -_max_side_displacement, _max_side_displacement);
			_current_projectile.SetPosition(_launch_position);
		}

		private void Awake() {
			if (_spawn_location == null) {
				Debug.LogError("Not all fields were initialized");
			}
		}

		private void Start() {
			_spawn_location.gameObject.SetActive(false); // hide spawn location
			_launch_position = _spawn_location.position;
		}
	}

}
