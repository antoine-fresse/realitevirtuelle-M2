using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Zombie : MonoBehaviour {

	public bool CanMove = false;
    private GameObject particleSystem;

    public int MaxLife = 5;

    private int _life;

	private Animator _animator;
	private NavMeshAgent _agent;

	private float _attackCD = 0.0f;


	void Start () {
        particleSystem = transform.Find("ParticleSystem").gameObject;
        particleSystem.GetComponent<ParticleSystem>().Stop();
	    _life = MaxLife;
		_animator = GetComponent<Animator>();
		_agent = GetComponent<NavMeshAgent>();
	}

	void FixedUpdate() {
		_attackCD -= Time.fixedDeltaTime;
		var player = GameObject.FindGameObjectWithTag("Player");

		if (!player) {
			_animator.SetFloat("speed", 0f);
			_agent.Stop();
			return;
		}

		var dir = (player.transform.position - transform.position);
		dir.y = 0;
		var dist = dir.magnitude;

		if (CanMove) {
			_agent.SetDestination(player.transform.position);
			_animator.SetFloat("speed", _agent.velocity.magnitude);
		}
		
		if (dist <= 1.5f) {
			if (_attackCD <= .0f) {
				_attackCD = 2f;
				_animator.SetFloat("attackBlend", Random.Range(0.0f,1.0f));
				_animator.SetTrigger("attack");
				StartCoroutine(DoAttack(0.5f));
			}
		}
		
	}

	IEnumerator DoAttack(float delay) {
		yield return new WaitForSeconds(delay);

		var attackPos = transform.position + transform.up*1.0f + transform.forward*0.7f;
		var radius = 0.7f;
		var hits = Physics.OverlapSphere(attackPos, radius);
		/*Debug.DrawLine(attackPos, attackPos + transform.up*radius, Color.red, 0.5f);
		Debug.DrawLine(attackPos, attackPos + transform.right * radius, Color.red, 0.5f);
		Debug.DrawLine(attackPos, attackPos + transform.forward * radius, Color.red, 0.5f);*/
		foreach (var hit in hits) {
			if(hit.gameObject.tag.Equals("Player"))
				hit.gameObject.GetComponent<Player>().OnHit();
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
