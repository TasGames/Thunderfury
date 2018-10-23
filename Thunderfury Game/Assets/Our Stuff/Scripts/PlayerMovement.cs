using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour 
{
	protected Rigidbody playerRb;
	[SerializeField] protected Camera cam;
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected float jumpPower;
    [SerializeField] protected float rotateSpeed;
	protected bool inverted = false;
	protected bool canJump = true;
	protected bool usingController = false;

	void Start()
	{
		playerRb = GetComponent<Rigidbody>();
		Cursor.visible = false;
	}
	
	void FixedUpdate()
	{
		if (usingController == false)
			KeyboardMouse();
	}

	void KeyboardMouse()
	{
		Move();
		Rotate();
		Jump();
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

	void Rotate()
	{
		float _yRot= Input.GetAxis("Mouse X");
		Vector3 _playerRotation = new Vector3(0f, _yRot, 0f) * rotateSpeed;
		Vector3 _rotation = _playerRotation;

		playerRb.MoveRotation(playerRb.rotation * Quaternion.Euler(_rotation));

		float _xRot = Input.GetAxis("Mouse Y");
		Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * rotateSpeed;

		if(inverted == false)
			_cameraRotation = _cameraRotation * -1;

		if(cam != null)
			cam.transform.Rotate(_cameraRotation);
	}

	void Jump()
	{
		if (Input.GetButton("Jump") && canJump == true)
        {
            playerRb.AddForce(0, jumpPower, 0);
            canJump = false;
        }	
	}

	void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Ground")
            canJump = true;
    }

}
