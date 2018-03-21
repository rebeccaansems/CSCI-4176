#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Replaces the Selector/ProximitySelector's OnGUI method with a method
    /// that enables or disables new Unity UI controls.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/usable_unity_u_i.html")]
#endif
    [AddComponentMenu("Dialogue System/UI/Unity UI/Selection/Usable Unity UI")]
    public class UsableUnityUI : MonoBehaviour
    {

        /// <summary>
        /// The text for the name of the current selection.
        /// </summary>
        public UnityEngine.UI.Text nameText = null;

        /// <summary>
        /// The text for the use message (e.g., "Press spacebar to use").
        /// </summary>
        public UnityEngine.UI.Text useMessageText = null;

        public Color inRangeColor = Color.yellow;

        public Color outOfRangeColor = Color.gray;

        /// <summary>
        /// The graphic to show if the selection is in range.
        /// </summary>
        public UnityEngine.UI.Graphic reticleInRange = null;

        /// <summary>
        /// The graphic to show if the selection is out of range.
        /// </summary>
        public UnityEngine.UI.Graphic reticleOutOfRange = null;

        [Serializable]
        public class AnimationTransitions
        {
            public string showTrigger = "Show";
            public string hideTrigger = "Hide";
        }

        public AnimationTransitions animationTransitions = new AnimationTransitions();

        private Canvas canvas = null;

        private Animator animator = null;

        public void Awake()
        {
            canvas = GetComponent<Canvas>();
            animator = GetComponent<Animator>();
        }

        public void Start()
        {
            Usable usable = Tools.GetComponentAnywhere<Usable>(gameObject);
            if ((usable != null) && (nameText != null)) nameText.text = usable.GetName();
            if (canvas != null) canvas.enabled = false;
        }

        public void Show(string useMessage)
        {
            if (canvas != null) canvas.enabled = true;
            if (useMessageText != null) useMessageText.text = useMessage;
            if (CanTriggerAnimations() && !string.IsNullOrEmpty(animationTransitions.showTrigger))
            {
                animator.SetTrigger(animationTransitions.showTrigger);
            }
        }

        public void Hide()
        {
            if (CanTriggerAnimations() && !string.IsNullOrEmpty(animationTransitions.hideTrigger))
            {
                animator.SetTrigger(animationTransitions.hideTrigger);
            }
            else
            {
                if (canvas != null) canvas.enabled = false;
            }
        }

        public void OnBarkStart(Transform actor)
        {
            Hide();
        }

        public void OnConversationStart(Transform actor)
        {
            Hide();
        }

        public void UpdateDisplay(bool inRange)
        {
            Color color = inRange ? inRangeColor : outOfRangeColor;
            if (nameText != null) nameText.color = color;
            if (useMessageText != null) useMessageText.color = color;
            Tools.SetGameObjectActive(reticleInRange, inRange);
            Tools.SetGameObjectActive(reticleOutOfRange, !inRange);
        }

        private bool CanTriggerAnimations()
        {
            return (animator != null) && (animationTransitions != null);
        }

    }

}
#endif