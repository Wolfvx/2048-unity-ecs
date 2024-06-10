using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cubic.Game {

	public class WinLooseDisplay : MonoBehaviour {

		[SerializeField] private GameObject _win_panel = null;
		[SerializeField] private GameObject _loose_panel = null;

		public void OnWin() {
			_win_panel.SetActive(true);
		}

		public void OnLoose() {
			_loose_panel.SetActive(true);
		}

		public void ResetPanels() {
			_win_panel.SetActive(false);
			_loose_panel.SetActive(false);
		}

		private void Awake() {
			if (_win_panel == null ||
				_loose_panel == null) {
				Debug.LogError("Not all fields were assigned!");
			}
		}

	}

}
