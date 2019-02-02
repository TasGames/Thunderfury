using System.Collections;
using System.Collections.Generic;
using InControl;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour 
{
	protected Rigidbody playerRb;

	[Title("Camera")]
	[SerializeField] protected Camera cam;

	[Title("Movement")]
    [SerializeField] protected float ForwardSpeed;
    [SerializeField] protected float BackwardSpeed;  
    [SerializeField] protected float StrafeSpeed;   
    [SerializeField] protected float RunMultiplier;
    [SerializeField] protected float JumpForce;
	[SerializeField] protected KeyCode RunKey;
	public float CurrentTargetSpeed = 8f;

	void Start()
	{
		playerRb = GetComponent<Rigidbody>();
		Cursor.visible = false;
	}
	
	void FixedUpdate()
	{

	}

    void UpdateDesiredTargetSpeed(Vector2 input)
    {
        if (input == Vector2.zero)
			return;
		if (input.x > 0 || input.x < 0)
			CurrentTargetSpeed = StrafeSpeed;
		if (input.y < 0)
			CurrentTargetSpeed = BackwardSpeed;
		if (input.y > 0)
			CurrentTargetSpeed = ForwardSpeed;
	}

	void Movement()
	{
		Vector2 input = GetInput();

    	if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon))
        {
        // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;

            desiredMove.x = desiredMove.x * CurrentTargetSpeed;
            desiredMove.z = desiredMove.z * CurrentTargetSpeed;
            desiredMove.y = desiredMove.y * CurrentTargetSpeed;
            if (playerRb.velocity.sqrMagnitude < (CurrentTargetSpeed * CurrentTargetSpeed))
            {
                playerRb.AddForce(desiredMove, ForceMode.Impulse);
            }
        }
	}

	/*protected Rigidbody playerRb;
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
    }*/

}
