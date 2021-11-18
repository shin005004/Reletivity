using UnityEngine;
using System.Collections;

public class PlayerMovement3D : MonoBehaviour
{
	//Consts 
	private const float SLOW_DOWN_RATE = 2f;
	private const float ACCEL_RATE = 20f;
	private const int INIT_FRAME_WAIT = 5;
	private const float DEGREE_TO_RADIAN_CONST = 57.2957795f;
	[HideInInspector] public float controllerBoost = 6000;
	private int inverted;

	//Affect our rotation speed
	public float rotSpeed;
	public Transform camTransform;

	// variables for making lorentz equation
	public int speedOfLightTarget;
	private float speedOfLightStep;
	public float mouseSensitivity;
	int frames;
	public float viwMax = 3;

	GameState state;

	void Start()
	{
		// grab Game State, we need it for many actions
		state = GetComponent<GameState>();

		// Lock and hide cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		//Set the speed of light to the starting speed of light in GameState
		speedOfLightTarget = (int)state.SpeedOfLight;
		inverted = -1;

		viwMax = Mathf.Min(viwMax, (float)GameObject.FindWithTag("Player").GetComponent<GameState>().MaxSpeed);
		frames = 0;
	}

	void LateUpdate()
	{
		if (true)
		{
			float viewRotX = 0;

			// If we're not paused, update speed and rotation using player input.
			if (!state.MenuFrozen)
			{
				state.deltaRotation = Vector3.zero;

				// PLAYER MOVEMENT LOGIC
				// Control with wasd and use fps camera

				Vector3 playerVelocityVector = state.PlayerVelocityVector;

				// Get our angle between the velocity and the X axis.
				// Make a Quaternion from the angle, one to rotate, one to rotate back. 

				float rotationAroundX = DEGREE_TO_RADIAN_CONST * Mathf.Acos(Vector3.Dot(playerVelocityVector, Vector3.right) / playerVelocityVector.magnitude);
				Quaternion rotateX = Quaternion.AngleAxis(rotationAroundX, Vector3.Cross(playerVelocityVector, Vector3.right).normalized);
				Quaternion unRotateX = Quaternion.AngleAxis(rotationAroundX, Vector3.Cross(Vector3.right, playerVelocityVector).normalized);

				if (playerVelocityVector.sqrMagnitude == 0)
				{
					// Shoot Errors
					rotationAroundX = 0;
					rotateX = Quaternion.identity;
					unRotateX = Quaternion.identity;
				}

				// Check the angle of the camera (need to add force)
				Vector3 addedVelocity = Vector3.zero;
				float cameraRotationAngle = -DEGREE_TO_RADIAN_CONST * Mathf.Acos(Vector3.Dot(camTransform.forward, Vector3.forward));
				Quaternion cameraRotation = Quaternion.AngleAxis(cameraRotationAngle, Vector3.Cross(camTransform.forward, Vector3.forward).normalized);

				// (left/right, 0, up/down) * (rotate to camera angle)
				addedVelocity += new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")) * ACCEL_RATE * (float)Time.deltaTime;
				if (addedVelocity.magnitude != 0.0f)
					state.keyHit = true;
				addedVelocity = cameraRotation * addedVelocity;

				// AUTO SLOW DOWN
				if (addedVelocity.x == 0)
					addedVelocity += new Vector3(-1 * SLOW_DOWN_RATE * playerVelocityVector.x * (float)Time.deltaTime, 0, 0);
				if (addedVelocity.z == 0)
					addedVelocity += new Vector3(0, 0, -1 * SLOW_DOWN_RATE * playerVelocityVector.z * (float)Time.deltaTime);
				if (addedVelocity.y == 0)
					addedVelocity += new Vector3(0, -1 * SLOW_DOWN_RATE * playerVelocityVector.y * (float)Time.deltaTime, 0);

				// if we are moving
				if (addedVelocity.sqrMagnitude != 0)
				{
					// Rotate our velocity Vector and rotate the added velocity to the camera
					Vector3 rotatedVelocity = rotateX * playerVelocityVector;
					addedVelocity = rotateX * addedVelocity;

					float gamma = (float)state.sqrtOneMinusVSquaredCWDividedByCSquared;
					rotatedVelocity = (1 / (1 + (rotatedVelocity.x * addedVelocity.x) / (float)state.cSqrd)) *
						(new Vector3(addedVelocity.x + rotatedVelocity.x, addedVelocity.y * gamma, gamma * addedVelocity.z));

					//Unrotate our new total velocity
					rotatedVelocity = unRotateX * rotatedVelocity;
					state.PlayerVelocityVector = rotatedVelocity;
				}

				// CHANGE the speed of light (N, M)
				int temp2 = (int)(Input.GetAxis("Speed of Light"));
				if (temp2 < 0 && speedOfLightTarget <= state.MaxSpeed)
				{
					temp2 = 0;
					speedOfLightTarget = (int)state.MaxSpeed;
				}
				if (temp2 != 0)
				{
					speedOfLightTarget += temp2;

					speedOfLightStep = Mathf.Abs((float)(state.SpeedOfLight - speedOfLightTarget) / 20);
				}

				// Smooth damp speed of light to target
				if (state.SpeedOfLight < speedOfLightTarget * .995)
					state.SpeedOfLight += speedOfLightStep;
				else if (state.SpeedOfLight > speedOfLightTarget * 1.005)
					state.SpeedOfLight -= speedOfLightStep;
				else if (state.SpeedOfLight != speedOfLightTarget)
					state.SpeedOfLight = speedOfLightTarget;

				//MOUSE CONTROLS

				//X axis position change, Y axis position change
				float positionChangeX = -(float)Input.GetAxis("Mouse X");
				float positionChangeY = (float)inverted * Input.GetAxis("Mouse Y");

				float viewRotY = 0;
				if (Mathf.Abs(positionChangeX) <= 1 && Mathf.Abs(positionChangeY) <= 1)
				{
					viewRotX = (float)(-positionChangeX * Time.deltaTime * rotSpeed * mouseSensitivity * controllerBoost);
					viewRotY = (float)(positionChangeY * Time.deltaTime * rotSpeed * mouseSensitivity * controllerBoost);
				}
				else
				{
					viewRotX = (float)(-positionChangeX * Time.deltaTime * rotSpeed * mouseSensitivity);
					viewRotY = (float)(positionChangeY * Time.deltaTime * rotSpeed * mouseSensitivity);
				}

				//  For initialization, it spins if it is kept on
				if (frames > INIT_FRAME_WAIT)
				{
					camTransform.Rotate(new Vector3(0, viewRotX, 0), Space.World);
					if ((camTransform.eulerAngles.x + viewRotY < 90 && camTransform.eulerAngles.x + viewRotY > 90 - 180) || (camTransform.eulerAngles.x + viewRotY > 270 && camTransform.eulerAngles.x + viewRotY < 270 + 180))
					{
						camTransform.Rotate(new Vector3(viewRotY, 0, 0));
					}
				}
				else
				{
					frames++;
				}

				//If we have a speed of light less than max speed, fix it. (should never happen)
				if (state.SpeedOfLight < state.MaxSpeed)
					state.SpeedOfLight = state.MaxSpeed;
			}
		}
	}
}
