using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor.Timeline;

namespace Unity.AnimeToolbox.Editor {
[CustomTimelineEditor(typeof(SnapPoint))]
public class SnapPointEditor : MarkerEditor {
    static GUIContent s_Temp = new GUIContent();

    public override MarkerDrawOptions GetMarkerOptions(IMarker marker) {
        SnapPoint snapPoint = (SnapPoint)marker;

        MarkerDrawOptions options = base.GetMarkerOptions(marker);
        options.tooltip = snapPoint.tooltip;
        return options;
    }


    public override void OnCreate(IMarker marker, IMarker clonedFrom) {
        SnapPoint  snapPoint  = (SnapPoint)marker;
        TrackAsset track      = marker.parent;
        SnapPoint  lastMarker = track.GetMarkers().OfType<SnapPoint>().LastOrDefault(x => x != snapPoint);
        if (lastMarker != null) {
            float h, s, v;
            Color.RGBToHSV(lastMarker.snapLineColor, out h, out s, out v);
            h = (h + 0.2f) % 1.0f;
            Color c = Color.HSVToRGB(h, s, v);
            snapPoint.snapLineColor.r = c.r;
            snapPoint.snapLineColor.g = c.g;
            snapPoint.snapLineColor.b = c.b;
        }
    }

    public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region) {
        SnapPoint snapPoint = (SnapPoint)marker;

        DrawSnapLine(snapPoint, uiState, region);
        DrawLabel(snapPoint, uiState, region);
    }


    static void DrawLabel(SnapPoint point, MarkerUIStates uiState, MarkerOverlayRegion region) {
        if (string.IsNullOrEmpty(point.label))
            return;

        float colorScale = uiState.HasFlag(MarkerUIStates.Selected) ? 1.0f : 0.85f;

        GUIStyle textStyle = EditorStyles.whiteMiniLabel;
        s_Temp.text = point.label;

        Rect labelRect = region.markerRegion;
        labelRect.x     += labelRect.width;
        labelRect.width =  textStyle.CalcSize(s_Temp).x + 5;
        Rect shadowRect =
            Rect.MinMaxRect(labelRect.xMin + 1, labelRect.yMin + 1, labelRect.xMax + 1, labelRect.yMax + 1);

        Color oldColor = GUI.color;
        GUI.color = Color.black;
        GUI.Label(shadowRect, s_Temp, textStyle);
        GUI.color = Color.white * colorScale;
        GUI.Label(labelRect, s_Temp, textStyle);
        GUI.color = oldColor;
    }

    static void DrawSnapLine(SnapPoint snapPoint, MarkerUIStates uiState, MarkerOverlayRegion region) {
        if (snapPoint.snapLine == SnapPoint.SnapLine.None)
            return;


        bool collapsed = uiState.HasFlag(MarkerUIStates.Collapsed);
        if (collapsed && snapPoint.snapLine == SnapPoint.SnapLine.NotCollapsed)
            return;

        float offset = collapsed ? 7 : 15;
        Color color  = snapPoint.snapLineColor;
        if (uiState.HasFlag(MarkerUIStates.Selected)) {
            color = color * 1.5f;
        }

        Rect r = new Rect(region.markerRegion.center.x - 0.5f,
            region.markerRegion.min.y + offset,
            1.0f,
            region.timelineRegion.height
        );

        Color oldColor = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(r, Texture2D.whiteTexture, ScaleMode.StretchToFill);

        if (snapPoint.drawHighlight) {
            double previousTime = region.startTime;
            foreach (var m in snapPoint.parent.GetMarkers()) {
                if (m.time < snapPoint.time && m is SnapPoint)
                    previousTime = Math.Max(m.time, previousTime);
            }

            if (previousTime != snapPoint.time) {
                Rect highlightRect = region.markerRegion;
                highlightRect.xMin   = ToPixel(region, previousTime);
                highlightRect.xMax   = region.markerRegion.center.x;
                highlightRect.height = 2;
                GUI.DrawTexture(highlightRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true);
            }
        }

        GUI.color = oldColor;
    }

    static float ToPixel(MarkerOverlayRegion region, double time) {
        double p = (time - region.startTime) / (region.endTime - region.startTime);
        return region.timelineRegion.x + region.timelineRegion.width * (float)p;
    }
}
} //end namespace