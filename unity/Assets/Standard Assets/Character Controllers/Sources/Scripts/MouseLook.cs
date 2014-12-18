using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public static MouseLook Instance { get; private set; }

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	public float forceX = 0f;
	public float forceY = 0f;

	float rotationY = 0F;

	public float Shake = 0.0f;

	void Awake() {
		Instance = this;
	}

	void Update ()
	{

		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Horizontal") * sensitivityX + forceX;
			
		rotationY += Input.GetAxis("Vertical") * sensitivityY + forceY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

		var shakeVec = new Vector3(Shake * Mathf.Sin(20f * Time.time), Shake * Mathf.Sin(40f * Time.time), 0);
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0) + shakeVec;

		Shake = Mathf.Max(0f, Shake - Time.deltaTime*10f);
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}