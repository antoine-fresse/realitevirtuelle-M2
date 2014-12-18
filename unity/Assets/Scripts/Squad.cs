using UnityEngine;
using System.Collections;

public delegate void OnAllDeadHandler();

public class Squad : MonoBehaviour {

	private bool _done;

	public event OnAllDeadHandler OnAllDead;


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
		if (transform.childCount != 0 || _done) return;

		if (OnAllDead != null) OnAllDead();
		_done = true;
	}
}
