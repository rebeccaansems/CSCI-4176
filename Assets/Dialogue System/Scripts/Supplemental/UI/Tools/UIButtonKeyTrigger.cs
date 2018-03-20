#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This script adds a key or button trigger to a UIButton.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/unity_u_i_dialogue_u_i.html#unityUIDialogueUINavigation")]
#endif
    [AddComponentMenu("Dialogue System/UI/Unity UI/Effects/UI Button Key Trigger")]
    public class UIButtonKeyTrigger : MonoBehaviour
    {

        public KeyCode key = KeyCode.None;
        public string buttonName = string.Empty;

        private UnityEngine.UI.Button button = null;

        void Awake()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            if (button == null) enabled = false;
        }

        void Update()
        {
            if (DialogueManager.IsDialogueSystemInputDisabled()) return;
            if (Input.GetKeyDown(key) || (!string.IsNullOrEmpty(buttonName) && DialogueManager.GetInputButtonDown(buttonName)))
            {
                var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
            }
        }

    }

}
#endif