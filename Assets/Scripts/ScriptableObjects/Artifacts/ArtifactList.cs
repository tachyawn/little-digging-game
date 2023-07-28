using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New ArtifactList", fileName = "ArtifactList")]
public class ArtifactList : ScriptableObject//, IDataPersistence
{
    //Change to array when all artifacts have been designed
    public List<Artifacts> _artifacts;
    public List<bool> _foundArtifacts;
}
