using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New DiggingTool", fileName = "DiggingTool")]
public class DiggingTool : ScriptableObject
{
    public string _toolName;
    public int _toolStrength;
}
