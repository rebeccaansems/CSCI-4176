using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This script replaces the normal continue button functionality with
    /// a two-stage process. If the typewriter effect is still playing, it
    /// simply stops the effect. Otherwise it sends OnContinue to the UI.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/unity_u_i_dialogue_u_i.html#unityUIDialogueUIContinueButtonFastForward")]
#endif
    [AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Unity UI Continue Button Fast Forward")]
    public class UnityUIContinueButtonFastForward : MonoBehaviour
    {

        [Tooltip("Dialogue UI that the continue button affects.")]
        public UnityUIDialogueUI dialogueUI;

        [Tooltip("Typewriter effect to fast forward if it's not done playing.")]
        public UnityUITypewriterEffect typewriterEffect;

        [Tooltip("Hide the continue button when continuing.")]
        public bool hideContinueButtonOnContinue = false;

        private UnityEngine.UI.Button continueButton;

        public virtual void Awake()
        {
            if (dialogueUI == null)
            {
                dialogueUI = Tools.GetComponentAnywhere<UnityUIDialogueUI>(gameObject);
            }
            if (typewriterEffect == null)
            {
                typewriterEffect = GetComponentInChildren<UnityUITypewriterEffect>();
            }
            continueButton = GetComponent<UnityEngine.UI.Button>();
        }

        public virtual void OnFastForward()
        {
            if ((typewriterEffect != null) && typewriterEffect.IsPlaying)
            {
                typewriterEffect.Stop();
            }
            else
            {
                if (hideContinueButtonOnContinue && continueButton != null) continueButton.gameObject.SetActive(false);
                if (dialogueUI != null) dialogueUI.OnContinue();
            }
        }

    }

}
