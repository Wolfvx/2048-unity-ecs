using UnityEngine;

using Cubic.Game.Projectiles;

namespace Cubic.Game.Field {

	public interface IPopulateField {

		Projectile[] PopulateField(Transform[] locations);

	}

}