using UnityEngine;

using Cubic.Game.Projectiles;

namespace Cubic.Game.Field {

	public class FieldGenerator : MonoBehaviour, IPopulateField {

		private IProjectileFactory _projectile_factory = null;

		public void RegisterProjectileFactory(IProjectileFactory factory) {
			_projectile_factory = factory;
		}

		public Projectile[] PopulateField(Transform[] locations) {
			Projectile[] ret = new Projectile[locations.Length];
			for (int i = 0; i < locations.Length; i++) {
				var proj = _projectile_factory.CreateProjectile(PROJECTILE_TYPE.CUBE, locations[i].position) as CubeProjectile;
				if (i / 5 == 0) proj.SetCubeValue(16);
				if (i / 5 == 1) proj.SetCubeValue(8);
				if (i / 5 == 2) proj.SetCubeValue(4);
				if (i / 5 == 3) proj.SetCubeValue(2);
				ret[i] = proj;
			}
			return ret;
		}
	}

}
