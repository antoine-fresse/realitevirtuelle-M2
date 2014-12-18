using UnityEngine;
using System.Collections.Generic;

public class SplineWalker : MonoBehaviour {

	public GameObject splineGameObject;

    private List<BezierSpline> splines;

    private BezierSpline currentSpline;
    private int currentSplineIndex = 0;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress;
	private bool goingForward = true;

	public Squad[] AssociatedSquads;

    private void Start() {
        splines = new List<BezierSpline>();
        foreach (Transform child in splineGameObject.transform) {
            BezierSpline s = child.gameObject.GetComponent<BezierSpline>();
            if (s != null) {
                splines.Add(s);
            }
        }
    }

	private void Update () {
        if (currentSpline != null) {
            if (goingForward) {
                progress += Time.deltaTime / duration;
                if (progress > 1f) {
                    if (mode == SplineWalkerMode.Once) {
                        progress = 1f;
                    } else if (mode == SplineWalkerMode.Loop) {
                        progress -= 1f;
                    } else {
                        progress = 2f - progress;
                        goingForward = false;
                    }
                }
            } else {
                progress -= Time.deltaTime / duration;
                if (progress < 0f) {
                    progress = -progress;
                    goingForward = true;
                }
            }
            Vector3 position = currentSpline.GetPoint(progress);
            transform.localPosition = position;
            if (lookForward) {
                transform.LookAt(position + currentSpline.GetDirection(progress));
            }
            if (progress >= 1f) {
                currentSpline = null;
                progress = 0f;
                currentSplineIndex++;
            }
        }
        if (Input.GetKeyDown("space")) {
            nextSpline();
        }
	}

    public void nextSpline() {
        if ((currentSplineIndex < splines.Count) && (currentSpline == null)) {
			if(currentSplineIndex<AssociatedSquads.Length)
				AssociatedSquads[currentSplineIndex].ActivateSquad();

            currentSpline = splines[currentSplineIndex];
        }
    }

    public void stopCurrentSpline() {
        currentSpline = null;
        progress = 0f;
        currentSplineIndex++;
    }
}