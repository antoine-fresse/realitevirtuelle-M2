using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Shooter : MonoBehaviour {

    public float FireRate = 0.2f;
    public LayerMask CanShoot;

    public ParticleEmitter Emitter;
    public Light MuzzleFlash;
    private float _timerFire ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButton(0) && _timerFire <= 0.0f) {

	        MuzzleFlash.DOIntensity(3f, FireRate*0.5f).From();
	        Emitter.Emit();

	        transform.DOPunchPosition(transform.forward/10f, FireRate*0.9f);
	        _timerFire = FireRate;

            //Debug.DrawRay(transform.position+new Vector3(0f,0.1f,0f), transform.forward*-100f, Color.red, FireRate*2f);

	        var hits = Physics.RaycastAll(transform.position, -transform.forward, 100f, CanShoot);

	        foreach (var hit in hits) {
	            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Zombies")) {
	                hit.collider.gameObject.GetComponent<Zombie>().OnHit();
	            }
	        }
	    }

	    if (_timerFire > 0.0f) {
	        _timerFire -= Time.deltaTime;
	    }
	}
}
