#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// Enables a scrollbar only if the content is larger than the container. This component
	/// only shows or hides the scrollbar when the component is enabled or when it receives
	/// the OnContentChanged event.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Unity UI Scrollbar Enabler")]
	public class UnityUIScrollbarEnabler : MonoBehaviour {

		public RectTransform container = null;
		public RectTransform content = null;
		public GameObject scrollbar = null;

		private bool started = false;
		private bool isChecking = false;

		private void Start() {
			started = true;
			CheckScrollbar();
		}

		public void OnEnable() {
			if (started) CheckScrollbar();
		}

		public void OnDisable() {
			isChecking = false;
		}

		public void CheckScrollbar() {
			if (isChecking || container == null || content == null || scrollbar == null) return;
			StartCoroutine(CheckScrollbarAfterUIUpdate());
		}

		private IEnumerator CheckScrollbarAfterUIUpdate() {
			isChecking = true;
			yield return null;
			scrollbar.SetActive(content.rect.height > container.rect.height);
			isChecking = false;
		}

	}

}
#endif