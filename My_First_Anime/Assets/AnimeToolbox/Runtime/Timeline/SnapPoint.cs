using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace Unity.AnimeToolbox {
[System.Serializable]
public class SnapPoint : Marker {
    [Flags]
    public enum SnapLine {
        None         = 0,
        NotCollapsed = 1,
        Always       = 255,
    }


    public Color    snapLineColor = new Color(0.75f, 0.4f, 0.1f, 0.2f);
    public SnapLine snapLine      = SnapLine.NotCollapsed;
    public bool     drawHighlight = true;

    public string label = String.Empty;

    [TextArea] public string tooltip = String.Empty;
}
} //end namespace