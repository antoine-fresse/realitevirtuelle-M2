using UnityEngine;

public class Spawner : MonoBehaviour {

    public int size;
    public int number;
    public GameObject prefab;
    public float rate;

    private float timeSinceLastSpawn;
    private int numberLeft;
    public bool started = true;

	// Use this for initialization
	void Start () {
        numberLeft = number;
        timeSinceLastSpawn = (1.0f / rate);
	}
	
	// Update is called once per frame
	void Update () {
        if (started) {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= (1.0f/rate)) {
                numberLeft--;
                timeSinceLastSpawn = 0f;

                float x = (Random.value * 2f * size) - size;
                float z = (Random.value * 2f * size) - size;
                Vector3 pos = transform.position + new Vector3(x, 0, z);
               
                var t = ((GameObject)Instantiate(prefab, pos, Quaternion.identity)).transform;
				t.SetParent(transform.parent, true);
	            t.GetComponent<Zombie>().CanMove = true;
            }
        }
		if (numberLeft == 0) {
			Destroy(gameObject);
		}
	}

    public void Spawn() {
        started = true;
    }
}
