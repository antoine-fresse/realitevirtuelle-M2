using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int MaxLife = 10;

	private int _life;

    FMOD.Studio.EventInstance muffledSound;
    FMOD.Studio.EventInstance normalSound;

    private GameManager _gameManager;

	// Use this for initialization
	void Start () {
		_life = MaxLife;

        normalSound = FMOD_StudioSystem.instance.GetEvent("snapshot:/NormalSound");
        muffledSound = FMOD_StudioSystem.instance.GetEvent("snapshot:/MuffleSound");
        normalSound.start();

        GameObject go = GameObject.Find("GameManager");
        if (go != null) {
            _gameManager = go.GetComponent<GameManager>();
        }
	}

    void Update() {
        if (_gameManager != null) {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50.0f);
            bool found = false;
            for (int i = 0; i < hitColliders.Length; i++) {
                Zombie zombie = hitColliders[i].gameObject.GetComponent<Zombie>();
                if ((zombie != null) && (zombie.CanMove)) {
                    found = true;
                    break;
                }
            }
            print("setFightMode : " + found);
            _gameManager.setFightMode(found);
        }
        
    }

	public void OnHit() {
		_life--;
        if (_life <= 3) {
            normalSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            muffledSound.start();
        } else {
            muffledSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            normalSound.start();
        }
        FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/CivilianHit", transform.position);
		MouseLook.Instance.Shake = 2f;
		if (_life == 0) {
			// TODO Game Over
			Debug.Log("Game Over !");
		}
	}
}
