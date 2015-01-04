using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int MaxLife = 10;

	private int _life;
	// Use this for initialization
	void Start () {
		_life = MaxLife;
	}

	public void OnHit() {
		_life--;
        FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/CivilianHit", transform.position);
		MouseLook.Instance.Shake = 2f;
		if (_life == 0) {
			// TODO Game Over
			Debug.Log("Game Over !");
		}
	}
}
