using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileGenerator))]
public class TileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileGenerator myScript = (TileGenerator)target;
        if(GUILayout.Button("Store Coin"))
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

        EditorGUILayout.HelpBox($"The current number of patterns is: {myScript._tileList._patterns.Count}", MessageType.Info);
    }
}