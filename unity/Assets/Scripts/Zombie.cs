using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Zombie : MonoBehaviour {

    private GameObject particleSystem;

    public int MaxLife = 5;

    private int _life;

	private Animator _animator;

	void Start () {
        particleSystem = transform.Find("ParticleSystem").gameObject;
        particleSystem.GetComponent<ParticleSystem>().Stop();
	    _life = MaxLife;
		_animator = GetComponent<Animator>();
	}

	void FixedUpdate() {
		var player = GameObject.FindGameObjectWithTag("Player");
		var angles = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles;

		angles.x = transform.eulerAngles.x;
		angles.z = transform.eulerAngles.z;

		transform.eulerAngles = new Vector3(angles.x, Mathf.LerpAngle(transform.eulerAngles.y, angles.y,Time.fixedDeltaTime*2f), angles.z);

		var dir = (player.transform.position - transform.position);
		dir.y = 0;
		var dist = dir.magnitude;


		if (dist > 1f) {
			rigidbody.velocity = transform.forward*2f + new Vector3(0, rigidbody.velocity.y, 0);
			_animator.SetBool("running", true);
		}
		else {
			rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0);
			_animator.SetBool("running", false);
		}
		
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
