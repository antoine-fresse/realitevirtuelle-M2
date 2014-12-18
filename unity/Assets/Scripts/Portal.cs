using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public GameObject destination;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        SplineWalker walker = other.gameObject.GetComponent<SplineWalker>();
        if (walker != null) {
            walker.stopCurrentSpline();
        }
        other.gameObject.transform.position = destination.transform.position;
        other.gameObject.transform.rotation = destination.transform.rotation;

        if (walker != null) {
            walker.nextSpline();
        }
    }
}
