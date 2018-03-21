using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem
{

    [AddComponentMenu("Dialogue System/Miscellaneous/Quest Log Window Hotkey")]
    public class QuestLogWindowHotkey : MonoBehaviour
    {

        [Tooltip("Toggle the quest log window when this key is pressed.")]
        public KeyCode key = KeyCode.J;

        [Tooltip("Toggle the quest log window when this input button is presed.")]
        public string buttonName = string.Empty;

        [Tooltip("Use this quest log window. If unassigned, will automatically find quest log window in scene.")]
        public QuestLogWindow questLogWindow;

        void Awake()
        {
            if (questLogWindow == null) questLogWindow = FindObjectOfType<QuestLogWindow>();
            if (questLogWindow == null) enabled = false;
        }

        void Update()
        {
            if (DialogueManager.IsDialogueSystemInputDisabled()) return;
            if (Input.GetKeyDown(key) || (!string.IsNullOrEmpty(buttonName) && DialogueManager.GetInputButtonDown(buttonName)))
            {
                if (questLogWindow.IsOpen) questLogWindow.Close(); else questLogWindow.Open();
            }
        }

    }

}
