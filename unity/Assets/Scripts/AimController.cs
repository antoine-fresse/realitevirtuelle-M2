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


	void Update ()
	{
		ajoutY = Mathf.Clamp(ajoutY + Input.GetAxis("AimX") * sensitivityX * Time.deltaTime, -rangeX, rangeX);
		ajoutX = Mathf.Clamp(ajoutX + Input.GetAxis("AimY") * sensitivityY * Time.deltaTime, -rangeY, rangeY);

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