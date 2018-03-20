#if UNITY_2017_1_OR_NEWER && !(UNITY_2017_3 && UNITY_WSA)
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace PixelCrushers.DialogueSystem
{

    [Serializable]
    public class BarkBehaviour : PlayableBehaviour
    {

        [Tooltip("Get bark text from a conversation.")]
        public bool useConversation = true;

        [Tooltip("Get the bark text from this conversation.")]
        [ConversationPopup]
        public string conversation;

        [Tooltip("Bark this text.")]
        public string text;

        [Tooltip("(Optional) Barker is barking to this listener.")]
        public Transform listener;

        public string GetEditorBarkText()
        {
            return useConversation ? "(From " + conversation + ")" : text;
        }

    }
}
#endif
