using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {


    public int MaxLife = 5;

    private int _life;

	void Start () {
	    _life = MaxLife;
	}


    public void OnHit() {
        _life -= 1;
        if (_life == 0) {
            Destroy(gameObject);
        }
    }
}
