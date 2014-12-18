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
	    if ((Fire||Input.GetMouseButton(0)) && _timerFire <= 0.0f) {
		    HasFired = true;
	        MuzzleFlash.DOIntensity(3f, FireRate*0.5f).From();
		    MuzzleFlash.DOColor(Color.yellow, FireRate*0.5f).From();
	        Emitter.Emit();

			FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/Fire", transform.position);
			

	        transform.DOPunchPosition(transform.forward/10f, FireRate*0.9f);
	        _timerFire = FireRate;

            


			var newTrail = ((Transform)Instantiate(Trail, Trail.position, Quaternion.identity)).GetComponent<TrailRenderer>();

			newTrail.enabled = true;
			newTrail.time = FireRate ;
			newTrail.autodestruct = true;

		    newTrail.transform.DOMove(transform.position - transform.forward*10f, FireRate);

			var hits = Physics.RaycastAll(transform.parent.position, transform.parent.forward, 100f, CanShoot);

			//Debug.DrawRay(transform.parent.position, transform.parent.forward*100f, Color.red, 1.0f);

		    var minDist = 99999f;
		    bool found = false;
		    var z = new RaycastHit();
			foreach(var hit in hits) {
				if ((hit.point - transform.parent.position).magnitude < minDist)
					minDist = (hit.point - transform.parent.position).magnitude;
				
	            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Zombies")) {
		            if (found) {
			            if ((hit.point - transform.parent.position).magnitude < (z.point - transform.parent.position).magnitude) {
				            z = hit;
			            }
		            }else
						z = hit;
		            found = true;
	            }
	        }
		    if (found) {
			    if((z.point - transform.parent.position).magnitude <= minDist)
					z.collider.gameObject.GetComponent<Zombie>().OnHit(z.point, z.normal);
		    }
	    }

	    if (_timerFire > 0.0f) {
	        _timerFire -= Time.deltaTime;
	    }
	}
}
