using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour, Interactable
{
    public Artifacts _artifact;
    [SerializeField] GameObject _artifactObj;
    public bool _artifactFound = false;

    private void Awake() 
    {
        _artifactObj.GetComponent<SpriteRenderer>().sprite = _artifact._sprite;
    }

    public void UpdatePedestal(bool found)
    {
        _artifactFound = found;

        if (_artifactFound) _artifactObj.SetActive(true);
        else _artifactObj.SetActive(false);
    }

    public void Activate()
    {
        DialogueManager.Instance.EnterDialogueMode(_artifact._descriptionInkJSON);
    }
}
