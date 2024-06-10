using UnityEngine;

namespace Cubic.Game.Projectiles {

	public interface IProjectileFactory {

		Projectile CreateProjectile(PROJECTILE_TYPE type, Vector3 position);
		void ReturnProjectile(Projectile projectile);

	}

}
