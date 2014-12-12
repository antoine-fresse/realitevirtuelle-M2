using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

    private GameObject particleSystem;

    public int MaxLife = 5;

    private int _life;

	void Start () {
        particleSystem = transform.Find("ParticleSystem").gameObject;
        particleSystem.GetComponent<ParticleSystem>().Stop();
	    _life = MaxLife;
	}


    public void OnHit(Vector3 point, Vector3 normal) {
        _life -= 1;
        Quaternion quat = new Quaternion();
        quat.SetLookRotation(normal);
        particleSystem.transform.localRotation = quat;
        particleSystem.transform.localPosition = transform.InverseTransformPoint(point);
        particleSystem.GetComponent<ParticleSystem>().Play();

        if (_life == 0) {
            Destroy(gameObject);
        }
    }
}
