#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This is a variation of UnityUIDialogueUI that uses the speaker's bark UI for subtitles.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Dialogue/Bark Subtitle Dialogue UI")]
	public class UnityUIBarkSubtitleDialogueUI : UnityUIDialogueUI {
		
		public override void ShowSubtitle(Subtitle subtitle) {
			var barkUI = subtitle.speakerInfo.transform.GetComponentInChildren<UnityUIBarkUI>();
			if (barkUI == null) {
				Debug.Log("Null bark UI: " + subtitle.formattedText.text);
			} else {
				barkUI.Bark(subtitle);
			}
			HideResponses();
		}

	}

}
#endif