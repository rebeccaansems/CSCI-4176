#if UNITY_2017_1_OR_NEWER && !(UNITY_2017_3 && UNITY_WSA)
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    public class ConversationMixerBehaviour : PlayableBehaviour
    {

        private HashSet<int> played = new HashSet<int>();

        // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            GameObject trackBinding = playerData as GameObject;

            if (!trackBinding) return;

            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0.001f && !played.Contains(i))
                {
                    played.Add(i);
                    ScriptPlayable<StartConversationBehaviour> inputPlayable = (ScriptPlayable<StartConversationBehaviour>)playable.GetInput(i);
                    StartConversationBehaviour input = inputPlayable.GetBehaviour();
                    if (Application.isPlaying)
                    {
                        if (input.entryID <= 0)
                        {
                            DialogueManager.StartConversation(input.conversation, trackBinding.transform, input.conversant);
                        }
                        else
                        {
                            DialogueManager.StartConversation(input.conversation, trackBinding.transform, input.conversant, input.entryID);
                        }
                    }
                    else
                    {
                        var message = OverrideActorName.GetActorName(trackBinding.transform) + " conversation: " + input.conversant;
                        PreviewUI.ShowMessage(message, 2, 0);
                    }
                }
                else if (inputWeight <= 0.001f && played.Contains(i))
                {
                    played.Remove(i);
                }
            }
        }

        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            played.Clear();
        }

        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            played.Clear();
        }

    }
}
#endif
