#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Optional component to provide references to selector UI elements.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/unity_u_i_selector_display.html")]
#endif
    [AddComponentMenu("Dialogue System/UI/Unity UI/Selection/Unity UI Selector Elements")]
    public class UnityUISelectorElements : MonoBehaviour
    {

        /// <summary>
        /// The main graphic (optional). Assign this if you have created an entire 
        /// panel for the selector.
        /// </summary>
        public UnityEngine.UI.Graphic mainGraphic = null;

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

        public UnityUISelectorDisplay.AnimationTransitions animationTransitions = new UnityUISelectorDisplay.AnimationTransitions();

        public static UnityUISelectorElements instance = null;

        private void Awake()
        {
            instance = this;
        }
    }

}
#endif