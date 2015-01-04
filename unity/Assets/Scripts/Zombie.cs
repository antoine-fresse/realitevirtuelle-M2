using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Zombie : MonoBehaviour {

	public bool CanMove = false;
    private GameObject particleSystem;

    public int MaxLife = 5;

    public int _life;

	private Animator _animator;
	private NavMeshAgent _agent;
    private Collider _collider;
    private Rigidbody _body;

	private float _attackCD = 0.0f;

    FMOD.Studio.ParameterInstance paramFmod;


	void Awake () {
        particleSystem = transform.Find("ParticleSystem").gameObject;
        particleSystem.GetComponent<ParticleSystem>().Stop();
	    _life = MaxLife;
		_animator = GetComponent<Animator>();
		_agent = GetComponent<NavMeshAgent>();
        _body = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
	}

	void FixedUpdate() {
        if (_life > 0) {
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

            dir.Normalize();

            if (CanMove) {
                _agent.SetDestination(player.transform.position);
                _animator.SetFloat("speed", _agent.velocity.magnitude);
            }

            if (dist <= 3.0f) {
                if (_attackCD <= .0f) {
                    _attackCD = 2f;
                    _animator.SetFloat("attackBlend", Random.Range(0.0f, 1.0f));
                    _animator.SetTrigger("attack");
                    StartCoroutine(DoAttack(0.5f));
                }
            }
        }
	}

	IEnumerator DoAttack(float delay) {
		yield return new WaitForSeconds(delay);

        if (_life > 0) {
            var attackPos = transform.position + transform.up * 1.0f + transform.forward * 0.7f;
            var radius = 1.0f;
            var hits = Physics.OverlapSphere(attackPos, radius);
            /*Debug.DrawLine(attackPos, attackPos + transform.up*radius, Color.red, 0.5f);
            Debug.DrawLine(attackPos, attackPos + transform.right * radius, Color.red, 0.5f);
            Debug.DrawLine(attackPos, attackPos + transform.forward * radius, Color.red, 0.5f);*/
            foreach (var hit in hits) {
                if (hit.gameObject.tag.Equals("Player"))
                    hit.gameObject.GetComponent<Player>().OnHit();
            }
        }
	}


    public void OnHit(Vector3 point, Vector3 normal) {
        if (_life > 0) {
            _life -= 1;
            Quaternion quat = new Quaternion();
            quat.SetLookRotation(normal);
            particleSystem.transform.localRotation = quat;
            particleSystem.transform.localPosition = transform.InverseTransformPoint(point);
            particleSystem.GetComponent<ParticleSystem>().Play();

            if (_life == 0) {
                FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/ZombieDeath", transform.position);

                _body.useGravity = false;
                _collider.enabled = false;
                _agent.enabled = false;
                _animator.enabled = false;
                StartCoroutine(WaitAndDestroy());

            } else {
                FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/ZombieHit", transform.position);
            }
        }
    }

    private IEnumerator WaitAndDestroy() {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }
}
