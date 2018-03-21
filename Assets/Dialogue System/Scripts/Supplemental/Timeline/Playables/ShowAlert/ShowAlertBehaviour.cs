#if UNITY_2017_1_OR_NEWER && !(UNITY_2017_3 && UNITY_WSA)
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace PixelCrushers.DialogueSystem
{

    [Serializable]
    public class ShowAlertBehaviour : PlayableBehaviour
    {

        [Tooltip("Show this message using the Dialogue System's alert panel.")]
        public string message;

        [Tooltip("Show alert for duration based on text length, not duration of playable clip.")]
        public bool useTextLengthForDuration;

    }
}
#endif
