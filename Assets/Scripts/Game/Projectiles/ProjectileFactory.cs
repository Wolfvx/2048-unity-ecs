using System.Collections.Generic;
using UnityEngine;

using Cubic.Game.Field;

namespace Cubic.Game.Projectiles {

	public class ProjectileFactory : MonoBehaviour, IProjectileFactory {

		[SerializeField] private GameObject _cube_prefab = null;
		[SerializeField] private int _max_projectiles = 20;

		private LinkedList<Projectile> _cube_pool = null;
		private IFieldManager _field_manager = null;
		private ICubeValueChangeNotify _cube_notify = null;

		public void RegisterInterfaces(IFieldManager manager, ICubeValueChangeNotify cube_notify) {
			_field_manager = manager;
			_cube_notify = cube_notify;
		}

		public void Prepare() {
			createAndPopulatePool();
		}

		public Projectile CreateProjectile(PROJECTILE_TYPE type, Vector3 position) {
			Projectile ret = null;
			switch (type) {
				case PROJECTILE_TYPE.CUBE:
					if (_cube_pool.Count == 0) {
						// LOOSE Condition?
						Debug.LogError("No more free cubes!");
						break;
					}
					ret = _cube_pool.First.Value;
					ret.transform.position = position;
					ret.transform.rotation = Quaternion.identity;
					ret.gameObject.SetActive(true);
					_cube_pool.RemoveFirst();
					break;
				default:
					throw new System.NotSupportedException("Unknown PROJECTILE_TYPE " + type.ToString());
			}
			return ret;
		}

		public void ReturnProjectile(Projectile projectile) {
			switch (projectile.ProjectileType) {
				case PROJECTILE_TYPE.CUBE:
					projectile.gameObject.SetActive(false);
					_cube_pool.AddLast(projectile);
					break;
				default:
					throw new System.NotSupportedException("Unknown PROJECTILE_TYPE " + projectile.ProjectileType.ToString());
			}
		}

		private void Awake() {
			if (_cube_prefab == null) {
				Debug.LogError("Not all fields were initialized");
			}
		}

		private void createAndPopulatePool() {
			_cube_pool = new LinkedList<Projectile>();
			for (int i = 0; i < _max_projectiles; i++) {
				var obj = Instantiate(_cube_prefab, new Vector3(-1000, 0, 0), Quaternion.identity, transform);
				var projectile = obj.GetComponent<CubeProjectile>();
				projectile.RegisterFieldManager(_field_manager);
				projectile.RegisterValueChangeNotify(_cube_notify);
				_cube_pool.AddLast(projectile);
				obj.SetActive(false);
			}
		}

	}

}
