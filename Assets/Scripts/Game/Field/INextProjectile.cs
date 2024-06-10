using UnityEngine;

namespace Cubic.Game.Field {

	public interface INextProjectile {

		void GetNextProjectile(ILoadNextProjectile caller, Vector3 spawn_position, bool spawn_now = false);

	}

}