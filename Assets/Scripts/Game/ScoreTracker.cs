using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cubic.Game {

	public class ScoreTracker : MonoBehaviour, IScoreIncrement {

		[SerializeField] private Text _score_text = null;

		private int _score;

		public void ResetScore() {
			_score = 0;
			_score_text.text = _score.ToString();
		}

		public void IncrementScore() {
			_score++;
			_score_text.text = _score.ToString();
		}

		private void Awake() {
			if (_score_text == null) {
				Debug.LogError("Not all fields were assigned!");
			}
		}

	}

}
