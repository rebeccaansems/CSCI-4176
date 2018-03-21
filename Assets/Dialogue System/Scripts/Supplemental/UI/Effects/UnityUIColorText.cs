#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This script provides methods to change a Text element's color.
	/// You can tie it to hover events on buttons if you want the button's
	/// text color to change when hovered.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Unity UI Color Text")]
	public class UnityUIColorText : MonoBehaviour {

		public Color color;

		public UnityEngine.UI.Text text;

		private Color originalColor;

		private void Awake() {
			if (text == null) text = GetComponentInChildren<UnityEngine.UI.Text>();
			if (text != null) originalColor = text.color;
		}

		public void ApplyColor() {
			if (text != null) {
				originalColor = text.color;
				text.color = color;
			}
		}

		public void UndoColor() {
			if (text != null) text.color = originalColor;
		}

	}

}
#endif