using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Shooter : MonoBehaviour {


	public bool Fire = false;
    public float FireRate = 0.2f;
    public LayerMask CanShoot;
    public ParticleEmitter Emitter;
    public Light MuzzleFlash;
	public Transform Trail;

	public bool HasFired = false;
	private float _timerFire;
	
	// Use this for initialization
	void Start () {
		_timerFire = FireRate;
		
	}
	
	// Update is called once per frame
	void Update () {
		HasFired = false;
	    if (Fire && _timerFire <= 0.0f) {
		    HasFired = true;
	        MuzzleFlash.DOIntensity(3f, FireRate*0.5f).From();
	        Emitter.Emit();

			FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/Fire", transform.position);
			

	        transform.DOPunchPosition(transform.forward/10f, FireRate*0.9f);
	        _timerFire = FireRate;

            //Debug.DrawRay(transform.position+new Vector3(0f,0.1f,0f), transform.forward*-100f, Color.red, FireRate*2f);


			var newTrail = ((Transform)Instantiate(Trail, Trail.position, Quaternion.identity)).GetComponent<TrailRenderer>();

			newTrail.enabled = true;
			newTrail.time = FireRate ;
			newTrail.autodestruct = true;

		    newTrail.transform.DOMove(transform.position - transform.forward*10f, FireRate);

	        var hits = Physics.RaycastAll(transform.position, -transform.forward, 100f, CanShoot);
			
	        foreach (var hit in hits) {
	            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Zombies")) {
	                hit.collider.gameObject.GetComponent<Zombie>().OnHit(hit.point, hit.normal);
	            }
	        }
	    }

	    if (_timerFire > 0.0f) {
	        _timerFire -= Time.deltaTime;
	    }
	}
}
