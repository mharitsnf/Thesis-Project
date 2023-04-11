
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataVisualizer))]
public class DataVisualizer_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        DataVisualizer visualizer = (DataVisualizer) target;

        visualizer.visualizationMode = EditorGUILayout.Popup("Time constraint", visualizer.visualizationMode,
            new[] { "By Percentage", "By Frames", "By Timeframe" });

        switch (visualizer.visualizationMode)
        {
            case 0:
                visualizer.lowerLimitPercentage = (int)EditorGUILayout.Slider("Lower Limit",
                    visualizer.lowerLimitPercentage, 0, visualizer.upperLimitPercentage);
                visualizer.upperLimitPercentage = (int) EditorGUILayout.Slider("Upper Limit",
                    visualizer.upperLimitPercentage, visualizer.lowerLimitPercentage, 100);
                break;
            case 1:
                visualizer.lowerLimitFrame = EditorGUILayout.IntField("Lower Limit", visualizer.lowerLimitFrame);
                visualizer.upperLimitFrame = EditorGUILayout.IntField("Upper Limit", visualizer.upperLimitFrame);
                break;
            case 2:
                visualizer.lowerLimitTimestamp = EditorGUILayout.IntField("Lower Limit", visualizer.lowerLimitTimestamp);
                visualizer.upperLimitTimestamp = EditorGUILayout.IntField("Upper Limit", visualizer.upperLimitTimestamp);
                break;
        }

        if (visualizer.useBoundingBox)
        {
            visualizer.minBoundingBox = EditorGUILayout.Vector3Field("Min Bounding Box", visualizer.minBoundingBox);
            visualizer.maxBoundingBox = EditorGUILayout.Vector3Field("Max Bounding Box", visualizer.maxBoundingBox);
        }

        visualizer.InvokeDrawLines();
    }
}
