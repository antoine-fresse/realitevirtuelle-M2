using UnityEngine;
using System.Collections;

public delegate void OnAllDeadHandler();

public class Squad : MonoBehaviour {


	public bool isFinalSquad = false;
	public int nextLevel = 2;

	public void ActivateSquad() {
		foreach(Transform t in transform) {
			var z = t.gameObject.GetComponent<Zombie>();
			if (z)
				z.CanMove = true;

			var s = t.gameObject.GetComponent<Spawner>();
			if (s) 
				s.started = true;
		}
	}

	public void DeactivateSquad() {
		foreach (Transform t in transform) {
			var z = t.gameObject.GetComponent<Zombie>();
			if (z)
				z.CanMove = false;

			var s = t.gameObject.GetComponent<Spawner>();
			if (s) 
				s.started = false;
		}
	}

	void Update() {
        if (isFinalSquad && Input.GetKeyDown(KeyCode.P))
            Application.LoadLevel(nextLevel);

		if (transform.childCount != 0) return;

		if (isFinalSquad)
			Application.LoadLevel(nextLevel);
		Destroy(gameObject);
		SplineWalker.Instance.nextSpline();
	}
}
