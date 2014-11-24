using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	private float _cnt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.CompareTag("Player"))
			_cnt = 0f;
	}

	void OnCollisionStay(Collision c) {
		if (c.gameObject.CompareTag("Player")) {
			_cnt += Time.deltaTime;
			if(_cnt > 1.0f)
				Debug.Log("Perdu");
		}
			
	}

}
