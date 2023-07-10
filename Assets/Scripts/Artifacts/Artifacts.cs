using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifact", menuName = "New Artifact")]
public class Artifacts : ScriptableObject
{
    public string _name;
    public Sprite _sprite;
    public string _description;
    public int _value;
}
