using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour {

	public float sensitivityX = 60f;
	public float sensitivityY = 60f;
	public float rangeX = 20F;
	public float rangeY = 20F;

	public RectTransform Cursor;
	public float baseX = 0f;
	public float baseY = 180f;

	private float ajoutX = 0f;
	private float ajoutY = 0f;
	public MouseLook cameraControl;


	public float AimX = 0f;
	public float AimY = 0f;

	void Update ()
	{

		/*
		 * AimX = Input.GetAxis("AimX");
		 * AimY = Input.GetAxis("AimY");
		 */
/*

		ajoutY = Mathf.Clamp(ajoutY +  AimX * sensitivityX * Time.deltaTime, -rangeX, rangeX);
		ajoutX = Mathf.Clamp(ajoutX + AimY * sensitivityY * Time.deltaTime, -rangeY, rangeY);
*/
		ajoutX = Mathf.Clamp(AimY * sensitivityX, -rangeY, rangeY);
		ajoutY = Mathf.Clamp(AimX * sensitivityY, -rangeX, rangeX);

		cameraControl.forceX = Mathf.Abs(ajoutY) >= rangeX ? Mathf.Sign(ajoutY) : 0f;
		cameraControl.forceY = Mathf.Abs(ajoutX) >= rangeY ? Mathf.Sign(-ajoutX) : 0f;
	
		transform.localEulerAngles = new Vector3(baseX - ajoutX, baseY + ajoutY, 0);


		var v = Camera.mainCamera.WorldToScreenPoint(transform.position + transform.forward*1000f );
		Cursor.anchoredPosition = new Vector2(v.x, v.y);
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}