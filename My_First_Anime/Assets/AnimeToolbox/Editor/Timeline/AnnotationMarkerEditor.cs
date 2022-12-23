using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditor.Timeline;

namespace Unity.AnimeToolbox.Editor {
[CustomTimelineEditor(typeof(Annotation))]
public class AnnotationMarkerEditor : MarkerEditor {
    public override MarkerDrawOptions GetMarkerOptions(IMarker marker) {
        Annotation annotation = (Annotation)marker;
        MarkerDrawOptions options    = base.GetMarkerOptions(marker);

        options.tooltip = annotation.details;

        return options;
    }

    public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region) {
        Annotation annotation = (Annotation)marker;
        if (string.IsNullOrEmpty(annotation.summary)) 
            return;
        
        GUIStyle style = EditorStyles.largeLabel;
        if (uiState.HasFlag(MarkerUIStates.Collapsed))
            style = EditorStyles.miniLabel;
        else if (uiState.HasFlag(MarkerUIStates.Selected))
            style = EditorStyles.whiteLargeLabel;

        Vector2 size = style.CalcSize(new GUIContent(annotation.summary));
        Rect rect = new Rect(
            region.markerRegion.xMax,
            region.markerRegion.yMin - style.padding.top,
            size.x,
            size.y
        );
        GUI.Label(rect, annotation.summary, style);
    }
}
} //end namespace