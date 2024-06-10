using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cubic.Game {

	public class GameSceneManager : SceneManager {

		[SerializeField] private GameManager _game_manager = null;

		protected override void Awake() {
			base.Awake();
		}

		private void Start() {
			_game_manager.StartGame();
		}

	}

}
