using UnityEngine;

namespace Cubic.Game.Projectiles {

	public interface ILaunchableProjectile {

		void Launch(float strength);
		void SetPosition(Vector3 position);

	}

}
