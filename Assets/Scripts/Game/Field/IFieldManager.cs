using UnityEngine;

using Cubic.Game.Projectiles;

namespace Cubic.Game.Field {

	public interface IFieldManager {

		void TrackProjectile(Projectile projectile);
		void RemoveProjectile(Projectile projectile);

	}

}
