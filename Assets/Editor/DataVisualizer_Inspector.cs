
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


        if (visualizer.useBoundingBox)
        {
            visualizer.minBoundingBox = EditorGUILayout.Vector3Field("Min Bounding Box", visualizer.minBoundingBox);
            visualizer.maxBoundingBox = EditorGUILayout.Vector3Field("Max Bounding Box", visualizer.maxBoundingBox);
        }

        visualizer.InvokeDrawLines();
    }
}
