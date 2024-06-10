using System.Collections;
using UnityEngine;

namespace Cubic.Game.Projectiles {

	[RequireComponent(typeof(Renderer))]
	public class CubeProjectile : Projectile, ILaunchableProjectile {

		public int CubeValue { get; private set; } = 0;

		public override PROJECTILE_TYPE ProjectileType => PROJECTILE_TYPE.CUBE;

		private ICubeValueChangeNotify _value_change_notify = null;
		private Renderer _renderer = null;

		public void RegisterValueChangeNotify(ICubeValueChangeNotify notify) {
			_value_change_notify = notify;
		}

		public void SetCubeValue(int number) {
			CubeValue = number;
			_renderer.material.mainTexture = DataKeeper.GetCubeTextureByValue(number);
		}

		public void Launch(float strength) {
			_rigidbody.AddForce(new Vector3(0, 0, strength));
			StartCoroutine(trackProjectileAfter(0.2f));
		}

		public void SetPosition(Vector3 position) {
			transform.position = position;
		}

		protected override void Awake() {
			base.Awake();
			if (_renderer == null) _renderer = GetComponent<Renderer>();
		}

		private IEnumerator trackProjectileAfter(float time) {
			yield return new WaitForSeconds(time);
			trackProjectile();
		}

		private void OnCollisionEnter(Collision collision) {
			if (!_alive) return;
			var other_cube = collision.gameObject.GetComponent<CubeProjectile>();
			if (other_cube != null) {
				if (other_cube.CubeValue == CubeValue) {
					CubeValue += CubeValue;
					SetCubeValue(CubeValue);
					other_cube.BeConsume();
					_value_change_notify.OnCubeValueChanged(CubeValue);
				}
				
			}
		}

	}

}
