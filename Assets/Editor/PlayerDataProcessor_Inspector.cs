using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerDataProcessor))]
public class PlayerDataProcessor_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerDataProcessor pdp = (PlayerDataProcessor) target;
        
        pdp.visualizationMode = EditorGUILayout.Popup("Time constraint", pdp.visualizationMode,
            new[] { "By Frames", "By Timestamp" });
        
        switch (pdp.visualizationMode)
        {
            case 0:
                GUILayout.BeginHorizontal();
                pdp.lowerLimitFrame = EditorGUILayout.IntField("Lower Limit", pdp.lowerLimitFrame);
                if (GUILayout.Button("-1")) pdp.lowerLimitFrame = Mathf.Max(pdp.lowerLimitFrame - 1, 0);
                if (GUILayout.Button("+1")) pdp.lowerLimitFrame = Mathf.Max(pdp.lowerLimitFrame + 1, 0);
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                pdp.upperLimitFrame = EditorGUILayout.IntField("Upper Limit", pdp.upperLimitFrame);
                if (GUILayout.Button("-1")) pdp.upperLimitFrame = Mathf.Max(pdp.upperLimitFrame - 1, 0);
                if (GUILayout.Button("+1")) pdp.upperLimitFrame = Mathf.Max(pdp.upperLimitFrame + 1, 0);
                GUILayout.EndHorizontal();
                break;
            case 1:
                pdp.lowerLimitTimestamp = EditorGUILayout.IntField("Lower Limit", pdp.lowerLimitTimestamp);
                pdp.upperLimitTimestamp = EditorGUILayout.IntField("Upper Limit", pdp.upperLimitTimestamp);
                break;
        }
        
        DataVisualizer.Instance.InvokeDrawLines();
    }
}
