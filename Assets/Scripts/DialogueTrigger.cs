using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] GameObject _visualCue;
    [SerializeField] TextAsset _inkJSON;
    bool _playerInRange;

    private void Awake() 
    {
        _visualCue.SetActive(false);
    }

    private void Update() {
        if (_playerInRange) 
        {
            _visualCue.SetActive(true);
            // if (InputManager.Instance._interacting && !DialogueManager.Instance._dialogueIsPlaying)
            // {
            //     InputManager.Instance.RegisterInteraction();
            //     DialogueManager.Instance.EnterDialogueMode(_inkJSON);
            // }
        }
        else 
        {
            _visualCue.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            _playerInRange = false;
        }
    }
}
