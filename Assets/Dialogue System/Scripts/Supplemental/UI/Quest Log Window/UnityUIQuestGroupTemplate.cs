#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem {

    /// <summary>
    /// This component hooks up the elements of a Unity UI quest group template.
    /// Add it to your quest group template and assign the properties.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/quest_log_window.html#questLogWindowUnityUI")]
#endif
    [AddComponentMenu("Dialogue System/UI/Unity UI/Quest/Unity UI Quest Group Template")]
	public class UnityUIQuestGroupTemplate : MonoBehaviour	{

		[Header("Quest Group Heading")]
		[Tooltip("The quest group name")]
		public UnityEngine.UI.Text heading;

		public bool ArePropertiesAssigned {
			get {
				return (heading != null);
			}
		}

		public void Initialize() {}

	}

}
#endif