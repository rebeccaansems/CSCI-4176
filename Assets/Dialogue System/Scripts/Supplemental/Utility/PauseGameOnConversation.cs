using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Pauses the game during conversations by setting Time.timeScale to 0.
    /// Conversations can still run because this component also sets
    /// DialogueTime.Mode to Realtime.
    /// </summary>
    [AddComponentMenu("Dialogue System/Miscellaneous/Pause Game On Conversation")]
    public class PauseGameOnConversation : MonoBehaviour
    {

        private float preConversationTimeScale = 1;

        public void OnConversationStart(Transform actor)
        {
            if (!enabled) return;
            preConversationTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnConversationEnd(Transform actor)
        {
            if (!enabled) return;
            Time.timeScale = preConversationTimeScale;
        }

    }

}