using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Shooter : MonoBehaviour {

    public float FireRate = 0.2f;
    public LayerMask CanShoot;
    public ParticleEmitter Emitter;
    public Light MuzzleFlash;
	public Transform Trail;

	private float _timerFire;
	
	// Use this for initialization
	void Start () {
		_timerFire = FireRate;
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButton(0) && _timerFire <= 0.0f) {

	        MuzzleFlash.DOIntensity(3f, FireRate*0.5f).From();
	        Emitter.Emit();

			FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/Fire", transform.position);
			

	        transform.DOPunchPosition(transform.forward/10f, FireRate*0.9f);
	        _timerFire = FireRate;

            //Debug.DrawRay(transform.position+new Vector3(0f,0.1f,0f), transform.forward*-100f, Color.red, FireRate*2f);


			var newTrail = ((Transform)Instantiate(Trail, Trail.position, Quaternion.identity)).GetComponent<TrailRenderer>();

			newTrail.enabled = true;
			newTrail.time = FireRate * 2f;
			newTrail.autodestruct = true;
		    
			newTrail.transform.DOMove(transform.position - transform.forward * 100f, FireRate).From();

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
