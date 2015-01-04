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

	private bool _next = false;

	public Squad[] AssociatedSquads;

    private FMOD.Studio.ParameterInstance footstepParam;

	void Awake() {
		Instance = this;
	}

	public static SplineWalker Instance { get; private set; }

	private void Start() {
        splines = new List<BezierSpline>();
        foreach (Transform child in splineGameObject.transform) {
            BezierSpline s = child.gameObject.GetComponent<BezierSpline>();
            if (s != null) {
                splines.Add(s);
            }
        }
        FMOD_StudioEventEmitter[] emitters = GetComponents<FMOD_StudioEventEmitter>();
        for (int i = 0; i < emitters.Length; i++) {
            emitters[i].Play();
            footstepParam = emitters[i].getParameter("speed");
            if (footstepParam != null) {
                footstepParam.setValue(0.0f);
                break;
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
                if (footstepParam != null) {
                    footstepParam.setValue(0.0f);
                }
                currentSpline.complete();
                currentSpline = null;
                progress = 0f;
                currentSplineIndex++;
	            if (_next) {
		            nextSpline();
	            }
            } else if (footstepParam != null) {
                footstepParam.setValue(1.0f);
            }
        }
        if (Input.GetKeyDown("space")) {
            nextSpline();
        }
	}

    public void nextSpline() {
		_next = true;
        if ((currentSplineIndex < splines.Count) && (currentSpline == null)) {
			if(currentSplineIndex<AssociatedSquads.Length)
				AssociatedSquads[currentSplineIndex].ActivateSquad();

            currentSpline = splines[currentSplineIndex];
			_next = false;
        }
    }

    public void stopCurrentSpline() {
        currentSpline = null;
        progress = 0f;
        currentSplineIndex++;
    }
}