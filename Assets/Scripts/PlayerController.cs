using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public const int playerId = 0;
    public static bool inPlay = true;
    public static int _money = 0;

    Player _player; // The Rewired Player
    DigSystem _digSystem;
    Rigidbody2D _rB;
    Interactable _nearbyObject;

    [SerializeField] LayerMask _groundLayer;
    Vector2 groundCheckPoint;
    Vector2 groundCheckSize;

    [SerializeField] float _walkSpeed = 6f;
    [SerializeField] float _runSpeed = 10f;
    [SerializeField] float _shortJumpForce = 100f;
    [SerializeField] float _tallJumpForce = 300f;
    float _currentSpeed;
    bool _isGrounded; 

    void Awake() 
    {
        _player = ReInput.players.GetPlayer(playerId);

        _player.AddInputEventDelegate(Jump, UpdateLoopType.FixedUpdate, "Jump");
        _player.AddInputEventDelegate(Dig, UpdateLoopType.Update, "Dig");
        //_player.AddInputEventDelegate(SuperDig, UpdateLoopType.Update, "SuperDig");
        _player.AddInputEventDelegate(Interact, UpdateLoopType.Update, "Interact");

        _rB = GetComponent<Rigidbody2D>();
        if (TryGetComponent(out DigSystem digSystem)) _digSystem = digSystem;
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        if (!inPlay) return;

        _currentSpeed = _player.GetButton("Run") ? _runSpeed : _walkSpeed;
        _rB.velocity = new Vector2(_player.GetAxis("MoveHorizontal") * _currentSpeed, _rB.velocity.y);
    }

    private void Update() 
    {
        if (!inPlay) return;
        
        CheckPlayerState();
    }
    private void CheckPlayerState()
    {
        //If box collides with an object on the ground layer, isGrounded = true
        groundCheckPoint = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f);
        groundCheckSize = new Vector2(0.95f, 0.2f);
        _isGrounded = Physics2D.OverlapBox(groundCheckPoint, groundCheckSize, 0f, _groundLayer);
    }

    private void Jump(InputActionEventData data)
    {
        if (!inPlay) return;
        else if (!_isGrounded) return;

        if (data.GetButtonDown()) _rB.AddForce(new Vector2(0f, _tallJumpForce), ForceMode2D.Impulse);
    }
    private void Dig(InputActionEventData data)
    {
        if (!inPlay) return;
        else if (_digSystem == null) return;
            
        if (data.GetButtonDown()) _digSystem.Dig();
    }
    private void SuperDig(InputActionEventData data)
    {
        if (!inPlay) return;
        else if (_digSystem == null) return;

        if (data.GetButtonDown()) _digSystem.Dig();
    }
    private void Interact(InputActionEventData data)
    {
        if (!inPlay) return;
        else if (DialogueManager.Instance._dialogueIsPlaying) return;
        else if (_nearbyObject == null) return;
            
        if (data.GetButtonDown()) _nearbyObject.Activate();
    }

    void OnDestroy() {
        // Unsubscribe from events when object is destroyed
        _player.RemoveInputEventDelegate(Jump);
        _player.RemoveInputEventDelegate(Dig);
        _player.RemoveInputEventDelegate(SuperDig);
        _player.RemoveInputEventDelegate(Interact);
    }

    //--Trigger Collision Methods--
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            _nearbyObject = interactable;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.TryGetComponent(out Interactable interactable) && interactable == _nearbyObject)
        {
            _nearbyObject = null;
        }
    }

    //--Data Persistence Methods--
    public void SaveData(GameData data)
    {
        data.money = _money;
    }
    public void LoadData(GameData data)
    {
        _money = data.money;
    }
}
