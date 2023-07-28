using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class MuseumCurator : MonoBehaviour, Interactable
{
    ArtifactManager _artifactManager;
    Story story;
    [SerializeField] TextAsset _curatorInkJSON;

    // Start is called before the first frame update
    void Start()
    {
        _artifactManager = GameObject.FindObjectOfType<ArtifactManager>();
        //Update variables here
    }

    public void Activate()
    {
        DialogueManager.Instance.EnterDialogueMode(_curatorInkJSON);
    }
}
