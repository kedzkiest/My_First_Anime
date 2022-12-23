using UnityEngine;
using UnityEngine.Timeline;

namespace Unity.AnimeToolbox {
public class Annotation : Marker {
    [TextArea] public string summary;

    [TextArea] public string details;
}
} //end namespace