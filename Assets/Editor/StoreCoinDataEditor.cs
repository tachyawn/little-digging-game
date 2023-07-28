using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StoreCoinData))]
public class StoreCoinDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StoreCoinData myScript = (StoreCoinData)target;
        if(GUILayout.Button("Load Pattern Data"))
        {
            myScript.LoadPatternData(true);
        }
        EditorGUILayout.Space();
        if(GUILayout.Button("Store Pattern"))
        {
            myScript.StoreCoinPattern();
        }
        if(GUILayout.Button("Place Pattern"))
        {
            myScript.FillCoinPattern(myScript._patternIndex);
        }
        if(GUILayout.Button("Remove Pattern"))
        {
            myScript.RemoveCoinPattern(myScript._patternIndex);
        }
        EditorGUILayout.Space();
        if(GUILayout.Button("Clear all Patterns"))
        {
            myScript.ClearPatterns();
        }

        EditorGUILayout.HelpBox($"The current number of patterns is: {myScript._patternCount}", MessageType.Info);
    }
}