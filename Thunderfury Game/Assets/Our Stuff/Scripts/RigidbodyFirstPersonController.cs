using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (CapsuleCollider))]
public class RigidbodyFirstPersonController : MonoBehaviour
{
	[Serializable] public class MovementSettings
	{
		public float ForwardSpeed = 8.0f;
		public float BackwardSpeed = 4.0f;
		public float StrafeSpeed = 4.0f;
		public float JumpForce = 30f;
		public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
		[HideInInspector] public float CurrentTargetSpeed = 8f;

		public void UpdateDesiredTargetSpeed(Vector2 input)
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
	}

	[Serializable] public class AdvancedSettings
	{
		public float groundCheckDistance = 0.01f;
		public float stickToGroundHelperDistance = 0.5f;
		public float slowDownRate = 20f; 
		public bool airControl;
		[Tooltip("set it to 0.1 or more if you get stuck in wall")]
		public float shellOffset;
	}

	[Serializable] public class MouseLook
	{
		public float xSensitivity = 2f;
		public float ySensitivity = 2f;
		public bool clampVerticalRotation = true;
		public float minimumX = -90F;
		public float maximumX = 90F;
		public bool smooth;
		public float smoothTime = 5f;
		public bool lockCursor = true;

		protected Quaternion characterTargetRot;
		protected Quaternion cameraTargetRot;
		protected bool isCursorLocked = true;

		public void Init(Transform character, Transform camera)
		{
			characterTargetRot = character.localRotation;
			cameraTargetRot = camera.localRotation;
		}

		public void LookRotation(Transform character, Transform camera)
		{
			float yRot = Input.GetAxis("Mouse X") * xSensitivity;
			float xRot = Input.GetAxis("Mouse Y") * ySensitivity;

			characterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
			cameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

			if(clampVerticalRotation == true)
				cameraTargetRot = ClampRotationAroundXAxis (cameraTargetRot);

			if(smooth == true)
			{
				character.localRotation = Quaternion.Slerp (character.localRotation, characterTargetRot,
					smoothTime * Time.deltaTime);
				camera.localRotation = Quaternion.Slerp (camera.localRotation, cameraTargetRot,
					smoothTime * Time.deltaTime);
			}
			else
			{
				character.localRotation = characterTargetRot;
				camera.localRotation = cameraTargetRot;
			}

			UpdateCursorLock();
		}

		public void SetCursorLock(bool value)
		{
			lockCursor = value;
			
			if(lockCursor == false)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		public void UpdateCursorLock()
		{
			if (lockCursor == true)
				InternalLockUpdate();
		}

		void InternalLockUpdate()
		{
			if(Input.GetKeyUp(KeyCode.Escape))
				isCursorLocked = false;
			else if(Input.GetMouseButtonUp(0))
				isCursorLocked = true;

			if (isCursorLocked == true)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else if (isCursorLocked == false)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		Quaternion ClampRotationAroundXAxis(Quaternion q)
		{
			q.x /= q.w;
			q.y /= q.w;
			q.z /= q.w;
			q.w = 1.0f;

			float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

			angleX = Mathf.Clamp (angleX, minimumX, maximumX);

			q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

			return q;
		}
	}

	[SerializeField] protected Camera cam;
	[SerializeField] protected MovementSettings movementSettings = new MovementSettings();
	[SerializeField] protected MouseLook mouseLook = new MouseLook();
	[SerializeField] protected AdvancedSettings advancedSettings = new AdvancedSettings();

	protected Rigidbody playerRb;
	protected CapsuleCollider capsuleCol;
	protected float yRotation;
	protected Vector3 groundContactNormal;
	protected bool hasJumped;
	protected bool previouslyGrounded;
	protected bool isJumping; 
	protected bool isGrounded;

	void Start()
	{
		playerRb = GetComponent<Rigidbody>();
		capsuleCol = GetComponent<CapsuleCollider>();
		mouseLook.Init (transform, cam.transform);
	}

	void Update()
	{
		RotateView();

		if (Input.GetButtonDown("Jump") && hasJumped == false)
			hasJumped = true;
	}

	void FixedUpdate()
	{
		GroundCheck();
		Vector2 input = GetInput();

		if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || isGrounded))
		{
			Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
			desiredMove = Vector3.ProjectOnPlane(desiredMove, groundContactNormal).normalized;

			desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
			desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
			desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;

			if (playerRb.velocity.sqrMagnitude < (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
				playerRb.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
		}

		if (isGrounded == true)
		{
			playerRb.drag = 5f;

			if (hasJumped == true)
			{
				playerRb.drag = 0f;
				playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
				playerRb.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
				isJumping = true;
			}

			if (isJumping == false && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && playerRb.velocity.magnitude < 1f)
				playerRb.Sleep();
		}
		else
		{
			playerRb.drag = 0f;

			if (previouslyGrounded == true && isJumping == false)
				StickToGroundHelper();
		}

		hasJumped = false;
	}

	float SlopeMultiplier()
	{
		float angle = Vector3.Angle(groundContactNormal, Vector3.up);

		return movementSettings.SlopeCurveModifier.Evaluate(angle);
	}

	void StickToGroundHelper()
	{
		RaycastHit hitInfo;
		if (Physics.SphereCast(transform.position, capsuleCol.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo, ((capsuleCol.height/2f) - capsuleCol.radius) + advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
		{
			if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
				playerRb.velocity = Vector3.ProjectOnPlane(playerRb.velocity, hitInfo.normal);
		}
	}

	Vector2 GetInput()
	{
		Vector2 input = new Vector2
			{
				x = Input.GetAxis("Horizontal"),
				y = Input.GetAxis("Vertical")
			};

		movementSettings.UpdateDesiredTargetSpeed(input);

		return input;
	}

	void RotateView()
	{
		if (Mathf.Abs(Time.timeScale) < float.Epsilon)
			return;

		float oldYRotation = transform.eulerAngles.y;

		mouseLook.LookRotation (transform, cam.transform);

		if (isGrounded == true || advancedSettings.airControl)
		{
			Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
			playerRb.velocity = velRotation*playerRb.velocity;
		}
	}

	void GroundCheck()
	{
		previouslyGrounded = isGrounded;
		RaycastHit hitInfo;

		if (Physics.SphereCast(transform.position, capsuleCol.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo, ((capsuleCol.height/2f) - capsuleCol.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
		{
			isGrounded = true;
			groundContactNormal = hitInfo.normal;
		}
		else
		{
			isGrounded = false;
			groundContactNormal = Vector3.up;
		}

		if (previouslyGrounded == false && isGrounded && isJumping)
			isJumping = false;
	}
}
