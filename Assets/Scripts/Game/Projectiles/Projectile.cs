using UnityEngine;

using Cubic.Game.Field;

namespace Cubic.Game.Projectiles {

	[RequireComponent(typeof(Rigidbody))]
	public abstract class Projectile : MonoBehaviour {

		public virtual PROJECTILE_TYPE ProjectileType => PROJECTILE_TYPE.NONE;

		protected IFieldManager _field_manager = null;
		protected Rigidbody _rigidbody = null;
		protected bool _alive = false;

		public void RegisterFieldManager(IFieldManager manager) {
			_field_manager = manager;
		}

		public void BeConsume() {
			_alive = false;
			_field_manager.RemoveProjectile(this);
			_rigidbody.velocity = Vector3.zero; // Reset rigidbody forces
			_rigidbody.angularVelocity = Vector3.zero;
		}

		public bool isMoving() {
			if (_rigidbody.velocity.magnitude > 0.01f) return true;
			return false;
		}

		protected virtual void Awake() {
			if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
		}

		protected void trackProjectile() {
			_alive = true;
			_field_manager.TrackProjectile(this);
		}

	}

}
