using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Cubic.Game.Projectiles;

namespace Cubic.Game.Field {

	public class FieldManager : MonoBehaviour, IFieldManager, INextProjectile, ICubeValueChangeNotify {

		[SerializeField] private Transform[] _spawn_locations = null;
		[SerializeField] private Transform _player_position = null;
		[SerializeField] private int _max_active_projectiles = 30;
		[SerializeField] private float _next_projectile_time = 1f;
		[SerializeField] private int _win_cube_value = 2048;
		[SerializeField] private float _closest_distance_to_player = 1f;

		[SerializeField] private UnityEvent _on_win_event = null;
		[SerializeField] private UnityEvent _on_loose_event = null;

		private bool _run = false;

		private IProjectileFactory _projectile_factory = null;
		private IPopulateField _field_generator = null;

		private List<Projectile> _active_projectiles = null;
		private const float CONDITION_CHECK_WAIT_TIME = 0.2f;
		private const int MIN_CUBE_VALUE = 2;

		public void RegisterProjectileFactory(IProjectileFactory factory) {
			_projectile_factory = factory;
		}

		public void RegisterFieldGenerator(IPopulateField generator) {
			_field_generator = generator;
		}

		public void GetNextProjectile(ILoadNextProjectile caller, Vector3 spawn_position, bool spawn_now) {
			if (spawn_now) {
				createProjectile(caller, spawn_position);
			} else {
				StartCoroutine(spawnNextProjectile(caller, spawn_position));
			}
		}

		public void TrackProjectile(Projectile projectile) {
			if (_active_projectiles.Contains(projectile)) return; // Make sure only 1 instance per projectile gets registered
			_active_projectiles.Add(projectile);
		}

		public void RemoveProjectile(Projectile projectile) {
			if (!_active_projectiles.Contains(projectile)) return;
			_active_projectiles.Remove(projectile);
			_projectile_factory.ReturnProjectile(projectile);
		}

		public void OnCubeValueChanged(int value) {
			if (value == _win_cube_value) _on_win_event?.Invoke();
		}

		public void Prepare() {
			_run = false;
			populateField();
		}

		public void Run(bool value) {
			_run = value;
		}

		private void Awake() {
			if (_spawn_locations == null ||
				_player_position == null) {
				Debug.LogError("Not all fields were initialized");
			}
			_active_projectiles = new List<Projectile>();
		}

		private void Start() {
			for (int i = 0; i < _spawn_locations.Length; i++) {
				_spawn_locations[i].gameObject.SetActive(false); // hide spawn locations
			}
		}

		private void populateField() {
			var projectiles = _field_generator.PopulateField(_spawn_locations);
			if (projectiles != null) {
				_active_projectiles.AddRange(projectiles);
			}
		}

		private int getNextBestCubeValue() {
			int result = getClosesCubeValue();
			if (result == -1) result = MIN_CUBE_VALUE;
			return result;
		}

		private int getClosesCubeValue() {
			float distance = 10000f;
			int result = -1;
			for (int i = 0; i < _active_projectiles.Count; i++) {
				var cube = _active_projectiles[i] as CubeProjectile;
				if (cube == null) continue;
				float next_dist = Mathf.Abs(cube.transform.position.z - _player_position.position.z);
				if (next_dist < distance) {
					distance = next_dist;
					result = cube.CubeValue;
				}
			}
			return result;
		}

		private bool checkIfAnyCubeGotTooClose() {
			for (int i = 0; i < _active_projectiles.Count; i++) {
				var cube = _active_projectiles[i] as CubeProjectile;
				if (cube == null) continue;
				float next_dist = Mathf.Abs(cube.transform.position.z - _player_position.position.z);
				if (next_dist < _closest_distance_to_player) {
					return true;
				}
			}
			return false;
		}

		private int getMostCommonLowestCubeValue() {
			Dictionary<int, int> dict =
					new Dictionary<int, int>();

			for (int i = 0; i < _active_projectiles.Count; i++) {
				var cube = _active_projectiles[i] as CubeProjectile;
				if (cube == null) continue;
				int key = cube.CubeValue;
				if (dict.ContainsKey(key)) {
					int freq = dict[key];
					freq++;
					dict[key] = freq;
				} else
					dict.Add(key, 1);
			}

			int min_count = 0, result = -1;

			foreach (KeyValuePair<int,
						int> pair in dict) {
				if (min_count < pair.Value) {
					result = pair.Key;
					min_count = pair.Value;
				} else {
					if (min_count == pair.Value && result > pair.Key) {
						result = pair.Key;
						min_count = pair.Value;
					}
				}
			}
			return result;
		}

		private IEnumerator spawnNextProjectile(ILoadNextProjectile caller, Vector3 spawn_position) {
			yield return new WaitForSeconds(_next_projectile_time);
			if (_active_projectiles.Count >= _max_active_projectiles) {
				int wait_count = 1000;
				while (true) {
					if (wait_count-- < 0) {
						Debug.LogError("WAITING TOO LONG!!");
						break;
					}
					yield return new WaitForSeconds(CONDITION_CHECK_WAIT_TIME);
					if (_active_projectiles.Count >= _max_active_projectiles) {
						break;
					}
					if (!areThereMovingProjectiles()) {
						_on_loose_event?.Invoke();
						yield break;
					}
				}
			}
			createProjectile(caller, spawn_position);
		}

		private void createProjectile(ILoadNextProjectile caller, Vector3 spawn_position) {
			var cube = _projectile_factory.CreateProjectile(PROJECTILE_TYPE.CUBE, spawn_position) as CubeProjectile;
			if (cube == null) throw new System.NullReferenceException();
			cube.SetCubeValue(getNextBestCubeValue());
			caller.LoadProjectile(cube);
		}

		private bool areThereMovingProjectiles() {
			for (int i = 0; i < _active_projectiles.Count; i++) {
				if (_active_projectiles[i].isMoving()) return true;
			}
			return false;
		}

		private void Update() {
			if (_run) {
				if (checkIfAnyCubeGotTooClose()) {
					_run = false;
					_on_loose_event?.Invoke();
				}
			}
		}

	}

}
