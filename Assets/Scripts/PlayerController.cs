using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public static bool inPlay = true;

    Player _player; // The Rewired Player
    DigSystem _digSystem;
    Rigidbody2D _rB;

    [SerializeField] int playerId = 0;
    [SerializeField] float _walkSpeed = 6f;
    [SerializeField] float _runSpeed = 10f;
    [SerializeField] float _jumpForce = 100f;
    float _currentSpeed;

    void Awake() 
    {
        _player = ReInput.players.GetPlayer(playerId);
        _rB = GetComponent<Rigidbody2D>();
        _digSystem = GetComponent<DigSystem>();
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        _currentSpeed = _player.GetButton("Run") ? _runSpeed : _walkSpeed;
        _rB.velocity = new Vector2(_player.GetAxis("MoveHorizontal") * _currentSpeed, _rB.velocity.y);
    }

    private void Update() 
    {
        if (_player.GetButtonDown("Jump"))
        {
            _rB.AddForce(new Vector2(0f, _jumpForce * Time.deltaTime), ForceMode2D.Impulse);
        }
        if (_player.GetButtonDown("Dig"))
        {
            _digSystem.Dig();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if (other.TryGetComponent(out Interactable interactable))
        // {

        // }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        
    }
}
