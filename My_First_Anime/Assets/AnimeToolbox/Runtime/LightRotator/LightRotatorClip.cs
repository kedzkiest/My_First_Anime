using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Unity.AnimeToolbox {

[Serializable]
public class LightRotatorClip : PlayableAsset, ITimelineClipAsset
{
    public LightRotatorBehaviour template = new LightRotatorBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LightRotatorBehaviour>.Create (graph, template);
        return playable;
    }
}

} //end namespace
