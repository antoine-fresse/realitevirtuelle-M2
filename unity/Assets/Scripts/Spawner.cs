using UnityEngine;

public class Spawner : MonoBehaviour {

    public int size;
    public int number;
    public GameObject prefab;
    public float rate;

    private float timeSinceLastSpawn;
    private int numberLeft;
    private bool started;

	// Use this for initialization
	void Start () {
        numberLeft = number;
        timeSinceLastSpawn = (1.0f / rate);
        started = false;
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
                Vector3 pos = transform.localPosition + new Vector3(x, 0, z);
                print(pos);
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
	}

    public void Spawn() {
        started = true;
    }
}
