using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour 
{
	protected Rigidbody playerRb;

    protected bool disableInput;
    InputDevice inputDevice;

	[SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;

	void Start ()
	{
		playerRb = GetComponent<Rigidbody>();
        inputDevice = InputManager.ActiveDevice;
	}
	
	void FixedUpdate()
	{
		Move();
	}

	void Move()
	{
		float _xMove = Input.GetAxis("Horizontal");
		float _zMove = Input.GetAxis("Vertical");

		Vector3 _moveHor = transform.right * _xMove;
		Vector3 _moveVer = transform.forward * _zMove;

		Vector3 _velocity = (_moveHor + _moveVer).normalized * moveSpeed;

		playerRb.velocity = _velocity;
	}

}
