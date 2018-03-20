#if UNITY_2017_1_OR_NEWER && !(UNITY_2017_3 && UNITY_WSA)
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{

    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(BarkClip))]
    [TrackBindingType(typeof(GameObject))]
    public class BarkTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<BarkMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif