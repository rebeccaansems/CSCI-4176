#if UNITY_2017_1_OR_NEWER && !(UNITY_2017_3 && UNITY_WSA)
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace PixelCrushers.DialogueSystem
{

    [Serializable]
    public class StartConversationBehaviour : PlayableBehaviour
    {

        [Tooltip("(Optional) The other participant.")]
        public Transform conversant;

        [Tooltip("The conversation to start.")]
        [ConversationPopup]
        public string conversation;

        [Tooltip("Jump to a specific dialogue entry instead of starting from the conversation's START node.")]
        public bool jumpToSpecificEntry;

        [Tooltip("Dialogue entry to jump to.")]
        public int entryID;

    }
}
#endif
