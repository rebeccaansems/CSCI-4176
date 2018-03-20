#if UNITY_2017_1_OR_NEWER && !(UNITY_2017_3 && UNITY_WSA)
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{

    [Serializable]
    public class SetQuestStateClip : PlayableAsset, ITimelineClipAsset
    {
        public SetQuestStateBehaviour template = new SetQuestStateBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SetQuestStateBehaviour>.Create(graph, template);
            //SetQuestStateBehaviour clone = playable.GetBehaviour();
            return playable;
        }
    }
}
#endif
